using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Preview : MonoBehaviour
{
    [SerializeField] private SpriteRenderer[] spriteParts;
    [SerializeField] private Item.SpritePart[] spriteVariants;
    [SerializeField] private SpriteRenderer damagePart;
    [SerializeField] private SpriteRenderer stickerPart;

    public void Set(Item item)
    {
        var sig = item._itemSignature;

        for(int i = 0; i < 2; i++)
        {
            spriteParts[i].sprite = spriteVariants[i].variants[sig[i]];
        }

        damagePart.gameObject.SetActive(false);
        stickerPart.gameObject.SetActive(false);
    }
}
