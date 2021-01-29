using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemDescription : MonoBehaviour
{
    public float tweenDuration;
    private Transform itemPanel;
    private Transform itemBackground;
    private Vector3 destPos;
    private Vector3 srcPos;
    private bool isActive;

    // Start is called before the first frame update
    void Start()
    {
        this.tweenDuration = 0.5f;
        this.itemPanel = transform.Find("ItemDescriptionPanel");
        this.itemBackground = transform.Find("ItemDescriptionBackground");
        this.destPos = new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0);
        this.srcPos = this.itemPanel.position;
        this.isActive = false;
    }

    public void ShowItemDescription()
    {
        if (!this.isActive)
        {
            this.itemPanel.DOMoveX(this.destPos.x, 0.2f);
            this.itemBackground.GetComponent<Image>().DOFade(0.2f, this.tweenDuration);
        }
        this.isActive = true;
    }

    public void HideItemDescription()
    {
        if (this.isActive)
        {
            this.itemPanel.DOMoveX(this.srcPos.x, 0.2f);
            this.itemBackground.GetComponent<Image>().DOFade(0.0f, this.tweenDuration);
        }
        this.isActive = false;
    }
}
