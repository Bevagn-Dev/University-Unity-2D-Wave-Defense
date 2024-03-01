using UnityEngine.Audio;
using UnityEngine;
using System;

public class SoundManager : MonoBehaviour
{
    // Singleton instance
    public static SoundManager Instance { get; private set; }

    // Array of Sound objects
    public Sound[] sounds;

    private void Awake()
    {
        // Ensure only one instance of SoundManager exists
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }

        // Initialize audio sources for each sound
        foreach (Sound sound in sounds)
        {
            sound.audioSource = gameObject.AddComponent<AudioSource>();
            sound.audioSource.clip = sound.audioClip;
            sound.audioSource.pitch = sound.pitch;
            sound.audioSource.loop = sound.loop;
        }
    }

    private void Start()
    {
        // Play default sounds on start
        Play("Theme Song");
        Play("Wind Sound");
    }

    // Play sound by name
    public void Play(string name)
    {
        // Find the sound with the given name
        Sound sound = Array.Find(sounds, s => s.name == name);

        // If sound is found, play it
        if (sound != null)
        {
            sound.audioSource.Play();
        }
        else
        {
            Debug.LogWarning("Sound with name '" + name + "' not found!");
        }
    }
}
