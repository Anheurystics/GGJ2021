using System.Collections;
using DG.Tweening;
using UnityEngine;

public class Item : MonoBehaviour
{
    public static Item currentSelected = null;

    public float holdTime = 0.5f;
    
    [SerializeField] private BoxCollider2D collider;
    [SerializeField] private SpriteRenderer sprite;
    private Vector3 originalScale = Vector3.one;
    private Drawer drawer;
    private ItemDescription itemDescription;
    
    public SpriteRenderer Sprite => sprite;
    
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
                RaycastHit2D[] hits = Physics2D.GetRayIntersectionAll(Camera.main.ScreenPointToRay(Input.mousePosition), Mathf.Infinity, 1 << LayerMask.NameToLayer("ItemRoot"));
                if(hits.Length > 0)
                {
                    foreach(var hit in hits)
                    {
                        var container = hit.collider.transform;
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

                            sprite.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
                            sprite.sortingLayerName = "Drawer";
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
                        
                        sprite.maskInteraction = SpriteMaskInteraction.None;
                        sprite.sortingLayerName = "Mouse";
                        sprite.sortingOrder = 1000;
                    }
                }
            }
        }
    }

    IEnumerator ShowItemDescription()
    {
        yield return new WaitForSeconds(this.holdTime);
        this.itemDescription.ShowItemDescription();
    }
}
