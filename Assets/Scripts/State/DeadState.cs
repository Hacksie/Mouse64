using UnityEngine;

namespace HackedDesign
{
    public class DeadState : IState
    {
        private PlayerController player;
        private UI.AbstractPresenter deadPresenter;

        public bool PlayerActionAllowed => false;

        public DeadState(PlayerController player, UI.AbstractPresenter deadPresenter)
        {
            this.player = player;
            this.deadPresenter = deadPresenter;
        }

        public void Begin()
        {
            this.deadPresenter.Show();
        }

        public void End()
        {
            this.deadPresenter.Hide();
        }

        public void Update()
        {
        }

        public void FixedUpdate()
        {
            
        }

        public void LateUpdate()
        {
            this.player.LateUpdateBehaviour();
            
        }

        public void Interact()
        {
            
        }

        public void Select()
        {
            
        }

        public void Start()
        {
            
        }
    }
}