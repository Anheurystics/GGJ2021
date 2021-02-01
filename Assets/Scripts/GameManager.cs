using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoSingleton<GameManager>
{
    private int _score;
    private bool flawless;
    private bool final_score;
    public int score {
        get { return _score; }
    }

    // Modify these to determine penalties and bonuses
    public int gameDuration = 300;  // 5 minutes
    public int gameClock;
    public int correctDecision = 50;
    public int flawlessBonus = 100;
    public int wrongRejection = -10;
    public int wrongOffer = -0;

    private GameObject clockUI;
    private GameObject endGameModal;
    [SerializeField] private Text finalScoreText;

    private UIManager UI => UIManager.Instance;

    // Start is called before the first frame update
    void Start()
    {
        _score = 0;
        flawless = true;
        final_score = false;
        gameClock = gameDuration;

        clockUI = GameObject.Find("Clock");
        // endGameModal = GameObject.Find("EndGameModal");
        // readySign = GameObject.Find("ReadySign");
        // startSign = GameObject.Find("StartSign");
        // doneSign = GameObject.Find("DoneSign");

        AudioManager.Instance.PlayBGMGame();
        StartCoroutine(nameof(ShowReadyStartSigns));
    }

    IEnumerator ShowReadyStartSigns()
    {
        PauseScreen.AllowPause = false;
        UI.ShowModal(UIManager.Modal.Ready, false);
        yield return new WaitForSeconds(2f);
        UI.HideModal();

        CustomerSpawner.Instance.StartSpawner();

        UI.ShowModal(UIManager.Modal.Start, false);
        yield return new WaitForSeconds(2f);
        UI.HideModal();
        PauseScreen.AllowPause = true;

        yield return DecrementClock();
    }

    IEnumerator DecrementClock()
    {
        while (gameClock > 0)
        {
            yield return new WaitForSeconds(1f);
            gameClock--;
            clockUI.transform.Find("TimeRemaining").GetComponent<Text>().text = gameClock.ToString();
        }
        Debug.Log("Done!");
        StartCoroutine(nameof(TimesUp));
    }

    IEnumerator TimesUp()
    {
        PauseScreen.AllowPause = false;
        for (int i = 0; i < CustomerSpawner.Instance.queueSize; i++)
        {
            CustomerSpawner.Instance.DespawnCustomer();
        }

        UI.ShowModal(UIManager.Modal.Done, false);
        yield return new WaitForSeconds(1.5f);
        UI.HideModal();

        EndGame();
    }

    public void EndGame()
    {
        finalScoreText.text = ComputeFinalScore().ToString();
        UI.ShowModal(UIManager.Modal.EndGame);
    }

    public void AddCustomerResolved(bool correct)
    {
        // Call on customer despawn
        if (correct)
        {
            _score += correctDecision;
        }
        else
        {
            _score += wrongRejection;
            flawless = false;
        }
    }

    public void AddWrongItemGiven()
    {
        // Call on giving the wrong item
        // but the customer is still there
        _score += wrongOffer;
        flawless = false;
    }

    public int ComputeFinalScore()
    {
        if (!final_score)
        {
            final_score = true;
            // Compute bonuses here
            if (flawless)
            {
                _score += flawlessBonus;
            }
        }
        return _score;
    }
}
