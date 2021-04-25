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
        public bool Battle => false;
        public Dialog CurrentDialog { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

        public void Begin()
        {
            Cursor.visible = false;
            this.menuPresenter.Show();
            AudioManager.Instance.PlayMenuMusic();
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

        public void Start()
        {

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