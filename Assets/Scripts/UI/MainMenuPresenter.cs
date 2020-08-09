using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace HackedDesign.UI
{

    public class MainMenuPresenter : AbstractPresenter
    {
        [SerializeField] private GameObject defaultPanel = null;
        [SerializeField] private GameObject playPanel = null;
        [SerializeField] private GameObject optionsPanel = null;
        [SerializeField] private GameObject creditsPanel = null;
        [SerializeField] private AudioMixer masterMixer = null;
        [SerializeField] private UnityEngine.UI.Slider masterSlider = null;
        [SerializeField] private UnityEngine.UI.Slider fxSlider = null;
        [SerializeField] private UnityEngine.UI.Slider musicSlider = null;
        [SerializeField] private string URL = "https://hackeddesign.itch.io/";
        private MainMenuState state = MainMenuState.Default;

        void Awake()
        {
            if (defaultPanel == null) Debug.LogWarning("Default panel not set");
            float master, fx, music;
            masterMixer.GetFloat("MasterVolume", out master);
            masterMixer.GetFloat("FXVolume", out fx);
            masterMixer.GetFloat("MusicVolume", out music);
            masterSlider.value = master - 100;
            fxSlider.value = fx - 100;
            musicSlider.value = music - 100;
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

        public void MasterChanged()
        {
            masterMixer.SetFloat("MasterVolume", masterSlider.value);
        }
        public void FXChanged()
        {
            masterMixer.SetFloat("FXVolume", fxSlider.value);
        }
        public void MusicChanged()
        {
            masterMixer.SetFloat("MusicVolume", musicSlider.value);
        }
        public void SensitivityChanged()
        {

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