using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PackManager : Singleton<PackManager>
{
    [SerializeField] private GameObject _endOfDayContainer;
    [SerializeField] private Transform _resourceToCollectContainer;
    [SerializeField] private GameObject _resourcePrefab;
    [SerializeField] private TMP_Text _dayText;

    private int _currentDay = 0;

    private void Start() {
        _currentDay++;
        UpdateDayText();
    }

    public void OpenEndOfDayContainer() {
        _endOfDayContainer.SetActive(true);
    }

    public void NewDayButton() {
        CloseEndOfDayContainer();
        Time.timeScale = 1;
    }

    public void CloseEndOfDayContainer()
    {
        _endOfDayContainer.SetActive(false);
    }
    
    // currently button
    public void EndDay() {
        Time.timeScale = 0;
        OpenEndOfDayContainer();
        Debug.Log("end day");
    }

    public void AddResourceToCollect(ItemSO itemSO) {
        foreach (Transform resourceToCollectTransform in _resourceToCollectContainer)
        {
            ResourceToCollect resourceToCollect = resourceToCollectTransform.GetComponent<ResourceToCollect>();
            ItemSO potentialItemSO = resourceToCollect._itemSO;

            if (potentialItemSO == itemSO) {
                resourceToCollect.IncreaseAmountToCollectByOne();
                return;
            }
        }

        GameObject newResourceToCollect = Instantiate(_resourcePrefab, _resourceToCollectContainer);
        newResourceToCollect.GetComponent<Image>().sprite = itemSO.resourceSprite;
        newResourceToCollect.GetComponent<ResourceToCollect>().IncreaseAmountToCollectByOne();
        newResourceToCollect.GetComponent<ResourceToCollect>().SetItemSO(itemSO);
    }

    private void UpdateDayText() {
        _dayText.text = "Day:" + " " + _currentDay.ToString();
    }
}
