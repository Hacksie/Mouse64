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
        public bool Battle => true;

        public Dialog CurrentDialog { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

        public PlayingState(PlayerController player, EntityPool entityPool, LevelRenderer levelRenderer, UI.AbstractPresenter hudPresenter)
        {
            this.player = player;
            this.pool = entityPool;
            this.levelRenderer = levelRenderer;
            this.hudPresenter = hudPresenter;
            
        }

        public void Begin()
        {
            GameManager.Instance.ParticlesLeft.Play();
            Vector3 rightPos = this.levelRenderer.CalcPosition(GameManager.Instance.Data.currentLevel.settings.length + 1,0);
            GameManager.Instance.ParticlesRight.transform.position = rightPos + new Vector3(2.0f, 4, 0);
            GameManager.Instance.ParticlesRight.Play();
            //Cursor.visible = false;
            
            this.hudPresenter.Show(); 
        }

        public void End()
        {
            GameManager.Instance.ParticlesLeft.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            GameManager.Instance.ParticlesRight.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            this.hudPresenter.Hide();
            //Cursor.visible = true;
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

        public void Start()
        {
            GameManager.Instance.SetStartMenu();
        }

        public void ShowDialog()
        {
            throw new System.NotImplementedException();
        }

        public void EndDialog()
        {
            throw new System.NotImplementedException();
        }
    }
}