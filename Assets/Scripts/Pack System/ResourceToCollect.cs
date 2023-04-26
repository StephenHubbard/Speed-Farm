using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ResourceToCollect : MonoBehaviour
{
    public ItemSO _itemSO { get; private set; }
    private TMP_Text _amountText;
    private int _amountToCollect = 0;

    private void Awake() {
        _amountText = GetComponentInChildren<TMP_Text>();
    }

    private void Start() {
        UpdateAmountText();
    }

    public void IncreaseAmountToCollectByOne() {
        _amountToCollect++;
        UpdateAmountText();
    }

    public void SetItemSO(ItemSO itemSO) {
        _itemSO = itemSO;
    }

    private void UpdateAmountText(){
        _amountText.text = _amountToCollect.ToString();
    }
}
