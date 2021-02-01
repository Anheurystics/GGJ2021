using UnityEngine;
using UnityEngine.UI;

public class Pointer : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private Image image;
    [SerializeField] private Sprite closeSprite;
    [SerializeField] private Sprite openSprite;
    
    void Start()
    {
        Cursor.visible = false;
    }

    void Update()
    {
        var pos = cam.ScreenToWorldPoint(Input.mousePosition);
        pos.x += 20f;
        pos.y += -20f;
        pos.z = 10;

        transform.position = pos;

        image.sprite = (Input.GetMouseButton(0) || Input.GetMouseButton(1) || Item.currentSelected != null)? closeSprite : openSprite;
    }
}
