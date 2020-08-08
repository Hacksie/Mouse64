using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource fx = null;
    [SerializeField] private AudioSource music = null;
    [SerializeField] private AudioClip gunshot = null;

    public static AudioManager Instance { get; private set; }

    private AudioManager() => Instance = this;

    public void PlayGunshot()
    {
        fx.clip = gunshot;
        fx.Play();
    }
}
