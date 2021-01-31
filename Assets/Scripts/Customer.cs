using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Customer : MonoBehaviour
{
    [SerializeField] private SpriteRenderer sprite;
    public SpriteRenderer Sprite => sprite;

    public int customerId {
        get { return GetInstanceID(); }
    }

    private bool _hasItem;
    
    public bool hasItem {
        get { return _hasItem; }
    }

    private Item _neededItem;
    public string neededItemSignature {
        get { return _neededItem.itemSignature; }
    }

    public Item[] heldItems;

    [SerializeField] private AudioClip sfxArrive;

    public void SetCustomerType(bool hasItemToGive)
    {
        // Decide on whether they give items or need an item
        _hasItem = hasItemToGive;
    }

    public void SetNeededItem(Item neededItem = null)
    {
        _neededItem = neededItem;
    }

    public void SetHeldItems(Item[] held = null)
    {
        heldItems = held;
    }

    public void Spawn(int count)
    {
        sprite.sortingOrder = -count;
        transform.localPosition = new Vector3(3 - (1.5f * count), 4.5f - (0.2f * count));

        var spawnColor = Color.white * (1.0f - (count * 0.2f));
        spawnColor.a = 1.0f;
        sprite.color = spawnColor;
        if (!_hasItem)
        {
            Debug.Log("needs " + _neededItem.itemSignature);
        }
        else
        {
            Debug.Log("will give items");
        }
    }

    public void MoveTo(int count)
    {
        // Moves a customer to the next spot in the line
        sprite.sortingOrder = -count;
        float xTarget = 3 - (1.5f * count);
        float yTarget = 4.5f - (0.2f * count);
        transform.DOLocalMove(new Vector3(xTarget, yTarget), 2f).OnComplete(() => {
            if (count == 0)
            {
                OnArrive();
                AudioManager.Instance.PlaySFX(sfxArrive);
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
            GiveItems();
            Invoke(nameof(DespawnCustomer), 0.25f);
            CustomerSpawner.Instance.Bubble.ShowBubble(null);
        }
        else
        {
            CustomerSpawner.Instance.Bubble.ShowBubble(_neededItem);
        }
    }

    private void GiveItems()
    {
        // should not be called if user isn't janitor
        for (int i = 0; i < heldItems.Length; i++)
        {
            ItemManager.Instance.SpawnItem(heldItems[i]);
        }
    }

    private void DespawnCustomer()
    {
        CustomerSpawner.Instance.DespawnCustomer();
    }

    public bool NeedsThisItem(string itemSignature)
    {
        if (_neededItem != null)
        {
            Debug.Log("Comparing " + itemSignature + " vs " + _neededItem.itemSignature);
        }
        else
        {
            Debug.Log("Cant compare; needed item be null");
        }
        return _neededItem != null && itemSignature == _neededItem.itemSignature;
    }
}
