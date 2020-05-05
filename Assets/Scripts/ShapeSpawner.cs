using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeSpawner : MonoBehaviour
{


    public GameObject[] shapePrefabs;
    GameObject loadedShapePrefab;

    public void Awake()
    {
        loadedShapePrefab = GetRandomShapePrefab();
    }

    public Shape GetNextShape()
    {
        Shape currentShape;
        currentShape = (Shape)Instantiate(loadedShapePrefab, Vector3.zero, Quaternion.identity).GetComponent<Shape>();
        loadedShapePrefab = GetRandomShapePrefab();
        return currentShape;
    }

    private GameObject GetRandomShapePrefab()
    {
        return shapePrefabs[Random.Range(0, shapePrefabs.Length)];
    }


}
