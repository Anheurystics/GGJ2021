using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class ItemManager : MonoSingleton<ItemManager>
{
    private List<Item> itemLayers;
    [SerializeField] private SortingGroup[] drawers;

    void Start()
    {
        itemLayers = new List<Item>();
        foreach(var item in GameObject.FindObjectsOfType<Item>())
        {
            itemLayers.Add(item);
        }
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
