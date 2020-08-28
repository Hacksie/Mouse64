using UnityEngine;

namespace HackedDesign
{
    public class StartMenuState : IState
    {
        private UI.AbstractPresenter hudPresenter;
        private UI.AbstractPresenter startMenuPresenter;

        public bool PlayerActionAllowed => false;
        public bool Battle => false;

        public Dialog CurrentDialog { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

        public StartMenuState(UI.AbstractPresenter hudPresenter, UI.AbstractPresenter startMenuPresenter)
        {
            this.hudPresenter = hudPresenter;
            this.startMenuPresenter = startMenuPresenter;
        }

        public void Begin()
        {
            this.hudPresenter.Show();
            
        }

        public void End()
        {
            this.hudPresenter.Hide();
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
        }

        public void Start()
        {
        }

        public void ShowDialog()
        {
            this.startMenuPresenter.Show();
            this.startMenuPresenter.Repaint();
        }

        public void HideDialog()
        {
            this.startMenuPresenter.Hide();
        }
    }
}