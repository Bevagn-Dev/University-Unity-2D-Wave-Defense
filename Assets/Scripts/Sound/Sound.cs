using UnityEngine.Audio;
using UnityEngine;
using System;

[System.Serializable]
public class Sound
{ 
    [Header("Sound Name")]
    public string name;

    [Header("Audio Clip")]
    public AudioClip audioClip;

    [Header("Sound Settings")]
    [Range(0f, 1f)]
    public float volume;
    [Range(0.1f, 3f)]
    public float pitch;

    [Space(15)]

    public bool loop;

    [HideInInspector]
    public AudioSource audioSource;
}
