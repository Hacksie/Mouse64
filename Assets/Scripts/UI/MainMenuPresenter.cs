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
        [Header("Main")]
        [SerializeField] private GameObject defaultPanel = null;
        [SerializeField] private GameObject playPanel = null;
        [SerializeField] private GameObject randomPanel = null;
        [SerializeField] private GameObject optionsPanel = null;
        [SerializeField] private GameObject screenPanel = null;
        [SerializeField] private GameObject creditsPanel = null;
        [SerializeField] private AudioMixer masterMixer = null;
        [SerializeField] private GameObject quitButton = null;
        [SerializeField] private GameObject defaultButton = null;
        [Header("Play")]
        [SerializeField] private UnityEngine.UI.Button[] slotButtons = null;
        [SerializeField] private UnityEngine.UI.Text[] slotTexts = null;
        [SerializeField] private Color selectedColor = Color.red;
        [SerializeField] private Color unselectedColor = Color.black;
        [Header("Random")]
        [SerializeField] private UnityEngine.UI.InputField seedInput = null;
        [SerializeField] private UnityEngine.UI.Dropdown difficultyDropdown = null;
        [SerializeField] private string[] difficulties = { "Easy", "Medium", "Hard" };
        [Header("Options")]
        [SerializeField] private UnityEngine.UI.Slider masterSlider = null;
        [SerializeField] private UnityEngine.UI.Slider fxSlider = null;
        [SerializeField] private UnityEngine.UI.Slider musicSlider = null;
        [SerializeField] private UnityEngine.UI.Slider lookSlider = null;
        [SerializeField] private UnityEngine.UI.Dropdown resolutionsDropdown = null;
        [SerializeField] private UnityEngine.UI.Dropdown fullScreenDropdown = null;
        [SerializeField] private GameObject screenButton = null;
        [SerializeField] private UnityEngine.UI.Text masterVolumeText = null;
        [SerializeField] private UnityEngine.UI.Text fxVolumeText = null;
        [SerializeField] private UnityEngine.UI.Text musicVolumeText = null;
        [SerializeField] private UnityEngine.UI.Text lookText = null;


        private MainMenuState state = MainMenuState.Default;

        void Awake()
        {
            if (defaultPanel == null) Debug.LogWarning("Default panel not set");
            if (Application.platform == RuntimePlatform.WebGLPlayer) // || Application.platform == RuntimePlatform.WindowsEditor)
            {
                quitButton?.SetActive(false);
                screenButton?.SetActive(false);
            }
        }

        void Start()
        {
            PopulateValues();
        }

        private void PopulateValues()
        {
            resolutionsDropdown.ClearOptions();
            resolutionsDropdown.AddOptions(Screen.resolutions.ToList().ConvertAll(r => new UnityEngine.UI.Dropdown.OptionData(r.width + "x" + r.height)));
            resolutionsDropdown.SetValueWithoutNotify(Screen.resolutions.ToList().IndexOf(Screen.currentResolution));
            fullScreenDropdown.ClearOptions();
            fullScreenDropdown.AddOptions(new List<UnityEngine.UI.Dropdown.OptionData>() { new UnityEngine.UI.Dropdown.OptionData("Exclusive"), new UnityEngine.UI.Dropdown.OptionData("Full(Win)"), new UnityEngine.UI.Dropdown.OptionData("Maximised"), new UnityEngine.UI.Dropdown.OptionData("Window") });
            fullScreenDropdown.SetValueWithoutNotify((int)Screen.fullScreenMode);
            masterSlider.value = GameManager.Instance.PlayerPreferences.masterVolume;
            fxSlider.value = GameManager.Instance.PlayerPreferences.sfxVolume;
            musicSlider.value = GameManager.Instance.PlayerPreferences.musicVolume;
            lookSlider.value = GameManager.Instance.PlayerPreferences.lookSpeed;

            seedInput.text = ((int)System.DateTime.Now.Ticks).ToString();
            difficultyDropdown.ClearOptions();
            difficultyDropdown.AddOptions(difficulties.ToList().ConvertAll(i => new UnityEngine.UI.Dropdown.OptionData(i)));
            
            RepaintMasterText();
            RepaintFXText();
            RepaintMusicText();
            RepaintLookText();
        }

        public override void Repaint()
        {
            switch (state)
            {
                case MainMenuState.Play:
                    defaultPanel.SetActive(false);
                    playPanel.SetActive(true);
                    randomPanel.SetActive(false);
                    optionsPanel.SetActive(false);
                    screenPanel.SetActive(false);
                    creditsPanel.SetActive(false);


                    for (int i = 0; i < slotTexts.Length; i++)
                    {
                        slotTexts[i].text = (GameManager.Instance.gameSlots[i] == null || GameManager.Instance.gameSlots[i].newGame) ? "empty" : GameManager.Instance.gameSlots[i].saveName;
                    }

                    for (int j = 0; j < slotButtons.Length; j++)
                    {
                        if (j == GameManager.Instance.currentSlot)
                        {
                            var block = slotButtons[j].colors;
                            block.normalColor = selectedColor;
                            slotButtons[j].colors = block;
                        }
                        else
                        {
                            var block = slotButtons[j].colors;
                            block.normalColor = unselectedColor;
                            slotButtons[j].colors = block;
                        }
                    }
                    break;
                case MainMenuState.Random:
                    defaultPanel.SetActive(false);
                    playPanel.SetActive(false);
                    randomPanel.SetActive(true);
                    optionsPanel.SetActive(false);
                    screenPanel.SetActive(false);
                    creditsPanel.SetActive(false);
                    break;
                case MainMenuState.Options:
                    defaultPanel.SetActive(false);
                    playPanel.SetActive(false);
                    randomPanel.SetActive(false);
                    optionsPanel.SetActive(true);
                    screenPanel.SetActive(false);
                    creditsPanel.SetActive(false);
                    break;
                case MainMenuState.Screen:
                    defaultPanel.SetActive(false);
                    playPanel.SetActive(false);
                    randomPanel.SetActive(false);
                    optionsPanel.SetActive(false);
                    screenPanel.SetActive(true);
                    creditsPanel.SetActive(false);
                    break;
                case MainMenuState.Credits:
                    defaultPanel.SetActive(false);
                    playPanel.SetActive(false);
                    randomPanel.SetActive(false);
                    optionsPanel.SetActive(false);
                    screenPanel.SetActive(false);
                    creditsPanel.SetActive(true);
                    break;
                case MainMenuState.Default:
                default:

                    if (EventSystem.current.currentSelectedGameObject == null)
                    {
                        EventSystem.current.SetSelectedGameObject(defaultButton.gameObject);
                    }
                    defaultPanel.SetActive(true);
                    playPanel.SetActive(false);
                    randomPanel.SetActive(false);
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
            state = MainMenuState.Random;
            seedInput.text = ((int)System.DateTime.Now.Ticks).ToString();

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

            GameManager.Instance.RandomGame = false;
            //GameManager.Instance.SetGameOver();
            //GameManager.Instance.SetMissionSelect();
            GameManager.Instance.SetPrelude();
        }

        public void NewSeedEvent()
        {
            seedInput.text = ((int)System.DateTime.Now.Ticks).ToString();
        }

        public void StartRandomEvent()
        {
            GameManager.Instance.RandomGame = true;

            int result = 0;
            System.Int32.TryParse(seedInput.text, out result);

            GameManager.Instance.NewRandomGame(result, difficulties[difficultyDropdown.value]);
            GameManager.Instance.Reset();
            GameManager.Instance.EntityPool.DestroyEntities();
            GameManager.Instance.LevelRenderer.LoadRandomLevel(GameManager.Instance.Data.currentLevel);
            AudioManager.Instance.PlayRandomGameMusic();
            GameManager.Instance.SetPlaying();
        }

        public void QuitEvent()
        {
            GameManager.Instance.SetQuit();
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
            Logger.Log(this, "res changd");
            SetResolution();
            GameManager.Instance.PlayerPreferences.Save();

        }

        public void FullScreenChangedEvent()
        {
            Logger.Log(this, "full screen changd");
            SetResolution();
            GameManager.Instance.PlayerPreferences.Save();

        }

        private void SetResolution()
        {
            var resolution = Screen.resolutions[resolutionsDropdown.value];
            GameManager.Instance.PlayerPreferences.resolutionWidth = resolution.width;
            GameManager.Instance.PlayerPreferences.resolutionHeight = resolution.height;
            GameManager.Instance.PlayerPreferences.fullScreen = fullScreenDropdown.value;
            Screen.SetResolution(resolution.width, resolution.height, (FullScreenMode)fullScreenDropdown.value);
        }


        public void Tiny64WindowEvent()
        {

            var resolution = Screen.resolutions[resolutionsDropdown.value];
            GameManager.Instance.PlayerPreferences.resolutionWidth = resolution.width;
            GameManager.Instance.PlayerPreferences.resolutionHeight = resolution.height;
            GameManager.Instance.PlayerPreferences.resolutionRefresh = resolution.refreshRate;
            GameManager.Instance.PlayerPreferences.fullScreen = fullScreenDropdown.value;
            Screen.SetResolution(64, 64, FullScreenMode.Windowed, Screen.currentResolution.refreshRate);
            GameManager.Instance.PlayerPreferences.Save();
        }

        public void Small640WindowEvent()
        {

            var resolution = Screen.resolutions[resolutionsDropdown.value];
            GameManager.Instance.PlayerPreferences.resolutionWidth = resolution.width;
            GameManager.Instance.PlayerPreferences.resolutionHeight = resolution.height;
            GameManager.Instance.PlayerPreferences.resolutionRefresh = resolution.refreshRate;
            GameManager.Instance.PlayerPreferences.fullScreen = fullScreenDropdown.value;
            Screen.SetResolution(640, 640, FullScreenMode.Windowed, Screen.currentResolution.refreshRate);
            GameManager.Instance.PlayerPreferences.Save();
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
        Random,
        Options,
        Screen,
        Credits,
        Quit
    }
}