using UnityEngine;

namespace HackedDesign
{
    public class RoomState : IState
    {
        private PlayerController player;
        private EntityPool pool;
        private LevelRenderer levelRenderer;

        public bool PlayerActionAllowed => true;
        public bool Battle => false;

        public RoomState(PlayerController player, EntityPool entityPool, LevelRenderer levelRenderer)
        {
            this.player = player;
            this.pool = entityPool;
            this.levelRenderer = levelRenderer;
        }

        public void Begin()
        {
            GameManager.Instance.SaveGame();
            GameManager.Instance.Reset();
            //this.player.Sit = true;
            this.player.transform.position = new Vector3(1.75f, 0.275f, 0);
            this.pool.DestroyEntities();
            this.levelRenderer.LoadPreludeLevel();
            AudioManager.Instance.PlayMissionSelectMusic();
            GameManager.Instance.ParticlesLeft.Play();
            Vector3 rightPos = this.levelRenderer.CalcPosition(GameManager.Instance.Data.currentLevel.length + 1);
            GameManager.Instance.ParticlesRight.transform.position = rightPos + new Vector3(2.0f, 4, 0);
            GameManager.Instance.ParticlesRight.Play();
        }

        public void End()
        {
            GameManager.Instance.ParticlesSelect.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            //this.player.Sit = false;
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
            foreach (var e in this.pool.Pool)
            {
                e.UpdateLateBehaviour();
            }
        }

        public void Start()
        {
            
        }
    }
}