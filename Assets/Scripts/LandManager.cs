using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using CodeMonkey.Utils;
using UnityEngine.EventSystems;


public class LandManager : Singleton<LandManager>
{
    public bool BuyLandToggledOn { get; private set; }

    [SerializeField] private Tilemap grassTilemap;
    [SerializeField] private Tile dirtTile;
    [SerializeField] private GameObject showAvailableLandToBuyPrefab;

    private List<GameObject> allShowLandSprites = new List<GameObject>();

    private void Start() {
        DetermineStartingLandOwnership();
        BuyLandToggleFalse();
    }

    private void Update() {
        BuyLand();
    }

    public void BuyLandToggleTrue() {
        BuyLandToggledOn = true;
    }

    public void BuyLandToggleFalse()
    {
        BuyLandToggledOn = false;
    }

    private void BuyLand() {
        if (EventSystem.current.IsPointerOverGameObject()) { return; }

        if (Input.GetMouseButtonDown(0) && BuyLandToggledOn) {
            var grid = GridGeneration.Instance.GetGrid();

            Vector3 mousePosition = UtilsClass.GetMouseWorldPosition();
            grid.GetGridObject(mousePosition).BuyLand();
        }
    }

    public void ShowAvailableLandToBuy() {
        var grid = GridGeneration.Instance.GetGrid();

        for (int x = 0; x < grid.GetWidth(); x++)
        {
            for (int y = 0; y < grid.GetHeight(); y++)
            {
                Vector3Int tilePosition = new Vector3Int(x, y, 0);

                if (!grid.GetGridObject(tilePosition).DoesOwnLand()) {
                   GameObject newShowLandPrefab = Instantiate(showAvailableLandToBuyPrefab, new Vector2(x, y), Quaternion.identity);
                   allShowLandSprites.Add(newShowLandPrefab);
                }
            }
        }
    }

    public void HideAvailableLandToBuy() {
        if (allShowLandSprites.Count == 0) { return; }

        foreach (GameObject showLandSprite in allShowLandSprites)
        {
            Destroy(showLandSprite);
        }

        BuyLandToggleFalse();
    }

    private void DetermineStartingLandOwnership() {
        var grid = GridGeneration.Instance.GetGrid();

        for (int x = 0; x < grid.GetWidth(); x++)
        {
            for (int y = 0; y < grid.GetHeight(); y++)
            {
                Vector3Int tilePosition = new Vector3Int(x, y, 0);

                if (grassTilemap.GetTile(tilePosition) == dirtTile) {
                    grid.GetGridObject(tilePosition).ownsLand = true;
                }
            }
        }
    }
}
