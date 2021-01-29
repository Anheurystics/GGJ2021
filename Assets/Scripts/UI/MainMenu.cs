using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject sceneLoader;
    public void PlayGame()
    {
        SceneLoader sl = sceneLoader.GetComponent<SceneLoader>();
        sl.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void ExitGame()
    {
        Debug.Log("Quitting...");
        Application.Quit();
    }

    public void ShowCredits()
    {
        Debug.Log("Showing Credits...");
    }
}
