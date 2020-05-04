using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeSpawner : MonoBehaviour
{

    public Vector2Int[][] shapeVectors;

    Shape[] shapeManifest;
    Shape loadedShape;

    public void Awake()
    {
        shapeVectors = new Vector2Int[][] {
            new Vector2Int[] {new Vector2Int(-1,0), new Vector2Int(0,0), new Vector2Int(0,1), new Vector2Int(1,1)},
            new Vector2Int[] {new Vector2Int(0,0), new Vector2Int(1,0), new Vector2Int(1,1), new Vector2Int(0,1)},
            new Vector2Int[] {new Vector2Int(-1,-1), new Vector2Int(-1,0), new Vector2Int(0,0), new Vector2Int(1,0)},
            new Vector2Int[] {new Vector2Int(-2,0), new Vector2Int(-1,0), new Vector2Int(0,0), new Vector2Int(1,0)},
            new Vector2Int[] {new Vector2Int(-1,0), new Vector2Int(0,0), new Vector2Int(1,0), new Vector2Int(1,1)},
            new Vector2Int[] {new Vector2Int(-1,1), new Vector2Int(0,1), new Vector2Int(0,0), new Vector2Int(1,0)},
            new Vector2Int[] {new Vector2Int(-1,0), new Vector2Int(0,0), new Vector2Int(0,-1), new Vector2Int(1,0)},

        };

        shapeManifest = new Shape[shapeVectors.Length];

        for (int i = 0; i < shapeVectors.Length; i++)
        {
            shapeManifest[i] = new Shape(shapeVectors[i], new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), 1f));
        }
    }

    public Shape GetNextShape()
    {
        Shape currentShape;
        currentShape = (loadedShape == null) ? GetRandomShape() : loadedShape;
        loadedShape = GetRandomShape();
        return currentShape;
    }

    private Shape GetRandomShape()
    {
        Shape shape = shapeManifest[Random.Range(0, shapeManifest.Length)];
        shape.Reset();
        return shape;
    }

}
