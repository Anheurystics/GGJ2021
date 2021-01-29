using System.Collections;
using System.Runtime.InteropServices;
using DG.Tweening;
using UnityEngine;

public class Item : MonoBehaviour
{
    public static Item currentSelected = null;
    
    private BoxCollider2D collider;
    private SpriteRenderer sprite;
    private Vector3 originalScale = Vector3.one;
    private Drawer drawer;
    
    public SpriteRenderer Sprite => sprite;
    
    void Start()
    {
        collider = GetComponent<BoxCollider2D>();
        sprite = GetComponent<SpriteRenderer>();
    }

    void OnMouseDown()
    {
        if(currentSelected == null && (drawer == null || Drawer.opened == drawer))
        {
            currentSelected = this;
            collider.enabled = false;
            originalScale = transform.localScale;
            transform.DOScale(originalScale * 0.4f, 0.2f);
            sprite.sortingOrder = 1000;
            transform.parent = null;
            drawer = null;
        }
    }

    void Update()
    {
        if(currentSelected == this)
        {
            var mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouseWorldPos.x += 0.5f;
            mouseWorldPos.y -= 0.5f;
            mouseWorldPos.z = 0;

            transform.position = mouseWorldPos;

            if(Input.GetMouseButton(1))
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
                            ItemManager.Instance.BringToTop(this);
                            break;
                        }
                    }
                }
            }
        }
    }
}
