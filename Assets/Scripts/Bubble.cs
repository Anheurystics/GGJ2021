using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Bubble : MonoBehaviour
{
    private List<string> itemTypes = new List<string>{"Book", "Bottle", "Phone", "Umbrella"};
    [SerializeField] private Sprite[] itemTypeSilhouettes;
    [SerializeField] private Sprite[] mainColors;
    [SerializeField] private Sprite[] subColors;
    
    [SerializeField] private SpriteRenderer itemType;
    [SerializeField] private SpriteRenderer mainColor;
    [SerializeField] private SpriteRenderer subColor;
    [SerializeField] private SpriteRenderer damage;
    [SerializeField] private SpriteRenderer sticker;

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
            itemType.sprite = itemTypeSilhouettes[itemTypes.IndexOf(itemRequired.itemName)];
            int[] sig = itemRequired._itemSignature;
            mainColor.sprite = mainColors[sig[0]];
            subColor.sprite = subColors[sig[1]];
            damage.sprite = dotSprite;
            sticker.sprite = dotSprite;
        }
    }

    public void HideBubble()
    {
        gameObject.SetActive(false);
    }
}
