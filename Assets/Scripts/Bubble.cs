using System;
using UnityEngine;
using UnityEngine.AI;

public class Bubble : MonoBehaviour
{
    [SerializeField] private Sprite[] itemTypeSilhouettes;
    [SerializeField] private Sprite[] mainColors;
    [SerializeField] private Sprite[] subColors;
    
    [SerializeField] private SpriteRenderer itemType;
    [SerializeField] private SpriteRenderer mainColor;
    [SerializeField] private SpriteRenderer subColor;

    [SerializeField] private Transform requirementsRoot;
    [SerializeField] private SpriteRenderer dropSprite;
    [SerializeField] private Sprite damageSprite;
    [SerializeField] private Sprite stickerSprite;
    [SerializeField] private Sprite dotSprite;

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
