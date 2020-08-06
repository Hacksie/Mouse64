using UnityEngine;

namespace HackedDesign
{
    public class MissionSelectState : IState
    {
        private PlayerController player;
        private EntityPool pool;
        private LevelRenderer levelRenderer;

        public bool PlayerActionAllowed => false;

        public MissionSelectState(PlayerController player, EntityPool entityPool, LevelRenderer levelRenderer)
        {
            this.player = player;
            this.pool = entityPool;
            this.levelRenderer = levelRenderer;
        }

        public void Begin()
        {
            this.pool.DestroyEntities();
            GameManager.Instance.Data.currentLevel.name = "hotdog";
            this.levelRenderer.LoadLevel(GameManager.Instance.Data.currentLevel);
        }

        public void End()
        {

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
            this.player.FixedUpdateBehaviour();
        }

        public void LateUpdate()
        {
            this.player.LateUpdateBehaviour();
            foreach(var e in this.pool.Pool)
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