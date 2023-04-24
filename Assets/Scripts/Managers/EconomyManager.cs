using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EconomyManager : Singleton<EconomyManager>
{
    public int CurrentCoinAmount => _currentCoinAmount;
    public int CurrentAmountToBuyLandPlot => _currentAmountToBuyLandPlot;

    [SerializeField] private TMP_Text _coinText;
    [SerializeField] private TMP_Text _amountToBuyLandPlotText;
    [SerializeField] private int _startingGold = 500;
    [SerializeField] private int _currentAmountToBuyLandPlot = 1;

    private int _currentCoinAmount = 0;

    private void Start() {
        _currentCoinAmount = _startingGold;
        UpdateText();
    }

    public void UpdateCurrentCoinAmount(int amount) {
        _currentCoinAmount += amount;
        UpdateText();
    }

    public bool BuyLandPlots(int amountOfPlotsToBuy) {
        if (_currentCoinAmount >= _currentAmountToBuyLandPlot * amountOfPlotsToBuy) {
            UpdateCurrentCoinAmount(-(_currentAmountToBuyLandPlot * amountOfPlotsToBuy));
            return true;
        } else {
            Debug.Log("Not enough gold!");
            return false;
        }
    }

    public void IncreaseBuyLandPlotAmount() {
        _currentAmountToBuyLandPlot += 1;
        UpdateText();
    }

    private void UpdateText() {
        _coinText.text = _currentCoinAmount.ToString("D3");
        _amountToBuyLandPlotText.text = _currentAmountToBuyLandPlot.ToString();
    }
}
