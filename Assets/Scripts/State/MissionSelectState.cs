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

        public bool PlayerActionAllowed => false;

        public MissionSelectState(PlayerController player, EntityPool entityPool, LevelRenderer levelRenderer, UI.DialogPresenter dialogPresenter, UI.MissionPresenter missionPresenter)
        {
            this.player = player;
            this.pool = entityPool;
            this.levelRenderer = levelRenderer;
            this.dialogPresenter = dialogPresenter;
            this.missionPresenter = missionPresenter;
        }

        public void Begin()
        {
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
            }
            else 
            {
                this.dialogPresenter.Show();
                this.missionPresenter.Hide();
                this.dialogPresenter.Repaint();
            }
            
            this.player.UpdateBehavior();
            // foreach (var e in this.pool.Pool)
            // {
            //     e.UpdateBehaviour();
            // }
            //GameManager.Instance.Data.timer -= Time.deltaTime;
        }

        public void FixedUpdate()
        {
            //this.player.FixedUpdateBehaviour();
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
            GameManager.Instance.SetStartMenu();
        }
    }
}