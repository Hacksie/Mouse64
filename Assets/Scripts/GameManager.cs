
using System.IO;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

namespace HackedDesign
{
    public class GameManager : MonoBehaviour
    {
        public const string gameVersion = "1.0";
        [Header("Game")]
        [SerializeField] private PlayerController playerController = null;
        [SerializeField] private LevelRenderer levelRenderer = null;
        [SerializeField] private EntityPool entityPool = null;
        [SerializeField] private UnityEngine.Audio.AudioMixer mixer = null;
        [SerializeField] private PlayerPreferences preferences = null;
        [SerializeField] private bool invulnerability = true;
        [SerializeField] private Light2D globalLight = null;
        [SerializeField] private Color defaultLightColor = Color.gray;
        [SerializeField] private Color alertLightColor = Color.red;
        [SerializeField] private int alertGuards = 10;
        [SerializeField] private ParticleSystem particlesSelect = null;
        [SerializeField] private ParticleSystem particlesLeft = null;
        [SerializeField] private ParticleSystem particlesRight = null;
        [SerializeField] private bool isRandom = false;

        [Header("Data")]
        [SerializeField] public float easyAdj = 1.0f;
        [SerializeField] public float mediumAdj = 0.8f;
        [SerializeField] public float hardAdj = 0.6f;
        [SerializeField] public int currentSlot = 0;
        [SerializeField] public List<GameData> gameSlots = new List<GameData>(3);
        [SerializeField] public GameData randomGameSlot = new GameData();
        [SerializeField] private Level[] levels = null;
        [SerializeField] private string[] corpList = null;
        [SerializeField] private string[] nameList = null;

        [Header("UI")]
        [SerializeField] private GameObject gameCanvas = null;
        [SerializeField] private GameObject menuCanvas = null;
        [SerializeField] private UI.MainMenuPresenter mainMenuPanel = null;
        [SerializeField] private UI.StartMenuPresenter startMenuPanel = null;
        [SerializeField] private UI.HudPresenter hudPanel = null;
        [SerializeField] private UI.DeadPresenter deadPanel = null;
        [SerializeField] private UI.DialogPresenter dialogPanel = null;
        [SerializeField] private UI.MissionPresenter missionPanel = null;
        [SerializeField] private UI.MissionCompletePresenter missionCompletePanel = null;
        [SerializeField] private UI.LevelPresenter levelPanel = null;
        [SerializeField] private UI.GameOverPresenter gameOverPanel = null;

        private IState currentState;

        public static GameManager Instance { get; private set; }

        public bool RandomGame { get { return isRandom; } set { isRandom = value; } }
        public GameData Data { get { return isRandom ? randomGameSlot : this.gameSlots[this.currentSlot]; } private set { if (isRandom) { randomGameSlot = value; } else { this.gameSlots[this.currentSlot] = value; } } }
        public PlayerController Player { get { return playerController; } private set { playerController = value; } }
        public EntityPool EntityPool { get { return entityPool; } private set { entityPool = value; } }
        public LevelRenderer LevelRenderer { get { return levelRenderer; } private set { levelRenderer = value; } }
        public PlayerPreferences PlayerPreferences { get { return preferences; } private set { preferences = value; } }
        public ParticleSystem ParticlesSelect { get { return particlesSelect; } private set { particlesSelect = value; } }
        public ParticleSystem ParticlesLeft { get { return particlesLeft; } private set { particlesLeft = value; } }
        public ParticleSystem ParticlesRight { get { return particlesRight; } private set { particlesRight = value; } }
        public IState CurrentState
        {
            get
            {
                return this.currentState;
            }
            private set
            {
                if (currentState != null)
                {
                    this.currentState.End();
                }
                this.currentState = value;
                if (this.currentState != null)
                {
                    this.currentState.Begin();
                }
            }
        }

        private GameManager() => Instance = this;

        void Awake()
        {
            CheckBindings();

        }

        void Start()
        {
            Initialization();
        }

        private void Update() => CurrentState.Update();
        private void LateUpdate() => CurrentState.LateUpdate();
        private void FixedUpdate() => CurrentState.FixedUpdate();

        public void SetMainMenu() => CurrentState = new MainMenuState(this.mainMenuPanel);
        public void SetMissionSelect() => CurrentState = new MissionSelectState(this.playerController, this.entityPool, this.levelRenderer, this.dialogPanel, this.missionPanel, this.levelPanel);
        public void SetMissionComplete() => CurrentState = new MissionCompleteState(this.playerController, this.missionCompletePanel);
        public void SetPlaying() => CurrentState = new PlayingState(this.playerController, this.entityPool, this.levelRenderer, this.hudPanel);
        public void SetStartMenu() => CurrentState = new StartMenuState(this.hudPanel, this.startMenuPanel);
        public void SetDead() => CurrentState = new DeadState(this.playerController, this.deadPanel);
        public void SetGameOver() => CurrentState = new GameOverState(this.playerController, this.entityPool, this.levelRenderer, this.gameOverPanel);
        public void SetQuit() => Application.Quit();

