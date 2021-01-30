using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Customer : MonoBehaviour
{
    [SerializeField] private SpriteRenderer sprite;
    public SpriteRenderer Sprite => sprite;

    public void Spawn(int count)
    {
        sprite.sortingOrder = -count;
        transform.localPosition = new Vector3(3 - (1.5f * count), 4.5f - (0.2f * count));

        var spawnColor = Color.white * (1.0f - (count * 0.2f));
        spawnColor.a = 1.0f;
        sprite.color = spawnColor;
    }

    public void MoveTo(int count, Action onArrive = null)
    {
        sprite.sortingOrder = -count;
        transform.DOLocalMove(new Vector3(3 - (1.5f * count), 4.5f - (0.2f * count)), 2f).OnComplete(() => {
            onArrive?.Invoke();
        });

        var spawnColor = Color.white * (1.0f - (count * 0.2f));
        spawnColor.a = 1.0f;
        sprite.color = spawnColor;
        sprite.DOColor(spawnColor, 2f);
    }
}
