using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EconomyManager : Singleton<EconomyManager>
{
    public int CurrentCoinAmount => _currentCoinAmount;

    [SerializeField] private TMP_Text _coinText;
    [SerializeField] private int _startingGold = 500;

    private int _currentCoinAmount = 0;

    private void Start() {
        _currentCoinAmount = _startingGold;
        UpdateCoinText();
    }

    public void UpdateCurrentCoinAmount(int amount) {
        _currentCoinAmount += amount;
        UpdateCoinText();
    }

    private void UpdateCoinText() {
        _coinText.text = _currentCoinAmount.ToString("D3");
    }
}
