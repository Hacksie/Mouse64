
using System.IO;
using System.Linq;
using UnityEngine;


namespace HackedDesign
{
    public class GameManager : MonoBehaviour
    {
        [Header("Game")]
        [SerializeField] private PlayerController playerController = null;
        [SerializeField] private LevelRenderer levelRenderer = null;
        [SerializeField] private EntityPool entityPool = null;
        [SerializeField] private bool invulnerability = true;

        [Header("Data")]
        [SerializeField] private GameData gameData;
        [SerializeField] private Level[] levels;

        [Header("UI")]
        [SerializeField] private GameObject gameCanvas = null;
        [SerializeField] private GameObject menuCanvas = null;
        [SerializeField] private UI.MainMenuPresenter mainMenuPanel = null;
        [SerializeField] private UI.StartMenuPresenter startMenuPanel = null;
        [SerializeField] private UI.HudPresenter hudPanel = null;
        [SerializeField] private UI.DeadPresenter deadPanel = null;
        [SerializeField] private UI.DialogPresenter dialogPanel = null;
        [SerializeField] private UI.MissionPresenter missionPanel = null;

        public static GameManager Instance { get; private set; }

        public GameData Data { get { return gameData; } private set { gameData = value; } }

        public PlayerController Player { get { return playerController; } private set { playerController = value; } }

        public EntityPool EntityPool { get { return entityPool; } private set { entityPool = value; } }




        private IState currentState;

        public IState CurrentState
        {
            get
            {
                return currentState;
            }
            private set
            {
                if (currentState != null)
                {
                    currentState.End();
                }
                currentState = value;
                if (currentState != null)
                {
                    currentState.Begin();
                }
            }
        }


        private GameManager() => Instance = this;

        private void Awake()
        {
            Data.currentLevel = levels[0];
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
        public void SetMissionSelect() => CurrentState = new MissionSelectState(this.playerController, this.entityPool, this.levelRenderer, this.dialogPanel, this.missionPanel);
        //public void SetLoading() => CurrentState = new LevelLoadingState(this.levelRenderer, this.entityPool);
        public void SetPlaying() => CurrentState = new PlayingState(this.playerController, this.entityPool, this.levelRenderer, this.hudPanel);
        public void SetStartMenu() => CurrentState = new StartMenuState(this.hudPanel, this.startMenuPanel);
        public void SetDead() => CurrentState = new DeadState(this.playerController, this.deadPanel);
        public void SetQuit() => Application.Quit();

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
            if (Data.alert < Data.maxAlert)
            {
                Logger.Log(this, "Alert increased!");
                Data.alert++;
            }
        }

        public void TakeDamage(float amount)
        {
            Data.health -= amount;
            if(!invulnerability && Data.health <= 0)
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
        }
    }
}