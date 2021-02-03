using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject sceneLoader;
    void Start()
    {
        AudioManager.Instance.PlayBGMMenu();
        Cursor.visible = true;
    }

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
        UIManager.Instance.ShowModal(UIManager.Modal.Credits);
    }

    public void ShowHelp()
    {
        UIManager.Instance.ShowModal(UIManager.Modal.Help);
    }
}
