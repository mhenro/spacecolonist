using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public GameSound[] sounds;
    public static AudioManager instance;

    void Awake() {
        if (instance == null) {
            instance = this;
        } else {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        foreach (GameSound sound in sounds) {
            sound.audioSource = gameObject.AddComponent<AudioSource>();
            sound.audioSource.clip = sound.clip;
            sound.audioSource.volume = sound.volume;
            sound.audioSource.pitch = sound.pitch;
            sound.audioSource.loop = sound.loop;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Play(string soundName) {
        AudioSource audioSource = FindAudioSourceByName(soundName);
        audioSource.Play();
    }

    public void PlayOnce(string soundName) {
        AudioSource audioSource = FindAudioSourceByName(soundName);
        if (!audioSource.isPlaying) {
            audioSource.Play();
        }
    }

    public void Stop(string soundName) {
        AudioSource audioSource = FindAudioSourceByName(soundName);
        audioSource.Stop();
    }

    private AudioSource FindAudioSourceByName(string soundName) {
        GameSound sound = Array.Find(sounds, s => s.name == soundName);
        if (sound == null) {
            Debug.LogWarning("Sound: " + soundName + " not found!");
            return new AudioSource();
        }
        return sound.audioSource;
    }

}
