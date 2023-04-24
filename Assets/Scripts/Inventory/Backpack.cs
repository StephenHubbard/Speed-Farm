using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Backpack : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Sprite _backPackDefault;
    [SerializeField] private Sprite _backPackHighlight;

    private Image _image;

    private void Awake() {
        _image = GetComponent<Image>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _image.sprite = _backPackHighlight;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _image.sprite = _backPackDefault;

    }
}
