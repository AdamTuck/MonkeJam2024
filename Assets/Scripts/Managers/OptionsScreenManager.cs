using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class OptionsScreenManager : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;

    public void ChangeVolumeLevel(float volume)
    {
        audioMixer.SetFloat("VolumeMaster", volume);
    }

    public void ChangeMouseSensitivity (float sensitivity)
    {
        CameraMovementBehaviour.instance.sensitivityMultiplier = sensitivity;
    }

    public void ChangeInvertMouse (bool _invertMouse)
    {
        CameraMovementBehaviour.instance.invertMouse = _invertMouse;
    }
}