using UnityEngine;

namespace HackedDesign
{
    public class DeadState : IState
    {
        private PlayerController player;
        private UI.AbstractPresenter deadPresenter;

        public bool PlayerActionAllowed => false;
        public bool Battle => false;

        public Dialog CurrentDialog { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

        public DeadState(PlayerController player, UI.AbstractPresenter deadPresenter)
        {
            this.player = player;
            this.deadPresenter = deadPresenter;
        }

        public void Begin()
        {
            this.player.Dead = true;
            AudioManager.Instance.PlayDeathMusic();
            ShowDialog();
        }

        public void End()
        {
            HideDialog();
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

        public void Start()
        {
            
        }

        public void ShowDialog()
        {
            this.deadPresenter.Show();
            this.deadPresenter.Repaint();
        }

        public void HideDialog()
        {
            this.deadPresenter.Hide();
        }
    }
}