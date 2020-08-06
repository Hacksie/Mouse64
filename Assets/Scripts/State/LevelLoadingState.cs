using UnityEngine;

namespace HackedDesign
{
    public class LevelLoadingState : IState
    {


        public bool PlayerActionAllowed => false;
        private LevelRenderer levelRenderer;
        private EntityPool entityPool;

        public LevelLoadingState(LevelRenderer levelRenderer, EntityPool entityPool)
        {
            this.levelRenderer = levelRenderer;
            this.entityPool = entityPool;

        }

        public void Begin()
        {
            this.entityPool.DestroyEntities();
            this.levelRenderer.LoadLevel(GameManager.Instance.Data.currentLevel);
            
        }

        public void End()
        {
            
        }

        public void Update()
        {
            GameManager.Instance.SetPlaying();
        }

        public void FixedUpdate()
        {
        }

        public void LateUpdate()
        {

        }

        public void Select()
        {

        }

        public void Start()
        {

        }

        public void Interact()
        {

        }
    }
}