using UnityEngine;

public class ItemManager : MonoSingleton<ItemManager>
{
    [SerializeField] private Sprite[] randomShapes;
    [SerializeField] private Color[] randomColors;
    [SerializeField] private float[] randomScales;
    [SerializeField] private Transform tray;
    [SerializeField] private Item itemPrefab;

    public void SpawnItem()
    {
        var newItem = Instantiate(itemPrefab, tray);
        newItem.transform.localPosition = new Vector3(Random.Range(-2f, 2f), Random.Range(-2f, 2f), 0);
        newItem.transform.localScale = Vector3.one * PickRandom(randomScales);
        newItem.Sprite.sprite = PickRandom(randomShapes);
        newItem.Sprite.color = PickRandom(randomColors);
    }

    private T PickRandom<T>(T[] arr)
    {
        return arr[Random.Range(0, arr.Length)];
    }
}
