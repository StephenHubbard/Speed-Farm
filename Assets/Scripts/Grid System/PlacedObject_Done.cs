using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacedObject_Done : MonoBehaviour
{
    public PlacedObjectTypeSO PlacedObjectTypeSO { get; private set; }
    public Vector2Int Origin { get; private set; }

    public static PlacedObject_Done Create(Vector3 worldPosition, Vector2Int origin, PlacedObjectTypeSO placedObjectTypeSO)
    {
        Transform placedObjectTransform = Instantiate(placedObjectTypeSO.Prefab, worldPosition, Quaternion.identity);
        PlacedObject_Done placedObject = placedObjectTransform.GetComponent<PlacedObject_Done>();
        placedObject.Setup(placedObjectTypeSO, origin);
        return placedObject;
    }

    private void Setup(PlacedObjectTypeSO placedObjectTypeSO, Vector2Int origin)
    {
        this.PlacedObjectTypeSO = placedObjectTypeSO;
        this.Origin = origin;
    }

    public List<Vector2Int> GetGridPositionList()
    {
        return PlacedObjectTypeSO.GetGridPositionList(Origin);
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}
