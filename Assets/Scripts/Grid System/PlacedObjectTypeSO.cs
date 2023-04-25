using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class PlacedObjectTypeSO : ScriptableObject
{
    public Transform Prefab;
    public ItemSO ItemSO;
    public int Width;
    public int Height;
    public Sprite[] SpriteLifeCycle;

    public List<Vector2Int> GetGridPositionList(Vector2Int origin)
    {
        List<Vector2Int> gridPositionList = new List<Vector2Int>();
       
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                gridPositionList.Add(origin + new Vector2Int(x, y));
            }
        }
           
        return gridPositionList;
    }

}
