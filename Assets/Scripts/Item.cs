using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering;

public class Item : MonoBehaviour
{
    [System.Serializable]
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
    [SerializeField] private AudioClip sfxPickup;
    [SerializeField] private AudioClip sfxPutdown;
    private Vector3 originalScale = Vector3.one;
    private Drawer drawer;
    private ItemDescription itemDescription;

    private int[] _itemSignature;
    public string itemName {
        get { return this.gameObject.name.Split('(', ' ')[0]; }
    }

    public string itemSignature {
        get { return itemName + "," + string.Join(",", _itemSignature); }
    }

    private float shrinkScale = 0.4f;
    
    public void SetItemSignature(string signature)
    {
        // Define an object based on an item signature
        // Basically does the opposite of Randomize()
        var signatureParts = signature.Split(',');
        if (signatureParts[0] != itemName)
        {
            Debug.LogWarning("Signature and item type do not match: " + signatureParts[0] + " vs " + itemName);
        }
        if (signatureParts.Length - 1 != spriteParts.Length)
        {
            Debug.LogError("Signature and Parts list do not match: " + (signatureParts.Length - 1) + " vs " + spriteParts.Length);
        }

        _itemSignature = new int[spriteParts.Length];
        for (int i = 0; i < signatureParts.Length - 1; i++)
        {
            // Debug.Log("Parsing part " + i);
            var part = spriteParts[i];
            var variants = spriteVariants[i].variants;

            int sp = 0;
            if (System.Int32.TryParse(signatureParts[i + 1], out sp))
            {
                // Debug.Log("Choosing variant " + sp);
                part.sprite = variants[sp];
                _itemSignature[i] = sp;
            }
            else
            {
                Debug.LogError("Could not parse signature");
            }
        }
    }

    void Start()
    {
        itemDescription = (ItemDescription) GameObject.Find("ItemDescriptionUI").GetComponent(typeof(ItemDescription));
        originalScale = transform.localScale;
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
                            if (_customer.NeedsThisItem(itemSignature))
                            {
                                Debug.Log("Thanks!");
                                Destroy(gameObject);
                                currentSelected = null;
                                CustomerSpawner.Instance.DespawnCustomer();
                            }
                            else
                            {
                                Debug.Log("Not mine but can I have it?");
                            }
                            break;
                        }

                        var _drawer = container.parent?.GetComponent<Drawer>();
                        if(_drawer != null)
                        {
                            currentSelected = null;
                            boxCollider.enabled = true;
                            transform.DOKill();
                            transform.localScale = originalScale * shrinkScale;
                            transform.DOScale(originalScale, 0.2f);
                            transform.SetParent(container, true);
                            
                            var pos = transform.localPosition;
                            pos.z = 0;
                            transform.localPosition = pos;
                            
                            drawer = _drawer;
                            drawer.AddItem(this);

                            SetMaskInteraction(SpriteMaskInteraction.VisibleInsideMask);
                            SetSortingLayer("Drawer");

                            AudioManager.Instance.PlaySFX(sfxPutdown);
                            break;
                        }
                    }
                }
            }

            if(Input.mouseScrollDelta.y != 0)
            {
                var angle = transform.eulerAngles;
                angle.z += Input.mouseScrollDelta.y * 1800 * Time.deltaTime;
                transform.eulerAngles = angle;
            }
        }
    }

    public void Pickup()
    {
        if(drawer == null || Drawer.opened == drawer)
        {
            currentSelected = this;
            boxCollider.enabled = false;

            transform.DOKill();
            transform.localScale = originalScale;
            transform.DOScale(originalScale * shrinkScale, 0.2f);
            
            transform.parent = null;
            drawer?.RemoveItem(this);
            drawer = null;

            SetMaskInteraction(SpriteMaskInteraction.None);
            SetSortingLayer("Mouse");
            SetSortingOrder(1000);                        

            AudioManager.Instance.PlaySFX(sfxPickup);
            Debug.Log(itemSignature);
        }
    }

    public void Randomize()
    {
        _itemSignature = new int[spriteParts.Length];
        for(int i = 0; i < spriteParts.Length; i++)
        {
            var part = spriteParts[i];
            var variants = spriteVariants[i].variants;

            var rand = Random.Range(0, variants.Length);
            part.sprite = variants[rand];
            _itemSignature[i] = rand;
        }
    }

    public void Spin(float duration)
    {
        StartCoroutine(_Spin(duration));
    }

    private IEnumerator _Spin(float duration)
    {
        var spinDir = Random.Range(-45, 45);
        var angle = new Vector3(0, 0, Random.Range(0, 360));
        transform.eulerAngles = angle;
        float timer = 0;
        while(timer < duration)
        {
            timer += Time.deltaTime;
            angle.z += Time.deltaTime * spinDir;
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
