using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyLand : MonoBehaviour, Item
{
    public void UseItem()
    {
        LandManager.Instance.ShowAvailableLandToBuy();
        LandManager.Instance.BuyLandToggleTrue();
        GridGeneration.Instance.DeselectObjectType();
    }
}
