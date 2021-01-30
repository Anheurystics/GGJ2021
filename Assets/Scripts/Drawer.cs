using System.Collections.Generic;
using System.Security.Cryptography;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UIElements;

public class Drawer : MonoBehaviour
{
    public static Drawer opened = null;
    public BoxCollider2D containerCollider;
    public BoxCollider2D itemRootCollider;
    [HideInInspector] public Vector3 originalPos;

    private List<Item> items;

    void Start()
    {
        originalPos = transform.position;
        items = new List<Item>();
    }

    public static void OpenDrawer(Drawer drawer)
    {
        if(opened != null)
        {
            opened.Close();
        }

        opened = drawer;
        if(opened != null)
        {
            opened.Open();
        }
    }

    public void Open()
    {
        containerCollider.gameObject.SetActive(true);
        itemRootCollider.gameObject.SetActive(true);
        transform.DOMoveY(originalPos.y - 4f, 0.2f);
    }

    public void Close()
    {
        transform.DOMoveY(originalPos.y, 0.2f).OnComplete(() => {
            containerCollider.gameObject.SetActive(false);
            itemRootCollider.gameObject.SetActive(false);
        });
    }

    public void AddItem(Item item)
    {
        items.Add(item);
        AdjustItemLayers();
    }

    public void RemoveItem(Item item)
    {
        items.Remove(item);
        AdjustItemLayers();
    }

    private void AdjustItemLayers()
    {
        for(int i = 0; i < items.Count; i++)
        {
            items[i].Sprite.sortingOrder = i;
        }
    }

    void Update()
    {
        itemRootCollider.enabled = (Item.currentSelected != null);
    }

    void OnMouseDown()
    {
        OpenDrawer(opened == this? null : this);
    }
}
