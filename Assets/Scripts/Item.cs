using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

public class Item : MonoBehaviour
{
    [Serializable]
    public struct SpritePart
    {
        public Sprite[] variants;
    }

    public static Item currentSelected = null;

    public float holdTime = 0.5f;
    
    [SerializeField] private BoxCollider2D collider;
    [SerializeField] private SpriteRenderer[] spriteParts;
    [SerializeField] private SpritePart[] spriteVariants;
    private Vector3 originalScale = Vector3.one;
    private Drawer drawer;
    private ItemDescription itemDescription;
    
    void Start()
    {
        itemDescription = (ItemDescription) GameObject.Find("ItemDescriptionUI").GetComponent(typeof(ItemDescription));
    }

    private bool hover = false;

    void OnMouseEnter()
    {
        hover = true;
    }

    void OnMouseExit()
    {
        hover = false;
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
                            collider.enabled = true;
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
        else
        {
            if(hover)
            {
                if(Input.GetMouseButtonDown(1))
                {
                    if(currentSelected == null && (drawer == null || Drawer.opened == drawer))
                    {
                        currentSelected = this;
                        collider.enabled = false;

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
            }
        }
    }

    public void Randomize()
    {
        for(int i = 0; i < spriteParts.Length; i++)
        {
            var part = spriteParts[i];
            var variants = spriteVariants[i].variants;

            part.sprite = variants[UnityEngine.Random.Range(0, variants.Length)];
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
        foreach(var part in spriteParts)
        {
            part.sortingLayerName = layerName;
        }
    }

    public void SetSortingOrder(int order)
    {
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
