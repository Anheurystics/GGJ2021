using UnityEngine;
using UnityEngine.Events;

public class SpriteButton : MonoBehaviour
{
    [SerializeField] private Sprite downButton;
    [SerializeField] private Sprite upButton;
    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private UnityEvent onClick;
    
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
        onClick?.Invoke();
    }
}
