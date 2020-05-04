using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeSpawner : MonoBehaviour
{

    public Vector2Int[][] shapeVectors;
    public Color[] colorOptions;
    Shape[] shapeManifest;
    Shape loadedShape;

    public void Awake()
    {
        shapeVectors = new Vector2Int[][] {
            new Vector2Int[] {new Vector2Int(-1,0), new Vector2Int(0,0), new Vector2Int(0,1), new Vector2Int(1,1)}, // zig
            new Vector2Int[] {new Vector2Int(0,0), new Vector2Int(1,0), new Vector2Int(1,1), new Vector2Int(0,1)}, // square
            new Vector2Int[] {new Vector2Int(-1,-1), new Vector2Int(-1,0), new Vector2Int(0,0), new Vector2Int(1,0)}, // L
            new Vector2Int[] {new Vector2Int(-2,0), new Vector2Int(-1,0), new Vector2Int(0,0), new Vector2Int(1,0)}, //line
            new Vector2Int[] {new Vector2Int(-1,0), new Vector2Int(0,0), new Vector2Int(1,0), new Vector2Int(1,-1)}, // reverse L
            new Vector2Int[] {new Vector2Int(-1,1), new Vector2Int(0,1), new Vector2Int(0,0), new Vector2Int(1,0)}, // zag
            new Vector2Int[] {new Vector2Int(-1,0), new Vector2Int(0,0), new Vector2Int(0,-1), new Vector2Int(1,0)}, //steps

        };

        shapeManifest = new Shape[shapeVectors.Length];

        for (int i = 0; i < shapeVectors.Length; i++)
        {
            shapeManifest[i] = new Shape(shapeVectors[i], colorOptions[i]);
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
