using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemDescription : MonoBehaviour
{
    private Transform itemPanel;
    private Vector3 destPos;
    private Vector3 srcPos;
    private bool isActive;

    // Start is called before the first frame update
    void Start()
    {
        this.itemPanel = transform.Find("ItemDescriptionPanel");
        this.destPos = new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0);
        this.srcPos = this.itemPanel.position;
        this.isActive = false;
    }

    public void ShowItemDescription()
    {
        if (!this.isActive)
        {
            this.itemPanel.DOMoveX(this.destPos.x, 0.2f);
        }
        this.isActive = true;
    }

    public void HideItemDescription()
    {
        if (this.isActive)
        {
            this.itemPanel.DOMoveX(this.srcPos.x, 0.2f);
        }
        this.isActive = false;
    }
}
