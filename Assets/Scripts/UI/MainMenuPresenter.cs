using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HackedDesign.UI
{

    public class MainMenuPresenter : AbstractPresenter
    {
        [SerializeField] private GameObject defaultPanel = null;
        [SerializeField] private GameObject playPanel = null;
        [SerializeField] private GameObject optionsPanel = null;
        [SerializeField] private GameObject creditsPanel = null;
        [SerializeField] private string URL = "https://hackeddesign.itch.io/";
        private MainMenuState state = MainMenuState.Default;

        void Awake()
        {
            if (defaultPanel == null) Debug.LogWarning("Default panel not set");
        }

        public override void Repaint()
        {
            switch (state)
            {
                case MainMenuState.Play:
                    defaultPanel.SetActive(false);
                    playPanel.SetActive(true);
                    optionsPanel.SetActive(false);
                    creditsPanel.SetActive(false);
                    break;
                case MainMenuState.Options:
                    defaultPanel.SetActive(false);
                    playPanel.SetActive(false);
                    optionsPanel.SetActive(true);
                    creditsPanel.SetActive(false);
                    break;
                case MainMenuState.Credits:
                    defaultPanel.SetActive(false);
                    playPanel.SetActive(false);
                    optionsPanel.SetActive(false);
                    creditsPanel.SetActive(true);
                    break;
                case MainMenuState.Default:
                default:
                    defaultPanel.SetActive(true);
                    playPanel.SetActive(false);
                    optionsPanel.SetActive(false);
                    creditsPanel.SetActive(false);
                    break;

            }
        }


        public void OptionsEvent()
        {
            state = MainMenuState.Options;
        }

        public void CreditsEvent()
        {
            state = MainMenuState.Credits;
        }

        public void ReturnEvent()
        {
            state = MainMenuState.Default;
        }

        public void PlayEvent()
        {
            state = MainMenuState.Play;
        }

        public void StartEvent()
        {
            GameManager.Instance.SetMissionSelect();
        }

        public void QuitEvent()
        {
            GameManager.Instance.SetQuit();
        }

        public void LogoEvent()
        {
            Application.OpenURL(this.URL);
        }
    }

    public enum MainMenuState
    {
        Default,
        Play,
        Options,
        Credits,
        Quit
    }
}