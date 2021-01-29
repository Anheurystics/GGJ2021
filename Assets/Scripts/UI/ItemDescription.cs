using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemDescription : MonoBehaviour
{
    public float tweenDuration;
    public float backgroundAlpha;
    private Transform itemPanel;
    private Transform itemBackground;
    private Vector3 destPos;
    private Vector3 srcPos;
    private bool isActive;

    // Start is called before the first frame update
    void Start()
    {
        this.tweenDuration = 0.2f;
        this.backgroundAlpha = 0.2f;
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
            this.itemPanel.DOMoveX(this.destPos.x, this.tweenDuration);
            this.itemBackground.GetComponent<Image>().DOFade(this.backgroundAlpha, this.tweenDuration);
        }
        this.isActive = true;
    }

    public void HideItemDescription()
    {
        if (this.isActive)
        {
            this.itemPanel.DOMoveX(this.srcPos.x, this.tweenDuration);
            this.itemBackground.GetComponent<Image>().DOFade(0.0f, this.tweenDuration);
        }
        this.isActive = false;
    }
}
