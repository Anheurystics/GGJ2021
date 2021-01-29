using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class ItemManager : MonoSingleton<ItemManager>
{
    private List<Item> itemLayers;
    [SerializeField] private SortingGroup[] drawers;
    [SerializeField] private Transform tray;
    [SerializeField] private Item itemPrefab;

    private float secondsUntilNextItemSpawn = 5f;

    void Start()
    {
        itemLayers = new List<Item>();
        foreach(var item in GameObject.FindObjectsOfType<Item>())
        {
            itemLayers.Add(item);
        }

        StartCoroutine(DelayedSpawn(secondsUntilNextItemSpawn));
    }

    private IEnumerator DelayedSpawn(float delay)
    {
        yield return new WaitForSeconds(secondsUntilNextItemSpawn);
        var newItem = Instantiate(itemPrefab, tray);
        newItem.transform.localPosition = new Vector3(Random.Range(-2f, 2f), Random.Range(-2f, 2f), 0);
        newItem.transform.localScale = Vector3.one;

        StartCoroutine(DelayedSpawn(secondsUntilNextItemSpawn));
    }

    public void BringToTop(Item item)
    {
        if(itemLayers.Remove(item))
        {
            itemLayers.Add(item);
            for(int i = 0; i < itemLayers.Count; i++)
            {
                itemLayers[i].Sprite.sortingOrder = i;
            }
        }
    }
}
