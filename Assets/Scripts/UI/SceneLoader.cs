using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public Animator transition;
    public float transitionTime = 1.0f;

    public void LoadScene(int sceneIndex)
    {
        if (Time.timeScale == 0f)
        {
            // If paused, need to unpause
            Time.timeScale = 1f;
        }
        StartCoroutine("LoadSceneAnimation", sceneIndex);
    }

    IEnumerator LoadSceneAnimation(int sceneIndex)
    {
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(this.transitionTime);
        SceneManager.LoadScene(sceneIndex);
    }
}
