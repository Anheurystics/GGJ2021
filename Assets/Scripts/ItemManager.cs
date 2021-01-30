using DG.Tweening;
using UnityEngine;

public class ItemManager : MonoSingleton<ItemManager>
{
    [SerializeField] private BoxCollider2D itemStart;
    [SerializeField] private BoxCollider2D itemEnd;
    [SerializeField] private Transform tray;
    [SerializeField] private Item[] itemPrefabs;

    public void SpawnItem()
    {
        var newItem = Instantiate(itemPrefabs.PickRandom(), tray);
        newItem.transform.position = RandomPointInBounds(itemStart.bounds);
        newItem.Randomize();
        newItem.Spin(1.5f);

        newItem.SetSortingLayer("Slot");
        newItem.SetMaskInteraction(SpriteMaskInteraction.VisibleInsideMask);

        newItem.transform.DOMove(RandomPointInBounds(itemEnd.bounds), 1.5f);
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
