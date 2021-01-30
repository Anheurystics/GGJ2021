using System;
using UnityEngine;
using UnityEngine.AI;

public class Bubble : MonoBehaviour
{
    [SerializeField] private SpriteRenderer[] requirements;
    [SerializeField] private Transform requirementsRoot;
    [SerializeField] private SpriteRenderer dropSprite;

    public void ShowBubble(Item itemRequired = null)
    {
        gameObject.SetActive(true);
        requirementsRoot.gameObject.SetActive(itemRequired != null);
        dropSprite.gameObject.SetActive(itemRequired == null);
        if(itemRequired != null)
        {

        }
    }

    public void HideBubble()
    {
        gameObject.SetActive(false);
    }
}
