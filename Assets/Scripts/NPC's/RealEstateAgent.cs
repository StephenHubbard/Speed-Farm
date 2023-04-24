using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RealEstateAgent : MonoBehaviour, INPC
{
    public void Collect()
    {
        EconomyManager.Instance.IncreaseBuyLandPlotAmount();
    }

    
}
