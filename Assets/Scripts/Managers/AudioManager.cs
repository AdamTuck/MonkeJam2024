using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    [Header("Sources")]
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private AudioSource ambientSource, bikeSource, itemSource;
    [SerializeField] private AudioClip sirenAmbient, shopAmbient, dayMusic;
    [SerializeField] private AudioClip bikeAccelerate, bikeCoast;
    [SerializeField] private AudioClip scrapCollect;

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
            default:
                break;
        }
    }

    public void SetBikeAudio (string bikeAudio)
    {
        switch (bikeAudio)
        {
            case "bikeAccelerate":
                if (bikeSource.clip != bikeAccelerate)
                {
                    bikeSource.Stop();
                    bikeSource.clip = bikeAccelerate;
                    bikeSource.Play();
                }
                break;
            case "bikeCoast":
                if (bikeSource.clip != bikeCoast)
                {
                    bikeSource.Stop();
                    bikeSource.clip = bikeCoast;
                    bikeSource.Play();
                }
                break;
            case "none":
                bikeSource.Stop();
                bikeSource.clip = null;
                break;
        }
    }

    public void PlaySFX (string soundToPlay)
    {
        switch (soundToPlay)
        {
            case "scrapCollect":
                itemSource.PlayOneShot(scrapCollect);
                break;
            case "default":
                break;
        }
    }
}
