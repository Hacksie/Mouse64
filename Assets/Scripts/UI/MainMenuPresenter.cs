using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.EventSystems;
using System.Linq;

namespace HackedDesign.UI
{

    public class MainMenuPresenter : AbstractPresenter
    {
        [SerializeField] private GameObject defaultPanel = null;
        [SerializeField] private GameObject playPanel = null;
        [SerializeField] private GameObject optionsPanel = null;
        [SerializeField] private GameObject screenPanel = null;
        [SerializeField] private GameObject creditsPanel = null;
        [SerializeField] private AudioMixer masterMixer = null;
        [SerializeField] private UnityEngine.UI.Slider masterSlider = null;
        [SerializeField] private UnityEngine.UI.Slider fxSlider = null;
        [SerializeField] private UnityEngine.UI.Slider musicSlider = null;
        [SerializeField] private GameObject[] slotButtons = null;
        [SerializeField] private UnityEngine.UI.Text[] slotTexts = null;
        [SerializeField] private GameObject quitButton = null;
        [SerializeField] private GameObject screenButton = null;
        [SerializeField] private UnityEngine.UI.Dropdown resolutionsDropdown = null;
        [SerializeField] private GameObject defaultButton = null;

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
            if (Application.platform == RuntimePlatform.WebGLPlayer) // || Application.platform == RuntimePlatform.WindowsEditor)
            {
                quitButton?.SetActive(false);
                screenButton?.SetActive(false);
            }
            PopulateOptionsValues();
            EventSystem.current.SetSelectedGameObject(defaultButton);
        }

        private void PopulateOptionsValues()
        {
            resolutionsDropdown.ClearOptions();
            resolutionsDropdown.AddOptions(Screen.resolutions.ToList().ConvertAll(r => new UnityEngine.UI.Dropdown.OptionData(r.width + "x" + r.height)));
        }

        public override void Repaint()
        {
            for (int i = 0; i < slotTexts.Length; i++)
            {
                slotTexts[i].text = (GameManager.Instance.gameSlots[i] == null || GameManager.Instance.gameSlots[i].newGame) ? "empty" : GameManager.Instance.gameSlots[i].saveName;
            }

            switch (state)
            {
                case MainMenuState.Play:
                    defaultPanel.SetActive(false);
                    playPanel.SetActive(true);
                    optionsPanel.SetActive(false);
                    screenPanel.SetActive(false);
                    creditsPanel.SetActive(false);

                    EventSystem.current.SetSelectedGameObject(slotButtons[GameManager.Instance.currentSlot]);

                    break;
                case MainMenuState.Options:
                    defaultPanel.SetActive(false);
                    playPanel.SetActive(false);
                    optionsPanel.SetActive(true);
                    screenPanel.SetActive(false);
                    creditsPanel.SetActive(false);
                    break;
                case MainMenuState.Screen:
                    defaultPanel.SetActive(false);
                    playPanel.SetActive(false);
                    optionsPanel.SetActive(false);
                    screenPanel.SetActive(true);
                    creditsPanel.SetActive(false);
                    break;
                case MainMenuState.Credits:
                    defaultPanel.SetActive(false);
                    playPanel.SetActive(false);
                    optionsPanel.SetActive(false);
                    screenPanel.SetActive(false);
                    creditsPanel.SetActive(true);
                    break;
                case MainMenuState.Default:
                default:
                    defaultPanel.SetActive(true);
                    playPanel.SetActive(false);
                    optionsPanel.SetActive(false);
                    screenPanel.SetActive(false);
                    creditsPanel.SetActive(false);
                    break;
            }
        }

        public void Slot0Event()
        {
            GameManager.Instance.currentSlot = 0;
        }

        public void Slot1Event()
        {
            GameManager.Instance.currentSlot = 1;
        }

        public void Slot2Event()
        {
            GameManager.Instance.currentSlot = 2;
        }

        public void OptionsEvent()
        {
            state = MainMenuState.Options;
        }

        public void CreditsEvent()
        {
            state = MainMenuState.Credits;
        }

        public void ScreenEvent()
        {
            state = MainMenuState.Screen;
        }

        public void ReturnEvent()
        {
            state = MainMenuState.Default;
        }

        public void ReturnOptionsEvent()
        {
            state = MainMenuState.Options;
        }

        public void PlayEvent()
        {
            state = MainMenuState.Play;
        }

        public void DeleteEvent()
        {
            GameManager.Instance.gameSlots[GameManager.Instance.currentSlot] = null;
        }

        public void StartEvent()
        {
            if (GameManager.Instance.gameSlots[GameManager.Instance.currentSlot] == null || GameManager.Instance.gameSlots[GameManager.Instance.currentSlot].newGame)
            {
                Logger.Log(this, "New game");
                GameManager.Instance.NewGame();
            }

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
        Screen,
        Credits,
        Quit
    }
}