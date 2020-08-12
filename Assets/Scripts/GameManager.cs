
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
        [SerializeField] private bool invulnerability = true;
        [SerializeField] private Light2D globalLight = null;


        [Header("Data")]
        [SerializeField] public float easyAdj = 1.0f;
        [SerializeField] public float mediumAdj = 0.8f;
        [SerializeField] public float hardAdj = 0.6f;
        [SerializeField] public int currentSlot = 0;
        [SerializeField] public List<GameData> gameSlots = new List<GameData>(3);
        [SerializeField] private Level[] levels = null;

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


        public static GameManager Instance { get; private set; }

        public GameData Data { get { return this.gameSlots[this.currentSlot]; } private set { this.gameSlots[this.currentSlot] = value; } }
        public PlayerController Player { get { return playerController; } private set { playerController = value; } }
        public EntityPool EntityPool { get { return entityPool; } private set { entityPool = value; } }
        public LevelRenderer LevelRenderer { get { return levelRenderer; } private set { levelRenderer = value; } }

        private IState currentState;

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

        private void Awake()
        {
            LoadSlots();
            CheckBindings();
            Initialization();
            gameCanvas.SetActive(true);
            menuCanvas.SetActive(true);
            SetMainMenu();
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
            Data.saveName = System.DateTime.Now.ToString("yyyyddMM HHmm");
            Logger.Log(this, "Saving state", Data.saveName);
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
                currentLevel = levels[0]
            };

            this.playerController.Reset();
        }

        public void Reset()
        {
            Data.bullets = 6;
            Data.health = 0;
            Data.energy = 100;
            Data.alert = 0;
            Data.timer = 64;
            this.playerController.Reset();
        }

        public void NextLevel()
        {
            Data.currentLevel = levels[++Data.currentLevelIndex];
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

        public bool ConsumeBullet()
        {
            if (Data.bullets > 0)
            {
                Data.bullets--;
                return true;
            }

            return false;
        }

        public bool ConsumeStealth(float amount)
        {
            if (Data.energy > 0)
            {
                Data.energy = Mathf.Max(0, Data.energy - amount);
                return true;
            }

            return false;
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
        }
    }
}