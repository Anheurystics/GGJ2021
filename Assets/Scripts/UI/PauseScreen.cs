using UnityEngine;

public class PauseScreen : MonoBehaviour
{
    public static bool isPaused = false;
    public static bool AllowPause = true;

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
        if(!AllowPause)
        {
            return;
        }
        // just for debugging score
        Debug.Log("Score: " + GameManager.Instance.score);
        Debug.Log("going...will pause");
        UIManager.Instance.ShowModal(UIManager.Modal.Pause);
        Time.timeScale = 0f;
        isPaused = true;
        Cursor.visible = true;
    }

    public void Resume()
    {
        UIManager.Instance.HideModal();
        Time.timeScale = 1f;
        isPaused = false;
        Cursor.visible = false;
    }
}
