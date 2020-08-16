using UnityEngine;

namespace HackedDesign
{
    public class GameOverState : IState
    {
        private PlayerController player;
        private EntityPool pool;
        private LevelRenderer levelRenderer;
        private UI.AbstractPresenter gameOverPresenter;
        
        public bool PlayerActionAllowed => false;

        public GameOverState(PlayerController player, EntityPool entityPool, LevelRenderer levelRenderer, UI.GameOverPresenter gameOverPresenter)
        {
            this.player = player;
            this.pool = entityPool;
            this.levelRenderer = levelRenderer;
            this.gameOverPresenter = gameOverPresenter;
        }

        public void Begin()
        {
            GameManager.Instance.SaveGame();
            GameManager.Instance.Reset();
            this.player.Sit = true;
            this.player.transform.position = new Vector3(2, 0.275f, 0);
            this.levelRenderer.LoadMissionSelectLevel();
            this.gameOverPresenter.Show();
            this.gameOverPresenter.Repaint();
            AudioManager.Instance.PlayMissionSelectMusic();
            GameManager.Instance.ParticlesSelect.Play();
        }

        public void End()
        {
            GameManager.Instance.ParticlesSelect.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            this.player.Sit = false;
            this.gameOverPresenter.Hide();
            
        }

        public void Update()
        {
            this.player.UpdateBehavior();
        }

        public void FixedUpdate()
        {
            
        }

        public void LateUpdate()
        {
            this.player.LateUpdateBehaviour();
        }

        public void Start()
        {
            
        }
    }
}