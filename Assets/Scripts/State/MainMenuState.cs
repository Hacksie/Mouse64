using UnityEngine;

namespace HackedDesign
{
    public class MainMenuState : IState
    {
        private UI.AbstractPresenter menuPresenter;

        public MainMenuState(UI.AbstractPresenter mainMenuPresenter)
        {
            this.menuPresenter = mainMenuPresenter;
        }

        public bool PlayerActionAllowed => false;

        public void Begin()
        {
            this.menuPresenter.Show();
        }

        public void End()
        {
            this.menuPresenter.Hide();
        }

        public void Update()
        {

        }

        public void FixedUpdate()
        {

        }        

        public void LateUpdate()
        {
            menuPresenter.Repaint();
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