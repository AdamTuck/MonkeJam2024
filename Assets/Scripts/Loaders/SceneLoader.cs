using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    //[SerializeField] GameObject menuScreen;
    [SerializeField] GameObject loadScreen;
    [SerializeField] Slider loadBarFill;

    public void LoadSceneWithScreen(string sceneName)
    {
        //menuScreen.SetActive(false);
        loadScreen.SetActive(true);

        if (UIManager.instance)
        {
            UIManager.instance.SetPause(false);
        }

        StartCoroutine(LoadSceneAsync(sceneName));
    }

    public void LoadSceneOnly(string sceneName)
    {
        UIManager.instance.SetPause(false);
        SceneManager.LoadScene(sceneName);
    }

    IEnumerator LoadSceneAsync(string sceneName)
    {
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(sceneName);

        while (!loadOperation.isDone)
        {
            float progressValue = Mathf.Clamp01(loadOperation.progress / 0.9f);
            loadBarFill.value = progressValue;
            yield return null;
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}