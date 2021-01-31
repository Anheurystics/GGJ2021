using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseScreen : MonoBehaviour
{
    public static bool isPaused = false;
    private UIModal pauseModal;

    void Awake()
    {
        pauseModal = this.GetComponent<UIModal>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Pause()
    {
        // just for debugging score
        Debug.Log("Score: " + ScoreManager.Instance.score);
        Debug.Log("going...will pause");
        pauseModal.ShowModal();
        // Need to let the animation play before pausing
        Invoke(nameof(PauseAfterAnimation), pauseModal.tweenDuration);
    }

    private void PauseAfterAnimation()
    {
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void Resume()
    {
        Time.timeScale = 1f;
        isPaused = false;
        pauseModal.HideModal();
    }
}
