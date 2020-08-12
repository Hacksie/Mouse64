using UnityEngine;

namespace HackedDesign
{
    public class MissionSelectState : IState
    {
        private PlayerController player;
        private EntityPool pool;
        private LevelRenderer levelRenderer;
        private UI.DialogPresenter dialogPresenter;
        private UI.MissionPresenter missionPresenter;
        private UI.LevelPresenter levelPresenter;

        public bool PlayerActionAllowed => false;

        public MissionSelectState(PlayerController player, EntityPool entityPool, LevelRenderer levelRenderer, UI.DialogPresenter dialogPresenter, UI.MissionPresenter missionPresenter, UI.LevelPresenter levelPresenter)
        {
            this.player = player;
            this.pool = entityPool;
            this.levelRenderer = levelRenderer;
            this.dialogPresenter = dialogPresenter;
            this.missionPresenter = missionPresenter;
            this.levelPresenter = levelPresenter;
        }

        public void Begin()
        {
            GameManager.Instance.SaveGame();
            GameManager.Instance.Reset();
            //GameManager.Instance.Data.currentLevel = levels[Data.currentLevelIndex];
            this.player.Sit = true;
            this.player.transform.position = new Vector3(2, 0.275f, 0);
            this.pool.DestroyEntities();
            //GameManager.Instance.Data.currentLevel.name = "hotdog";
            this.levelRenderer.LoadMissionSelectLevel();
            this.dialogPresenter.Show();
        }

        public void End()
        {
            this.player.Sit = false;
            this.dialogPresenter.Hide();
            this.missionPresenter.Hide();
        }

        public void Update()
        {
            if(GameManager.Instance.Data.currentLevel.currentDialogue >= GameManager.Instance.Data.currentLevel.dialogue.Count)
            {
                this.dialogPresenter.Hide();
                this.missionPresenter.Show();
                this.missionPresenter.Repaint();
                this.levelPresenter.Hide();
            }
            else 
            {
                this.dialogPresenter.Show();
                this.levelPresenter.Show();
                this.missionPresenter.Hide();
                this.dialogPresenter.Repaint();
                this.levelPresenter.Repaint();
            }
            
            this.player.UpdateBehavior();
        }

        public void FixedUpdate()
        {
            
        }

        public void LateUpdate()
        {
            this.player.LateUpdateBehaviour();
            foreach (var e in this.pool.Pool)
            {
                e.UpdateLateBehaviour();
            }
        }

        public void Interact()
        {

        }

        public void Select()
        {

        }

        public void Start()
        {
            //GameManager.Instance.SetStartMenu();
        }
    }
}