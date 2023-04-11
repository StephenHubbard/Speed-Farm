using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EconomyManager : Singleton<EconomyManager>
{
    [SerializeField] private TMP_Text coinText;

    private int currentCoinAmount = 0;

    public void UpdateCurrentCoinAmount(int amount) {
        currentCoinAmount += amount;
        UpdateCoinText();
    }

    private void UpdateCoinText() {
        coinText.text = currentCoinAmount.ToString("D3");
    }
}
