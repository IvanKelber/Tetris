using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeSpawner : MonoBehaviour
{


    public GameObject[] shapePrefabs;
    Shape loadedShape;
    Board board;

    public void Awake()
    {
        loadedShape = (Shape)Instantiate(GetRandomShapePrefab(), transform.position, Quaternion.identity).GetComponent<Shape>();
    }

    public void SetBoard(Board board)
    {
        this.board = board;
        loadedShape.SetBoard(board);
    }

    private void Update()
    {
        if (board != null && loadedShape != null)
        {
            SetBoard(board);
        }
    }

    public Shape GetNextShape()
    {
        Shape currentShape = loadedShape;
        loadedShape = (Shape)Instantiate(GetRandomShapePrefab(), transform.position, Quaternion.identity).GetComponent<Shape>();
        return currentShape;
    }

    private GameObject GetRandomShapePrefab()
    {
        return shapePrefabs[Random.Range(0, shapePrefabs.Length)];
    }


}
