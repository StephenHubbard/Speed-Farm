using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSprite : MonoBehaviour
{
    [SerializeField] private Sprite[] allRockSprites;

    private SpriteRenderer _spriteRenderer;

    private void Awake() {
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    private void Start() {
        _spriteRenderer.sprite = allRockSprites[Random.Range(0, allRockSprites.Length)];
    }
}
