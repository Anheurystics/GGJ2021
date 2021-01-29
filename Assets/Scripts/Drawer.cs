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
        transform.position = originalPos - (Vector3.up * 4);
        containerCollider.gameObject.SetActive(true);
        itemRootCollider.gameObject.SetActive(true);
    }

    public void Close()
    {
        transform.position = originalPos;
        containerCollider.gameObject.SetActive(false);
        itemRootCollider.gameObject.SetActive(false);
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
