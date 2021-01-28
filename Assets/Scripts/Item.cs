using System.Collections;
using UnityEngine;

public class Item : MonoBehaviour
{
    public static Item currentSelected = null;
    
    private BoxCollider2D collider;
    private SpriteRenderer sprite;
    private Vector3 originalScale = Vector3.one;
    
    public SpriteRenderer Sprite => sprite;
    
    void Start()
    {
        collider = GetComponent<BoxCollider2D>();
        sprite = GetComponent<SpriteRenderer>();
    }

    void OnMouseDown()
    {
        if(currentSelected == null)
        {
            currentSelected = this;
            collider.enabled = false;
            originalScale = transform.localScale;
            transform.localScale *= 0.4f;
            sprite.sortingOrder = 1000;
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
                currentSelected = null;
                collider.enabled = true;
                transform.localScale = originalScale;
                
                ItemManager.Instance.BringToTop(this);
            }
        }
    }
}
