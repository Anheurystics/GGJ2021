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

    private Item itemRequired = null;
    private int descriptionCount = 0;

    public void ShowBubble(Item _itemRequired = null)
    {
        descriptionCount = 0;
        itemRequired = _itemRequired;
        gameObject.SetActive(true);
        requirementsRoot.gameObject.SetActive(itemRequired != null);
        dropSprite.gameObject.SetActive(itemRequired == null);
        if(itemRequired != null)
        {
            itemType.gameObject.SetActive(true);
            itemType.sprite = itemTypeSilhouettes[itemTypes.IndexOf(itemRequired.itemName)];
            
            mainColor.gameObject.SetActive(false);
            subColor.gameObject.SetActive(false);
            damage.gameObject.SetActive(false);
            sticker.gameObject.SetActive(false);
        }
    }

    public void ShowCharacteristic()
    {
        if(itemRequired == null)
        {
            return;
        }
        int[] sig = itemRequired._itemSignature;
        switch(++descriptionCount)
        {
            case 1:
                mainColor.gameObject.SetActive(true);
                mainColor.sprite = mainColors[sig[0]];
                subColor.gameObject.SetActive(true);
                subColor.sprite = subColors[sig[1]];
                break;
            case 2:
                damage.gameObject.SetActive(true);
                damage.sprite = sig[2] == 1? damageSprite: dotSprite;
                break;
            case 3:
                sticker.gameObject.SetActive(true);
                sticker.sprite = sig[3] == 1? stickerSprite : dotSprite;
                break;
        }
    }

    public void HideBubble()
    {
        gameObject.SetActive(false);
    }
}
