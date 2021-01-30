using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.AI;

public class Drawer : MonoBehaviour
{
    public static Drawer opened = null;
    public BoxCollider2D containerCollider;
    public BoxCollider2D itemRootCollider;
    [HideInInspector] public Vector3 originalPos;

    [SerializeField] private AudioClip sfxDrawerOpen;
    [SerializeField] private AudioClip sfxDrawerClose;

    private List<Item> items;

    void Start()
    {
        originalPos = transform.position;
        items = new List<Item>();
    }

    public static void OpenDrawer(Drawer drawer)
    {
        bool playSfx = true;
        if(opened != null)
        {
            opened.Close(playSfx);
            playSfx = false;
        }

        opened = drawer;
        if(opened != null)
        {
            opened.Open(playSfx);
        }
    }

    public void Open(bool playSfx = true)
    {
        containerCollider.gameObject.SetActive(true);
        itemRootCollider.gameObject.SetActive(true);
        transform.DOMoveY(originalPos.y - 4f, 0.2f);
        if(playSfx)
        {
            AudioManager.Instance.PlaySFX(sfxDrawerOpen);
        }
    }

    public void Close(bool playSfx = true)
    {
        transform.DOMoveY(originalPos.y, 0.2f).OnComplete(() => {
            containerCollider.gameObject.SetActive(false);
            itemRootCollider.gameObject.SetActive(false);
        });
        if(playSfx)
        {
            AudioManager.Instance.PlaySFX(sfxDrawerClose);
        }
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
            items[i].SetSortingOrder(i);
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
