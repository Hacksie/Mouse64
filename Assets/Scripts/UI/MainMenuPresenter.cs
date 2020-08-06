using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HackedDesign.UI
{

    public class MainMenuPresenter : AbstractPresenter
    {
        [SerializeField] private GameObject defaultPanel = null;
        [SerializeField] private GameObject creditsPanel = null;
        private MainMenuState state = MainMenuState.Default;

        void Awake()
        {
            if(defaultPanel == null) Debug.LogWarning("Default panel not set");
        }

        public override void Repaint()
        {
            switch (state)
            {
                case MainMenuState.Credits:
                    defaultPanel.SetActive(false);
                    creditsPanel.SetActive(true);
                    break;
                case MainMenuState.Default:
                default:
                    defaultPanel.SetActive(true);
                    creditsPanel.SetActive(false);
                    break;

            }
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
            GameManager.Instance.SetMissionSelect();
            //GameManager.Instance.SetPlaying();
        }

        public void QuitEvent()
        {
            GameManager.Instance.SetQuit();
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