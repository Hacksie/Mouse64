
using UnityEngine;
using UnityEngine.Audio;
using System.Linq;

namespace HackedDesign
{
    public class PlayerPreferences
    {
        public int resolutionWidth;
        public int resolutionHeight;
        public int resolutionRefresh;
        public int fullScreen;
        public float lookSpeed;
        public float masterVolume;
        public float sfxVolume;
        public float musicVolume;

        private AudioMixer mixer;

        public PlayerPreferences(AudioMixer mixer)
        {
            this.mixer = mixer;
        }

        public void Save()
        {
            PlayerPrefs.SetInt("ResolutionWidth", resolutionWidth);
            PlayerPrefs.SetInt("ResolutionHeight", resolutionHeight);
            PlayerPrefs.SetInt("ResolutionRefresh", resolutionRefresh);

            PlayerPrefs.SetInt("FullScreen", fullScreen);
            PlayerPrefs.SetFloat("LookSpeed", lookSpeed);
            PlayerPrefs.SetFloat("MasterVolume", sfxVolume);
            PlayerPrefs.SetFloat("FXVolume", sfxVolume);
            PlayerPrefs.SetFloat("MusicVolume", musicVolume);
        }

        public void Load()
        {
            resolutionWidth = PlayerPrefs.GetInt("ResolutionWidth", Screen.currentResolution.width);
            resolutionHeight = PlayerPrefs.GetInt("ResolutionHeight", Screen.currentResolution.height);
            resolutionRefresh = PlayerPrefs.GetInt("ResolutionRefresh", Screen.currentResolution.refreshRate);
            fullScreen = PlayerPrefs.GetInt("FullScreen", (int)Screen.fullScreenMode);
            lookSpeed = PlayerPrefs.GetFloat("LookSpeed", 180);
            masterVolume = PlayerPrefs.GetFloat("MasterVolume", 100);
            sfxVolume = PlayerPrefs.GetFloat("FXVolume", 100);
            musicVolume = PlayerPrefs.GetFloat("MusicVolume", 100);
            SetPreferences();
        }

        public void SetPreferences()
        {
            this.mixer.SetFloat("MasterVolume", this.masterVolume);
            this.mixer.SetFloat("FXVolume", this.sfxVolume);
            this.mixer.SetFloat("MusicVolume", this.musicVolume);

            Resolution scr = Screen.resolutions.FirstOrDefault(r => r.width == this.resolutionWidth && r.height == this.resolutionHeight && r.refreshRate == this.resolutionRefresh);

            Logger.Log("Player Preferences", scr.width.ToString(), " x ", scr.height.ToString());
            Screen.SetResolution(scr.width, scr.height, (FullScreenMode)this.fullScreen, scr.refreshRate);
        }
    }
}