        public void LoadSlots()
        {
            this.gameSlots = new List<GameData>(3) { null, null, null };
            this.gameSlots[0] = LoadSaveFile(0);
            this.gameSlots[1] = LoadSaveFile(1);
            this.gameSlots[2] = LoadSaveFile(2);
        }

        public void SaveGame()
        {
            Data.saveName = System.DateTime.Now.ToString("yyddMM HHmm");
            Logger.Log(this, "Saving state ", Data.saveName);
            string json = JsonUtility.ToJson(Data);
            string path = Path.Combine(Application.persistentDataPath, $"SaveFile{currentSlot}.json");
            File.WriteAllText(path, json);
            Logger.Log(this, "Saved ", path);
        }

        GameData LoadSaveFile(int slot)
        {
            var path = Path.Combine(Application.persistentDataPath, $"SaveFile{slot}.json");
            Logger.Log(name, "Attempting to load ", path);
            if (File.Exists(path))
            {
                Logger.Log(name, "Loading ", path);
                var contents = File.ReadAllText(path);
                return JsonUtility.FromJson<GameData>(contents);
            }
            else
            {
                Logger.Log(name, "Save file does not exist ", path);
            }

            return null;
        }

        public void NewGame()
        {
            this.gameSlots[this.currentSlot] = new GameData()
            {
                newGame = false,
                gameVersion = gameVersion,
                gameSlot = this.currentSlot,
                health = 100,
                energy = 100,
                bullets = 6,
                timer = 64,
                alert = 0,
                score = 0,
                currentLevelIndex = 0,
                seed = (int)System.DateTime.Now.Ticks,
                currentLevel = levels[0]
            };

            Data.currentLevel.corp = GetRandomCorp();
            Data.currentLevel.target = ((char)(Random.Range(0, 26) + 65)) + "." + GetRandomName();

            this.playerController.Reset();
        }

        public void NewRandomGame(int seed, string difficulty)
        {
            Logger.Log(this, "new random game ", seed.ToString(), difficulty);

            Random.InitState(seed);

            this.randomGameSlot = new GameData()
            {
                newGame = false,
                gameVersion = gameVersion,
                gameSlot = -1,
                health = 100,
                energy = 100,
                bullets = 6,
                timer = 64,
                alert = 0,
                score = 0,
                currentLevelIndex = 0,
                seed = seed
            };

            int length = 24;
            int security = 0;
            int openGuards = 0;
            int drones = 0;
            int gcannon = 0;
            int wcannon = 0;
            int rcannon = 0;
            int doors = 0;
            int maxAlert = 0;
            LevelAlertSpawn alertSpawn = LevelAlertSpawn.Guard;

            switch(difficulty)
            {
                case "Hard":
                length = 26;
                security = Random.Range(4,6);
                openGuards = Random.Range(3, 6);
                drones = Random.Range(1, 4);
                gcannon = Random.Range(0, 3);
                wcannon = Random.Range(0, 3);
                rcannon = Random.Range(0, 3);
                doors = Random.Range(7, 10);
                maxAlert = 5;
                alertSpawn = LevelAlertSpawn.Any;
                break;
                case "Medium":
                length = 24;
                security = Random.Range(2,4);
                openGuards = Random.Range(2, 5);
                drones = Random.Range(0, 3);
                gcannon = Random.Range(0, 2);
                wcannon = Random.Range(0, 2);
                rcannon = Random.Range(0, 2);
                doors = Random.Range(8, 11);
                maxAlert = 4;
                alertSpawn = LevelAlertSpawn.Guard;
                break;
                case "Easy":
                default:
                length = 24;
                security = Random.Range(1,4);
                openGuards = Random.Range(2, 4);
                drones = Random.Range(0, 2);
                gcannon = Random.Range(0, 1);
                wcannon = Random.Range(0, 1);
                rcannon = Random.Range(0, 2);
                doors = Random.Range(8, 12);
                maxAlert = 3;
                alertSpawn = LevelAlertSpawn.Guard;                
                break;
            }

            Data.currentLevel = new Level()
            {
                corp = GetRandomCorp(),
                target = ((char)(Random.Range(0, 26) + 65)) + "." + GetRandomName(),
                difficulty = difficulty,
                length = length,
                security = security,
                openGuards = openGuards,
                drones = drones,
                gcannon = gcannon,
                wcannon = wcannon,
                rcannon = rcannon,
                doors = doors,
                maxAlert = maxAlert,
                alertSpawn = alertSpawn
            };
            

            this.globalLight.color = defaultLightColor;
            this.entityPool.DestroyEntities();
            this.playerController.Reset();
        }

