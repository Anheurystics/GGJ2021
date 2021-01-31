using System.Collections.Generic;
using UnityEngine;

public class PreviewManager : MonoSingleton<PreviewManager>
{
    private List<string> itemTypes = new List<string>{"Book", "Bottle", "Phone", "Umbrella"};
    [SerializeField] private Preview[] previews;
    [SerializeField] private GameObject tint;
    private Preview currentPreview;

    public void ShowPreview(Item item)
    {
        if(currentPreview != null)
        {
            currentPreview.gameObject.SetActive(false);
        }

        tint.SetActive(true);
        currentPreview = previews[itemTypes.IndexOf(item.itemName)];
        currentPreview.gameObject.SetActive(true);
        currentPreview.Set(item);
    }

    public void HidePreview()
    {
        tint.SetActive(false);
        currentPreview?.gameObject.SetActive(false);
        currentPreview = null;
    }
}
