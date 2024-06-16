using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreen : MonoBehaviour
{
    [SerializeField] private GameObject mainMenuScreen;
    [SerializeField] private GameObject optionsScreen;
    public void StartGame()
    {
        //mainMenuScreen.SetActive(false);
        //GameManager.instance.ChangeState(GameManager.GameState.DayStart,);
        SceneManager.LoadScene(0);
    }

    public void OpenOptions()
    {
        optionsScreen.SetActive(true);
    }
    public void CloseOptions()
    {
        optionsScreen.SetActive(false);
    }
}
