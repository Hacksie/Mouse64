using UnityEngine;

namespace HackedDesign
{
    public class StartMenuState : IState
    {
        private UI.AbstractPresenter hudPresenter;
        private UI.AbstractPresenter startMenuPresenter;

        public bool PlayerActionAllowed => false;
        public bool Battle => false;

        public StartMenuState(UI.AbstractPresenter hudPresenter, UI.AbstractPresenter startMenuPresenter)
        {
            this.hudPresenter = hudPresenter;
            this.startMenuPresenter = startMenuPresenter;
        }

        public void Begin()
        {
            this.hudPresenter.Show();
            this.startMenuPresenter.Show();
            this.startMenuPresenter.Repaint();
        }

        public void End()
        {
            this.hudPresenter.Hide();
            this.startMenuPresenter.Hide();
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
    }
}