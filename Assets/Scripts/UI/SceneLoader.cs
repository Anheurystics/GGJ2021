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
        StartCoroutine("LoadSceneAnimation", sceneIndex);
    }

    IEnumerator LoadSceneAnimation(int sceneIndex)
    {
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(this.transitionTime);
        SceneManager.LoadScene(sceneIndex);
    }
}
