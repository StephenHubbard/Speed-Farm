using UnityEngine;
using UnityEditor;

public class RandomPlacerEditor : EditorWindow
{
    [SerializeField]
    private GameObject objectToPlace;
    [SerializeField]
    private int count = 1;
    [SerializeField]
    private Vector3 minPosition = Vector3.zero;
    [SerializeField]
    private Vector3 maxPosition = new Vector3(10, 10, 10);

    [MenuItem("Tools/Random Placer")]
    public static void ShowWindow()
    {
        GetWindow<RandomPlacerEditor>("Random Placer");
    }

    private void OnGUI()
    {
        objectToPlace = (GameObject)EditorGUILayout.ObjectField("Object to Place", objectToPlace, typeof(GameObject), false);
        count = EditorGUILayout.IntField("Count", count);
        minPosition = EditorGUILayout.Vector3Field("Min Position", minPosition);
        maxPosition = EditorGUILayout.Vector3Field("Max Position", maxPosition);

        if (GUILayout.Button("Place Objects"))
        {
            PlaceObjectsRandomly();
        }
    }

    private void PlaceObjectsRandomly()
    {
        if (objectToPlace == null)
        {
            Debug.LogError("No object assigned to place.");
            return;
        }

        for (int i = 0; i < count; i++)
        {
            Vector3 randomPosition = new Vector3(
                Random.Range(minPosition.x, maxPosition.x),
                Random.Range(minPosition.y, maxPosition.y),
                Random.Range(minPosition.z, maxPosition.z)
            );

            GameObject newObject = (GameObject)PrefabUtility.InstantiatePrefab(objectToPlace);
            newObject.transform.position = randomPosition;
            Undo.RegisterCreatedObjectUndo(newObject, "Placed object");
        }
    }
}
