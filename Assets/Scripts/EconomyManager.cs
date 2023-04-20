using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EconomyManager : Singleton<EconomyManager>
{
    [SerializeField] private TMP_Text _coinText;

    private int _currentCoinAmount = 0;

    public void UpdateCurrentCoinAmount(int amount) {
        _currentCoinAmount += amount;
        UpdateCoinText();
    }

    private void UpdateCoinText() {
        _coinText.text = _currentCoinAmount.ToString("D3");
    }
}
