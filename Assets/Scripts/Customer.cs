using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Customer : MonoBehaviour
{
    [SerializeField] private SpriteRenderer sprite;
    public SpriteRenderer Sprite => sprite;

    private bool _hasItem;
    
    public bool hasItem {
        get { return _hasItem; }
    }

    public void Spawn(int count, bool hasItemToGive)
    {
        sprite.sortingOrder = -count;
        transform.localPosition = new Vector3(3 - (1.5f * count), 4.5f - (0.2f * count));

        var spawnColor = Color.white * (1.0f - (count * 0.2f));
        spawnColor.a = 1.0f;
        sprite.color = spawnColor;

        _hasItem = hasItemToGive;
    }

    public void MoveTo(int count)
    {
        // Moves a customer to the next spot in the line
        sprite.sortingOrder = -count;
        transform.DOLocalMove(new Vector3(3 - (1.5f * count), 4.5f - (0.2f * count)), 2f).OnComplete(() => {
            if (count == 0)
            {
                OnArrive();
            }
        });

        var spawnColor = Color.white * (1.0f - (count * 0.2f));
        spawnColor.a = 1.0f;
        sprite.color = spawnColor;
        sprite.DOColor(spawnColor, 2f);
    }

    private void OnArrive()
    {
        if (hasItem)
        {
            ItemManager.Instance.SpawnItem();
            Invoke(nameof(DespawnCustomer), 0.5f);
        }
    }

    private void DespawnCustomer()
    {
        CustomerSpawner.Instance.DespawnCustomer();
    }
}
