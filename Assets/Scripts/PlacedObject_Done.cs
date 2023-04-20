using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacedObject_Done : MonoBehaviour
{
    public PlacedObjectTypeSO PlacedObjectTypeSO { get; private set; }
    public Vector2Int Origin { get; private set; }

    private PlacedObjectTypeSO.Dir _dir;

    public static PlacedObject_Done Create(Vector3 worldPosition, Vector2Int origin, PlacedObjectTypeSO.Dir dir, PlacedObjectTypeSO placedObjectTypeSO)
    {
        Transform placedObjectTransform = Instantiate(placedObjectTypeSO.Prefab, worldPosition, Quaternion.Euler(0, placedObjectTypeSO.GetRotationAngle(dir), 0));

        PlacedObject_Done placedObject = placedObjectTransform.GetComponent<PlacedObject_Done>();
        placedObject.Setup(placedObjectTypeSO, origin, dir);

        return placedObject;
    }

    private void Setup(PlacedObjectTypeSO placedObjectTypeSO, Vector2Int origin, PlacedObjectTypeSO.Dir dir)
    {
        this.PlacedObjectTypeSO = placedObjectTypeSO;
        this.Origin = origin;
        this._dir = dir;
    }

    public List<Vector2Int> GetGridPositionList()
    {
        return PlacedObjectTypeSO.GetGridPositionList(Origin, _dir);
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }

    public override string ToString()
    {
        return PlacedObjectTypeSO.NameString;
    }

}
