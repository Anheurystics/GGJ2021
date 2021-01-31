using DG.Tweening;
using UnityEngine;

public class ItemManager : MonoSingleton<ItemManager>
{
    [SerializeField] private BoxCollider2D itemStart;
    [SerializeField] private BoxCollider2D itemEnd;
    [SerializeField] private Transform tray;
    [SerializeField] private Item[] itemPrefabs;

    public Item GenerateRandomItem()
    {
        var newItem = Instantiate(itemPrefabs.PickRandom(), tray);
        newItem.Randomize();
        // dont activate it yet
        newItem.gameObject.SetActive(false);
        return newItem;
    }

    public Item GenerateSpecificItem(string itemSignature)
    {
        var itemType = itemSignature.Split(',')[0];
        var newItem = Instantiate(System.Array.Find(itemPrefabs, ele => ele.name == itemType), tray);
        // Debug.Log("Made a new " + newItem);
        newItem.SetItemSignature(itemSignature);
        // dont activate it yet
        newItem.gameObject.SetActive(false);
        return newItem;
    }

    public void SpawnItem(Item newItem)
    {
        newItem.gameObject.SetActive(true);
        newItem.transform.position = RandomPointInBounds(itemStart.bounds);
        newItem.Spin(1.5f);

        newItem.SetSortingLayer("Slot");
        newItem.SetMaskInteraction(SpriteMaskInteraction.VisibleInsideMask);

        newItem.transform.DOMove(RandomPointInBounds(itemEnd.bounds), 1.5f);

        CustomerSpawner.Instance.heldSet.Add(newItem.itemSignature);
    }

    private Vector3 RandomPointInBounds(Bounds bounds)
    {
        var rand = new Vector3(
            Random.Range(bounds.min.x, bounds.max.x),
            Random.Range(bounds.min.y, bounds.max.y),
            Random.Range(bounds.min.z, bounds.max.z)
        );

        return rand;
    }

    void Update()
    {
        if(Item.currentSelected == null)
        {
            if(Input.GetMouseButtonDown(1))
            {
                RaycastHit2D[] hits = Physics2D.GetRayIntersectionAll(Camera.main.ScreenPointToRay(Input.mousePosition), Mathf.Infinity, 1 << LayerMask.NameToLayer("Item"));
                if(hits.Length > 0)
                {
                    var item = hits[0].collider.GetComponent<Item>();
                    if(item != null)
                    {
                        item.Pickup();
                    }
                }
            }
        }
    }
}
