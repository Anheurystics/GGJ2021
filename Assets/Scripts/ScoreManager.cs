using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoSingleton<ScoreManager>
{
    private int _score;
    private bool flawless;
    private bool final_score;
    public int score {
        get { return _score; }
    }

    // Modify these to determine penalties and bonuses
    public int correctDecision = 50;
    public int flawlessBonus = 100;
    public int wrongRejection = -10;
    public int wrongOffer = -0;

    // Start is called before the first frame update
    void Start()
    {
        _score = 0;
        flawless = true;
        final_score = false;
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
