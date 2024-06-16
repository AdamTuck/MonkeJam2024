using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    [Header("Sources")]
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private AudioSource ambientSource, bikeSource;
    [SerializeField] private AudioClip sirenAmbient, shopAmbient, dayMusic;
    
    // Singleton
    public static AudioManager instance;
    private void Awake()
    {
        if (instance && instance != this)
        {
            Destroy(instance);
            return;
        }
        instance = this;
    }

    public void SetAmbience(string ambience)
    {
        ambientSource.Stop();

        switch (ambience)
        {
            case "storm":
                ambientSource.PlayOneShot(sirenAmbient);
                break;
            case "shop":
                ambientSource.PlayOneShot(shopAmbient);
                break;
            case "dayMusic":
                ambientSource.clip = dayMusic;
                ambientSource.Play();
                break;
        }
    }
}