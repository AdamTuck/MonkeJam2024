using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class OptionsScreenManager : MonoBehaviour
{
    public AudioMixer audioMixer;
    public void ChangeVolumeLevel(float volume)
    {
        audioMixer.SetFloat("VolumeMaster", volume);
    }
}
