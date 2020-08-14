using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource fx = null;
    [SerializeField] private AudioSource music = null;
    [SerializeField] private AudioClip activate = null;
    [SerializeField] private AudioClip gunshot = null;
    [SerializeField] private AudioClip energy = null;
    [SerializeField] private AudioClip denied = null;
    [SerializeField] private AudioClip bullet = null;
    [SerializeField] private AudioClip menuMusic = null;
    [SerializeField] private AudioClip selectMusic = null;
    [SerializeField] private AudioClip successMusic = null;
    [SerializeField] private AudioClip deathMusic = null;
    [SerializeField] private AudioClip[] playMusic = null;

    public static AudioManager Instance { get; private set; }

    private AudioManager() => Instance = this;

    public void PlayGunshot()
    {
        fx.clip = gunshot;
        fx.Play();
    }

    public void PlayEnergyPickup()
    {
        fx.clip = energy;
        fx.Play();
    }

    public void PlayBulletPickup()
    {
        fx.clip = bullet;
        fx.Play();
    }    

    public void PlayActivate()
    {
        fx.clip = activate;
        fx.Play();
    }    

    public void PlayDenied()
    {
        fx.clip = denied;
        fx.Play();
    } 

    public void PlayMenuMusic()
    {
        music.clip = menuMusic;
        music.Play();
    }

    public void PlayMissionSelectMusic()
    {
        music.clip = selectMusic;
        music.Play();
    }

    public void PlayRandomGameMusic()
    {
        music.clip = playMusic[Random.Range(0, playMusic.Length)];
        music.Play();
    }

    public void PlayMissionSuccessMusic()
    {
        music.clip = successMusic;
        music.Play();
    }

    public void PlayDeathMusic()
    {
        music.clip = deathMusic;
        music.Play();
    }
}