        public void Reset()
        {
            Data.bullets = 6;
            Data.health = 0;
            Data.energy = 100;
            Data.alert = 0;
            Data.timer = Data.currentLevel.window;
            this.globalLight.color = defaultLightColor;
            //this.entityPool.DestroyEntities();
            this.playerController.Reset();
        }

        public void NextLevel()
        {
            ++Data.currentLevelIndex;

            if (Data.currentLevelIndex >= levels.Length)
            {
                SetGameOver();
            }
            else if (Data.currentLevelIndex == (levels.Length - 1))
            {
                Data.currentLevel = levels[Data.currentLevelIndex];
                Data.seed = (int)System.DateTime.Now.Ticks;

                Data.currentLevel.corp = "Arisana";
                Data.currentLevel.target = "G.Booker";
                GameManager.Instance.SetMissionSelect();
            }
            else
            {
                Data.currentLevel = levels[Data.currentLevelIndex];
                Data.seed = (int)System.DateTime.Now.Ticks;

                Data.currentLevel.corp = GetRandomCorp();
                Data.currentLevel.target = ((char)(Random.Range(0, 26) + 65)) + "." + GetRandomName();
                GameManager.Instance.SetMissionSelect();
            }

        }

        public float DifficultyAdjustment()
        {
            switch (Data.currentLevel.difficulty)
            {
                case "Easy":
                    return easyAdj;
                case "Medium":
                    return mediumAdj;
                case "Hard":
                    return hardAdj;
            }

            return 1;
        }

        public string GetRandomCorp()
        {
            return this.corpList[Random.Range(0, this.corpList.Length)];
        }

        public string GetRandomName()
        {
            return this.nameList[Random.Range(0, this.nameList.Length)];
        }

        public void ConsumeBullet(int amount)
        {
            Data.bullets = Mathf.Clamp(Data.bullets - amount, 0, Data.maxBullets);
        }

        public void ConsumeStealth(float amount)
        {
            Data.energy = Mathf.Clamp(Data.energy - amount, 0, Data.maxEnergy);
        }

        public void TimeUp()
        {
            if (Data.alertTriggered)
            {
                return;
            }

            this.globalLight.color = alertLightColor;
            Data.alertTriggered = true;
            for (int i = 0; i < this.alertGuards; i++)
            {
                var position = entityPool.FindGuardSpawn(this.playerController.transform.position);
                entityPool.SpawnGuard(position);
            }
        }

        public void IncreaseGuardReaction()
        {
            Data.currentLevel.reactions++;
        }

        public void IncreaseAlert()
        {
            if (Data.alert < Data.currentLevel.maxAlert)
            {
                Data.alert++;

                var position = entityPool.FindGuardSpawn(this.playerController.transform.position);

                var c = Data.currentLevel.alertSpawn;
                c = c == LevelAlertSpawn.Any ? (LevelAlertSpawn)Random.Range(0, 2) : c;

                switch (c)
                {
                    case LevelAlertSpawn.Drone:
                        entityPool.SpawnDrone(position);
                        break;
                    case LevelAlertSpawn.Guard:
                    default:
                        entityPool.SpawnGuard(position);
                        break;
                }
            }
        }

        public void TakeDamage(int amount)
        {
            Data.health -= amount;
            if (!invulnerability && Data.health <= 0)
            {
                SetDead();
            }
        }

        private void CheckBindings()
        {
            this.playerController = this.playerController ?? FindObjectOfType<PlayerController>();
            this.levelRenderer = this.levelRenderer ?? FindObjectOfType<LevelRenderer>();
            this.entityPool = this.entityPool ?? FindObjectOfType<EntityPool>();
        }

        private void Initialization()
        {
            HideAllUI();
            LoadSlots();
            preferences = new PlayerPreferences(this.mixer);
            preferences.Load();
            ParticlesSelect.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            ParticlesLeft.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            ParticlesRight.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            entityPool.DestroyEntities();
            gameCanvas.SetActive(true);
            menuCanvas.SetActive(true);
            SetMainMenu();
        }

        private void HideAllUI()
        {
            this.hudPanel.Hide();
            this.startMenuPanel.Hide();
            this.deadPanel.Hide();
            this.dialogPanel.Hide();
            this.missionPanel.Hide();
            this.missionCompletePanel.Hide();
            this.levelPanel.Hide();
            this.gameOverPanel.Hide();
        }
    }
}