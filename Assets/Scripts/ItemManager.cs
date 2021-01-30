using DG.Tweening;
using UnityEngine;

public class ItemManager : MonoSingleton<ItemManager>
{
    [SerializeField] private Transform itemStart;
    [SerializeField] private Transform itemEnd;
    [SerializeField] private Transform tray;
    [SerializeField] private Item[] itemPrefabs;

    public void SpawnItem()
    {
        var newItem = Instantiate(itemPrefabs.PickRandom(), tray);
        newItem.transform.localPosition = itemStart.localPosition;
        newItem.Randomize();

        newItem.SetSortingLayer("Slot");
        newItem.SetMaskInteraction(SpriteMaskInteraction.VisibleInsideMask);

        newItem.transform.DOLocalMove(itemEnd.localPosition, 1.5f);
    }
}
