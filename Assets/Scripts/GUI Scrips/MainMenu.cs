using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    
    public GameObject settingsObj;
    public GameObject quitObj;

    public void HandlePlay(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }

    public void HandleSettings()
    {
        settingsObj.SetActive(true);
        quitObj.SetActive(false);
    }

    public void HandleQuit()
    {
        settingsObj.SetActive(false);
        quitObj.SetActive(true);
    }

    public void HandleConfirm()
    {
        Application.Quit();
    }

    public void HandleDeny()
    {
        settingsObj.SetActive(true);
        quitObj.SetActive(false);
    }
}
