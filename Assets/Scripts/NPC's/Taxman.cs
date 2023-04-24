using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Taxman : MonoBehaviour, INPC
{
    [SerializeField] private int taxPerPlotRate = 1;

    public void Collect()
    {
        int amountOfTaxesOwed = LandManager.Instance.AmountOfPlotsOwned * taxPerPlotRate;

        EconomyManager.Instance.UpdateCurrentCoinAmount(-amountOfTaxesOwed);
    }
}
