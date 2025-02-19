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
    [SerializeField] private AudioClip sfxCorrect;
    [SerializeField] private AudioClip sfxWrong;

    private Vector3 originalScale = Vector3.one;
    private Drawer drawer;

    public int[] _itemSignature;
    public string itemName;

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

        _itemSignature = new int[spriteParts.Length + 2];
        for (int i = 0; i < signatureParts.Length - 1; i++)
        {
            int sp = 0;
            if (System.Int32.TryParse(signatureParts[i + 1], out sp))
            {
                if(i < spriteParts.Length)
                {
                    var part = spriteParts[i];
                    var variants = spriteVariants[i].variants;
                    part.sprite = variants[sp];
                }
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
        originalScale = transform.localScale;
    }

    void OnMouseDown()
    {
        StartCoroutine("ShowItemDescription");
    }

    void OnMouseUp()
    {
        StopCoroutine("ShowItemDescription");
        PreviewManager.Instance.HidePreview();
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
                            Debug.Log("Raycasted customer " + _customer.customerId);
                            if (_customer.customerId != CustomerSpawner.Instance.currentlyServing)
                            {
                                // Customer is not the one you're serving right now, move on
                                continue;
                            }
                            if (_customer.NeedsThisItem(itemSignature))
                            {
                                // You gave the right item
                                Debug.Log("Thanks!");
                                GameManager.Instance.AddCustomerResolved(true);
                                CustomerSpawner.Instance.heldSet.Remove(itemSignature);
                                Destroy(gameObject);
                                currentSelected = null;
                                CustomerSpawner.Instance.DespawnCustomer();
                                AudioManager.Instance.PlaySFX(sfxCorrect);
                            }
                            else
                            {
                                Debug.Log("Not mine but can I have it?");
                                GameManager.Instance.AddWrongItemGiven();
                                AudioManager.Instance.PlaySFX(sfxWrong);
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
        _itemSignature = new int[spriteParts.Length + 2];
        for(int i = 0; i < spriteParts.Length; i++)
        {
            var part = spriteParts[i];
            var variants = spriteVariants[i].variants;

            var rand = Random.Range(0, variants.Length);
            part.sprite = variants[rand];
            _itemSignature[i] = rand;
        }

        _itemSignature[spriteParts.Length] = Random.Range(0, 2);
        _itemSignature[spriteParts.Length + 1] = Random.Range(0, 2);
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
        PreviewManager.Instance.ShowPreview(this);
    }
}
