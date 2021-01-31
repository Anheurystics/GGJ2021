using UnityEngine;
using UnityEngine.Events;

public class SpriteButton : MonoBehaviour
{
    [SerializeField] private Sprite downButton;
    [SerializeField] private Sprite upButton;
    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private UnityEvent onClick;
    [SerializeField] private AudioClip sfxPress;
    
    void OnMouseDown()
    {
        sprite.sprite = downButton;
    }

    void OnMouseExit()
    {
        sprite.sprite = upButton;
    }

    void OnMouseUpAsButton()
    {
        sprite.sprite = upButton;
        AudioManager.Instance.PlaySFX(sfxPress);
        onClick?.Invoke();
    }
}
