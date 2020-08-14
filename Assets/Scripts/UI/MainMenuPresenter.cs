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
        [SerializeField] private UnityEngine.UI.Slider lookSlider = null;
        [SerializeField] private GameObject[] slotButtons = null;
        [SerializeField] private UnityEngine.UI.Text[] slotTexts = null;
        [SerializeField] private GameObject quitButton = null;
        [SerializeField] private GameObject screenButton = null;
        [SerializeField] private UnityEngine.UI.Dropdown resolutionsDropdown = null;
        [SerializeField] private UnityEngine.UI.Dropdown fullScreenDropdown = null;
        //[SerializeField] private UnityEngine.UI.Toggle fullScreen = null;
        [SerializeField] private GameObject defaultButton = null;
        [SerializeField] private UnityEngine.UI.Text masterVolumeText = null;
        [SerializeField] private UnityEngine.UI.Text fxVolumeText = null;
        [SerializeField] private UnityEngine.UI.Text musicVolumeText = null;
        [SerializeField] private UnityEngine.UI.Text lookText = null;


        [SerializeField] private string URL = "https://hackeddesign.itch.io/";
        private MainMenuState state = MainMenuState.Default;

        void Awake()
        {
            if (defaultPanel == null) Debug.LogWarning("Default panel not set");
            if (Application.platform == RuntimePlatform.WebGLPlayer) // || Application.platform == RuntimePlatform.WindowsEditor)
            {
                quitButton?.SetActive(false);
                screenButton?.SetActive(false);
            }
            PopulateValues();
            EventSystem.current.SetSelectedGameObject(defaultButton);
        }

        private void PopulateValues()
        {
            resolutionsDropdown.ClearOptions();
            resolutionsDropdown.AddOptions(Screen.resolutions.ToList().ConvertAll(r => new UnityEngine.UI.Dropdown.OptionData(r.width + "x" + r.height)));

            resolutionsDropdown.value = Screen.resolutions.ToList().IndexOf(Screen.currentResolution);
            
            fullScreenDropdown.ClearOptions();
            fullScreenDropdown.AddOptions(new List<UnityEngine.UI.Dropdown.OptionData>() { new UnityEngine.UI.Dropdown.OptionData("Exclusive"), new UnityEngine.UI.Dropdown.OptionData("Full(Win)"), new UnityEngine.UI.Dropdown.OptionData("Maximised"), new UnityEngine.UI.Dropdown.OptionData("Window") });
            fullScreenDropdown.value = (int)Screen.fullScreenMode;
            // float master, fx, music;
            // masterMixer.GetFloat("MasterVolume", out master);
            // masterMixer.GetFloat("FXVolume", out fx);
            // masterMixer.GetFloat("MusicVolume", out music);
            //fullScreen.isOn = (bool)GameManager.Instance.PlayerPreferences.fullScreen;
            masterSlider.value = GameManager.Instance.PlayerPreferences.masterVolume;
            fxSlider.value = GameManager.Instance.PlayerPreferences.sfxVolume;
            musicSlider.value = GameManager.Instance.PlayerPreferences.musicVolume;
            lookSlider.value = GameManager.Instance.PlayerPreferences.lookSpeed;
            
            RepaintMasterText();
            RepaintFXText();
            RepaintMusicText();
            RepaintLookText();
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
            GameManager.Instance.PlayerPreferences.Save();
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

        public void RandomEvent()
        {
            
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

        public void MasterChangedEvent()
        {
            masterMixer.SetFloat("MasterVolume", masterSlider.value);
            GameManager.Instance.PlayerPreferences.masterVolume = masterSlider.value;
            RepaintMasterText();
            GameManager.Instance.PlayerPreferences.Save();
        }

        public void FXChangedEvent()
        {
            masterMixer.SetFloat("FXVolume", fxSlider.value);
            GameManager.Instance.PlayerPreferences.sfxVolume = fxSlider.value;
            RepaintFXText();
            GameManager.Instance.PlayerPreferences.Save();
        }

        public void MusicChangedEvent()
        {
            masterMixer.SetFloat("MusicVolume", musicSlider.value);
            GameManager.Instance.PlayerPreferences.musicVolume = musicSlider.value;
            RepaintMusicText();
            GameManager.Instance.PlayerPreferences.Save();
        }

        public void SensitivityChangedEvent()
        {
            GameManager.Instance.PlayerPreferences.lookSpeed = lookSlider.value;
            RepaintLookText();
            GameManager.Instance.PlayerPreferences.Save();
        }

        public void ResolutionChangedEvent()
        {

        }

        public void FullScreenChangedEvent()
        {

        }


        public void Tiny64WindowEvent()
        {
            Screen.SetResolution(64, 64, FullScreenMode.Windowed, Screen.currentResolution.refreshRate);
        }

        public void Small640WindowEvent()
        {
            Screen.SetResolution(640, 640, FullScreenMode.Windowed, Screen.currentResolution.refreshRate);
        }

        private void RepaintMasterText()
        {
            masterVolumeText.text = GameManager.Instance.PlayerPreferences.masterVolume.ToString("F0") + "db";
        }

        private void RepaintFXText()
        {
            fxVolumeText.text = GameManager.Instance.PlayerPreferences.sfxVolume.ToString("F0") + "db";
        }

        private void RepaintMusicText()
        {
            musicVolumeText.text = GameManager.Instance.PlayerPreferences.musicVolume.ToString("F0") + "db";
        }

        private void RepaintLookText()
        {
            lookText.text = GameManager.Instance.PlayerPreferences.lookSpeed.ToString("F0") + "#/s";
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