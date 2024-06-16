using System.Collections;
using System.Collections.Generic;
using UnityEngine;
<<<<<<< HEAD
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    [Header("Sources")]
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private AudioSource ambientSource, bikeSource;
    [SerializeField] private AudioClip sirenAmbient, shopAmbient, dayMusic;

    // Singleton
    public static SoundManager instance;
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
=======

public class SoundManager : MonoBehaviour
{
    AudioSource m_AudioSource;

>>>>>>> BurryBurstBranch
}
