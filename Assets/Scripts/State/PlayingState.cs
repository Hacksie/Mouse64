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
            GameManager.Instance.Reset();
            //this.player.Reset();
            this.pool.DestroyEntities();
            this.levelRenderer.LoadRandomLevel(GameManager.Instance.Data.currentLevel);
            GameManager.Instance.ParticlesLeft.Play();
            Vector3 rightPos = this.levelRenderer.CalcPosition(GameManager.Instance.Data.currentLevel.length + 1);
            GameManager.Instance.ParticlesRight.transform.position = rightPos + new Vector3(2.0f, 4, 0);
            GameManager.Instance.ParticlesRight.Play();
            GameManager.Instance.Data.timer = GameManager.Instance.Data.currentLevel.window;
            this.hudPresenter.Show();
            AudioManager.Instance.PlayRandomGameMusic();
        }

        public void End()
        {
            GameManager.Instance.ParticlesLeft.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            GameManager.Instance.ParticlesRight.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            this.hudPresenter.Hide();
        }

        public void Update()
        {
            this.player.UpdateBehavior();
            foreach (var e in this.pool.Pool)
            {
                e.UpdateBehaviour();
            }

            if (GameManager.Instance.Data.timer > 0)
            {
                GameManager.Instance.Data.timer -= Time.deltaTime;
            }
            else
            {
                GameManager.Instance.TimeUp();
                //Logger.LogWarning("PlayingState", "Time run out");
                //GameManager.Instance.EntityPool.S
            }


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