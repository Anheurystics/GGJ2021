using DG.Tweening;
using UnityEngine;

public class UIManager : MonoSingleton<UIManager>
{
    public enum Modal
    {
        Pause,
        EndGame,
        Ready,
        Start,
        Done,
        Credits
    }
    
    [SerializeField] private GameObject tint;
    [SerializeField] private RectTransform[] modals;

    private Vector2 startPos = new Vector2(-500, 0);
    private Vector2 endPos = new Vector2(0, 0);

    private RectTransform currentModal = null;

    void Start()
    {
        foreach(var modal in modals)
        {
            modal.anchoredPosition = startPos;
            modal.gameObject.SetActive(false);
        }
    }
    
    public void ShowModal(Modal modalIndex, bool setTint = true)
    {
        tint.SetActive(setTint);
        
        currentModal = modals[(int)modalIndex];
        currentModal.gameObject.SetActive(true);
        currentModal.anchoredPosition = startPos;
        var moveTween = currentModal.DOMove(endPos, 0.2f);
        moveTween.SetUpdate(true);
    }

    public void HideModal()
    {
        tint.SetActive(false);

        if(currentModal)
        {
            currentModal.anchoredPosition = endPos;
            currentModal.DOMove(startPos, 0.2f).SetUpdate(true);
            currentModal = null;
        }
    }
    
}
