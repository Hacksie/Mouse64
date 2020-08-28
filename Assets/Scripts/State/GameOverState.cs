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
        public bool Battle => false;

        public Dialog CurrentDialog { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

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
            GameManager.Instance.EntityPool.DestroyEntities();
            this.player.Sit = true;
            this.player.transform.position = new Vector3(2, 0.275f, 0);
            this.levelRenderer.LoadMissionSelectLevel();
            AudioManager.Instance.PlayMissionSelectMusic();
            GameManager.Instance.ParticlesSelect.Play();
            ShowDialog();
        }

        public void End()
        {
            GameManager.Instance.ParticlesSelect.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            this.player.Sit = false;
            HideDialog();
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

        public void ShowDialog()
        {
            this.gameOverPresenter.Show();
            this.gameOverPresenter.Repaint();
        }

        public void HideDialog()
        {
            this.gameOverPresenter.Hide();
        }
    }
}