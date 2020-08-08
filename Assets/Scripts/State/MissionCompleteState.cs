using UnityEngine;

namespace HackedDesign
{
    public class MissionCompleteState : IState
    {
        private PlayerController player;
        private UI.AbstractPresenter missionCompletePresenter;

        public bool PlayerActionAllowed => false;

        public MissionCompleteState(PlayerController player, UI.AbstractPresenter missionCompletePresenter)
        {
            this.player = player;
            this.missionCompletePresenter = missionCompletePresenter;
        }

        public void Begin()
        {
            GameManager.Instance.Reset();
            this.missionCompletePresenter.Show();
        }

        public void End()
        {
            this.missionCompletePresenter.Hide();
        }

        public void Update()
        {
            this.missionCompletePresenter.Repaint();
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