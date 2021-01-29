using DG.Tweening;
using UnityEngine;

public class Drawer : MonoBehaviour
{
    public static Drawer opened = null;
    public BoxCollider2D containerCollider;
    public BoxCollider2D itemRootCollider;
    [HideInInspector] public Vector3 originalPos;

    void Start()
    {
        originalPos = transform.position;
    }

    public static void OpenDrawer(Drawer drawer)
    {
        if(opened != null)
        {
            opened.Close();
        }

        opened = drawer;
        if(opened != null)
        {
            opened.Open();
        }
    }

    public void Open()
    {
        containerCollider.gameObject.SetActive(true);
        itemRootCollider.gameObject.SetActive(true);
        transform.DOMoveY(originalPos.y - 4f, 0.2f);
    }

    public void Close()
    {
        transform.DOMoveY(originalPos.y, 0.2f).OnComplete(() => {
            containerCollider.gameObject.SetActive(false);
            itemRootCollider.gameObject.SetActive(false);
        });
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
