using UnityEngine;

namespace HackedDesign
{
    public class PlayingState : IState
    {
        private PlayerController player;
        private EntityPool pool;
        private UI.AbstractPresenter hudPresenter;
        private LevelRenderer levelRenderer;

        public bool PlayerActionAllowed => true;

        public PlayingState(PlayerController player, EntityPool entityPool, LevelRenderer levelRenderer, UI.AbstractPresenter hudPresenter)
        {
            this.player = player;
            this.pool = entityPool;
            this.levelRenderer = levelRenderer;
            this.hudPresenter = hudPresenter;
        }

        public void Begin()
        {
            this.pool.DestroyEntities();
            this.levelRenderer.LoadRandomLevel(GameManager.Instance.Data.currentLevel);
            GameManager.Instance.Data.timer = GameManager.Instance.Data.currentLevel.window;
            this.hudPresenter.Show();
        }

        public void End()
        {
            this.hudPresenter.Hide();
        }

        public void Update()
        {
            this.player.UpdateBehavior();
            foreach(var e in this.pool.Pool)
            {
                e.UpdateBehaviour();
            }            
            GameManager.Instance.Data.timer -= Time.deltaTime;
        }

        public void FixedUpdate()
        {
            //this.player.FixedUpdateBehaviour();
        }

        public void LateUpdate()
        {
            this.player.LateUpdateBehaviour();
            foreach(var e in this.pool.Pool)
            {
                e.UpdateLateBehaviour();
            }

            this.hudPresenter.Repaint();
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