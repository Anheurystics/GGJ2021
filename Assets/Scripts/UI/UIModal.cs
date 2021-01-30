using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIModal : MonoBehaviour
{
    public float tweenDuration = 0.2f;
    public float backgroundAlpha = 0.5f;
    private Transform modalPanel;
    private Transform modalBackground;
    private Vector3 destPos;
    private Vector3 srcPos;
    private bool isActive;

    // Start is called before the first frame update
    void Start()
    {
        this.modalPanel = transform.Find("UIModalPanel");
        this.modalBackground = transform.Find("UIModalBackground");
        this.modalBackground.gameObject.SetActive(false);

        // Tween modal to center
        this.destPos = new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0);
        this.srcPos = this.modalPanel.position;
        this.isActive = false;
    }

    public void ShowModal()
    {
        if (!this.isActive)
        {
            this.modalBackground.gameObject.SetActive(true);
            this.modalPanel.DOMoveX(this.destPos.x, this.tweenDuration);
            this.modalBackground.GetComponent<Image>().DOFade(this.backgroundAlpha, this.tweenDuration);
        }
        this.isActive = true;
    }

    public void HideModal()
    {
        Debug.Log("hiding modal...");
        if (this.isActive)
        {
            this.modalBackground.gameObject.SetActive(false);
            this.modalPanel.DOMoveX(this.srcPos.x, this.tweenDuration);
            this.modalBackground.GetComponent<Image>().DOFade(0.0f, this.tweenDuration);
        }
        this.isActive = false;
    }
}
