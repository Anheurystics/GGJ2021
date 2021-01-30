using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Customer : MonoBehaviour
{
    [SerializeField] private SpriteRenderer sprite;
    public SpriteRenderer Sprite => sprite;

    [SerializeField] private Item[] itemPool;
    private bool _hasItem;
    
    public bool hasItem {
        get { return _hasItem; }
    }

    private Item _neededItem;

    [SerializeField] private AudioClip sfxArrive;

    public void SetCustomerType(bool hasItemToGive)
    {
        // Decide on whether they give items or need an item
        _hasItem = hasItemToGive;

        // If need item, set a needed item
        _neededItem = null;
        if (!_hasItem)
        {
            _neededItem = Instantiate(itemPool.PickRandom(), transform);
            _neededItem.gameObject.SetActive(false);  // I just need the item object for the data
            _neededItem.Randomize();
        }
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
            GiveItems(CustomerSpawner.Instance.itemsToGive);
            Invoke(nameof(DespawnCustomer), 0.25f);
            CustomerSpawner.Instance.Bubble.ShowBubble(null);
        }
        else
        {
            CustomerSpawner.Instance.Bubble.ShowBubble(_neededItem);
        }
    }

    private void GiveItems(int n)
    {
        for (int i = 0; i < n; i++)
        {
            ItemManager.Instance.SpawnItem();
        }
    }

    private void DespawnCustomer()
    {
        CustomerSpawner.Instance.DespawnCustomer();
        CustomerSpawner.Instance.Bubble.HideBubble();
    }

    public bool NeedsThisItem(string itemSignature)
    {
        return _neededItem != null && itemSignature == _neededItem.itemSignature;
    }
}
