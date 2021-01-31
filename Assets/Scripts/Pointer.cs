using UnityEngine;

public class Pointer : MonoBehaviour
{
    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private Sprite closeSprite;
    [SerializeField] private Sprite openSprite;
    
    void Start()
    {
        Cursor.visible = false;
    }

    void Update()
    {
        var pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        // pos.x += 0.25f;
        // pos.y += -0.25f;
        pos.z = 10;

        transform.position = pos;

        sprite.sprite = (Input.GetMouseButton(0) || Input.GetMouseButton(1) || Item.currentSelected != null)? closeSprite : openSprite;
    }
}
