using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering;

public class Item : MonoBehaviour
{
    [Serializable]
    public struct SpritePart
    {
        public Sprite[] variants;
    }

    public static Item currentSelected = null;

    public float holdTime = 0.5f;
    
    [SerializeField] private BoxCollider2D boxCollider;
    [SerializeField] private SortingGroup sortingGroup;
    [SerializeField] private SpriteRenderer[] spriteParts;
    [SerializeField] private SpritePart[] spriteVariants;
    private Vector3 originalScale = Vector3.one;
    private Drawer drawer;
    private ItemDescription itemDescription;
    
    void Start()
    {
        itemDescription = (ItemDescription) GameObject.Find("ItemDescriptionUI").GetComponent(typeof(ItemDescription));
    }

    void OnMouseDown()
    {
        StartCoroutine("ShowItemDescription");
    }

    void OnMouseUp()
    {
        StopCoroutine("ShowItemDescription");
        this.itemDescription.HideItemDescription();
    }

    void Update()
    {
        if(currentSelected == this)
        {
            var mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            // mouseWorldPos.x += 0.5f;
            // mouseWorldPos.y -= 0.5f;
            mouseWorldPos.z = 0;

            transform.position = mouseWorldPos;

            if(Input.GetMouseButtonDown(0))
            {
                int layerMask = 0;
                layerMask |= 1 << LayerMask.NameToLayer("ItemRoot");
                layerMask |= 1 << LayerMask.NameToLayer("Customer");

                RaycastHit2D[] hits = Physics2D.GetRayIntersectionAll(Camera.main.ScreenPointToRay(Input.mousePosition), Mathf.Infinity, layerMask);
                if(hits.Length > 0)
                {
                    foreach(var hit in hits)
                    {
                        var container = hit.collider.transform;
                        var _customer = container.GetComponent<Customer>();
                        if(_customer != null)
                        {
                            Destroy(gameObject);
                            currentSelected = null;
                            CustomerSpawner.Instance.DespawnCustomer();
                            break;
                        }

                        var _drawer = container.parent?.GetComponent<Drawer>();
                        if(_drawer != null)
                        {
                            currentSelected = null;
                            boxCollider.enabled = true;
                            transform.DOScale(originalScale, 0.2f);
                            transform.SetParent(container, true);
                            
                            var pos = transform.localPosition;
                            pos.z = 0;
                            transform.localPosition = pos;
                            
                            drawer = _drawer;
                            drawer.AddItem(this);

                            SetMaskInteraction(SpriteMaskInteraction.VisibleInsideMask);
                            SetSortingLayer("Drawer");

                            break;
                        }
                    }
                }
            }
        }
    }

    public void Pickup()
    {
        if(drawer == null || Drawer.opened == drawer)
        {
            currentSelected = this;
            boxCollider.enabled = false;

            originalScale = transform.localScale;
            transform.DOScale(originalScale * 0.4f, 0.2f);
            
            transform.parent = null;
            drawer?.RemoveItem(this);
            drawer = null;

            SetMaskInteraction(SpriteMaskInteraction.None);
            SetSortingLayer("Mouse");
            SetSortingOrder(1000);                        
        }
    }

    public void Randomize()
    {
        for(int i = 0; i < spriteParts.Length; i++)
        {
            var part = spriteParts[i];
            var variants = spriteVariants[i].variants;

            part.sprite = variants.PickRandom();
        }
    }

    public void Spin(float duration)
    {
        StartCoroutine(_Spin(duration));
    }

    private IEnumerator _Spin(float duration)
    {
        var angle = new Vector3(0, 0, UnityEngine.Random.Range(0, 360));
        transform.eulerAngles = angle;
        float timer = 0;
        while(timer < duration)
        {
            timer += Time.deltaTime;
            angle.z += Time.deltaTime * 30;
            transform.eulerAngles = angle;
            yield return null;
        }
    }

    public void SetMaskInteraction(SpriteMaskInteraction interaction)
    {
        foreach(var part in spriteParts)
        {
            part.maskInteraction = interaction;
        }
    }

    public void SetSortingLayer(string layerName)
    {
        sortingGroup.sortingLayerName = layerName;
        foreach(var part in spriteParts)
        {
            part.sortingLayerName = layerName;
        }
    }

    public void SetSortingOrder(int order)
    {
        sortingGroup.sortingOrder = order;
        foreach(var part in spriteParts)
        {
            part.sortingOrder = order;
        }
    }

    IEnumerator ShowItemDescription()
    {
        yield return new WaitForSeconds(this.holdTime);
        this.itemDescription.ShowItemDescription();
    }
}
