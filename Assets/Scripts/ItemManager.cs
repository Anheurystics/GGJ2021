using UnityEngine;

public class ItemManager : MonoSingleton<ItemManager>
{
    [SerializeField] private Transform tray;
    [SerializeField] private Item itemPrefab;

    public void SpawnItem()
    {
        var newItem = Instantiate(itemPrefab, tray);
        newItem.transform.localPosition = new Vector3(Random.Range(-2f, 2f), Random.Range(-2f, 2f), 0);
        newItem.transform.localScale = Vector3.one;
    }
}
