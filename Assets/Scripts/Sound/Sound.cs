using UnityEngine.Audio;
using UnityEngine;
using System;

[System.Serializable]
public class Sound
{
    public string nameSound;

    public AudioClip clip;

    [Range(0f, 1f)]
    public float volume;
    [Range(0f, 3f)]
    public float pitch;

    public bool loop;

    [HideInInspector]
    public AudioSource source;
}
