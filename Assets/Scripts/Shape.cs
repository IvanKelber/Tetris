﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Shape : MonoBehaviour
{

    public List<Vector2Int> blocks; //this is a list because the blocks can be removed by lines.
    public List<Mesh> blockMeshes;
    public Color color;
    public Board board; // The board that the piece is on.
    public bool isSet;

    public Vector2Int currentBoardIndex;
    public float blockSize = 1;
    float timeUntilTick;
    float timeUntilDown;
    float downDelay;
    MeshRenderer meshRenderer;
    MeshFilter meshFilter;
    private void Start()
    {
        meshRenderer = gameObject.AddComponent<MeshRenderer>();
        meshFilter = gameObject.AddComponent<MeshFilter>();

        meshRenderer.sharedMaterial = new Material(Shader.Find("Standard"));
        meshRenderer.material.SetColor("_Color", color);

        meshFilter.mesh = new Mesh();


        downDelay = board.tickSpeed * board.repeatPercentage;
    }

    private void Update()
    {
        Render();

        timeUntilTick -= Time.deltaTime;
        if (timeUntilTick <= 0)
        {
            //This is a tick
            HandleGravity();
            timeUntilTick = board.tickSpeed;
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            bool collisions = DetectCollision(new Vector2Int(1, 0));
            MoveShape(collisions, false);
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            bool collisions = DetectCollision(new Vector2Int(-1, 0));
            MoveShape(collisions, false);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            HandleGravity();
        }
        if (Input.GetKey(KeyCode.S))
        {
            if (timeUntilDown <= 0)
            {
                HandleGravity();
                timeUntilDown = downDelay;
            }
            else
            {
                timeUntilDown -= Time.deltaTime;
            }
        }
        else
        {
            timeUntilDown = downDelay;
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            RotateRight();
        }
        // if (isSet)
        // {
        //     bool cleared = ClearCompletedRows();
        //     if (cleared)
        //     {
        //         FillDownwards();
        //     }
        //     PlaceShape(shapeSpawner.GetNextShape());
        // }
    }

    public void HandleGravity()
    {
        // Vector2Int oldPosition = boardPosition;
        bool collisions = DetectCollision(new Vector2Int(0, -1));
        MoveShape(collisions, true);
    }

    public void MoveShape(bool collisions, bool downwards)
    {
        // if we have collided with something while moving down then we are "set" and we should fill the tilemap
        if (collisions && downwards)
        {
            isSet = true;
            for (int i = 0; i < blocks.Count; i++)
            {
                board.Fill(currentBoardIndex + blocks[i]);
            }
        }
        else if (!collisions)
        {
            //as long as we didn't get any collisions we can move.
            transform.position = board.GetPositionFromIndex(currentBoardIndex.y, currentBoardIndex.x);
        }
    }

    public bool DetectCollision(Vector2Int directionVector)
    {
        if (isSet)
        {
            return true; //Do nothing if the current shape is already set
        }
        Vector2Int newBoardIndex = currentBoardIndex + directionVector;
        bool collisions = false;
        for (int i = 0; i < blocks.Count; i++)
        {

            if (board.IsFilled(newBoardIndex + blocks[i]))
            {
                collisions = true;
                break;
            }
        }
        if (!collisions)
        {
            currentBoardIndex = newBoardIndex;
        }
        return collisions;

        // _DebugPositions("new positions: ");
    }

    public void Render()
    {
        Mesh mesh = GetComponent<MeshFilter>().mesh;
        mesh.Clear();
        List<Vector3> verticesList = new List<Vector3>();
        List<int> triangles = new List<int>();
        for (int i = 0; i < blocks.Count; i++)
        {
            verticesList.AddRange(GetVertices(blocks[i]));
            triangles.AddRange(GetTriangles(i));
        }
        Vector3[] vertices = verticesList.ToArray();
        mesh.vertices = vertices;
        mesh.triangles = triangles.ToArray();
        mesh.RecalculateNormals();
    }

    public Vector3[] GetVertices(Vector2Int blockIndex)
    {
        Vector2 block = new Vector2(blockIndex.x * blockSize, blockIndex.y * blockSize);
        Vector3[] vertices = new Vector3[4]
        {
                new Vector3(block.x, block.y, 10),
                new Vector3(block.x + blockSize, block.y, 10),
                new Vector3(block.x, block.y + blockSize, 10),
                new Vector3(block.x + blockSize, block.y + blockSize, 10)
        };
        string s = "";
        foreach (Vector3 v in vertices)
        {
            s += v;
        }
        Debug.Log(s);
        return vertices;
    }

    public int[] GetTriangles(int block)
    {
        int start = 4 * block;
        int[] tris = new int[6]
       {
            // lower left triangle
            start, start + 2, start + 1,
            // upper right triangle
            start + 2, start + 3, start + 1
       };
        return tris;
    }

    public void RenderBlock(Vector2 position)
    {


        Mesh mesh = new Mesh();

        Vector3[] vertices = new Vector3[4]
        {
            new Vector3(position.x, position.y, -10),
            new Vector3(position.x + blockSize, position.y, -10),
            new Vector3(position.x, position.y + blockSize, -10),
            new Vector3(position.x + blockSize, position.y + blockSize, -10)
        };
        mesh.vertices = vertices;

        int[] tris = new int[6]
        {
            // lower left triangle
            0, 2, 1,
            // upper right triangle
            2, 3, 1
        };
        mesh.triangles = tris;

        Vector3[] normals = new Vector3[4]
        {
            -Vector3.forward,
            -Vector3.forward,
            -Vector3.forward,
            -Vector3.forward
        };
        mesh.normals = normals;

        Vector2[] uv = new Vector2[4]
        {
            new Vector2(0, 0),
            new Vector2(1, 0),
            new Vector2(0, 1),
            new Vector2(1, 1)
        };
        mesh.uv = uv;

        meshFilter.mesh = mesh;
    }

    public Vector2Int GetMaxDimensions()
    {
        int minX = 0;
        int maxX = 0;
        int minY = 0;
        int maxY = 0;
        foreach (Vector2Int block in blocks)
        {
            maxX = (int)Mathf.Max(maxX, block.x);
            minX = (int)Mathf.Min(minX, block.x);
            maxY = (int)Mathf.Max(maxY, block.y);
            minY = (int)Mathf.Min(minY, block.y);
        }
        return new Vector2Int(maxX - minX, maxY - minY);
    }


    public void RotateRight()
    {
        if (isSet)
        {
            return; //Do nothing if the current shape is already set
        }


        // // _DebugPositions("old positions: ");
        bool collisions = false;
        List<Vector2Int> rotated_blocks = new List<Vector2Int>(); ;
        for (int i = 0; i < blocks.Count; i++)
        {
            Vector2Int rotated_block = new Vector2Int(blocks[i].y, -blocks[i].x);
            if (board.IsFilled(currentBoardIndex + rotated_block))
            {
                collisions = true;
                break;
            }
            else
            {
                rotated_blocks.Add(rotated_block);
            }
        }
        if (!collisions)
        {
            blocks = rotated_blocks;
        }
        // // _DebugPositions("new positions: ");

    }

    private void OnDrawGizmos()
    {

        // Debug.Log(color);
        // Debug.Log(transform.position);

        // for (int row = 0; row < board.boardRows; row++)
        // {
        //     for (int col = 0; col < board.boardCols; col++)
        //     {
        //         Vector2 pos = board.GetPositionFromIndex(row, col);
        //         if (row + col == 0)
        //         {
        //             Debug.Log("bottomLeft position from gizmos" + pos);
        //         }
        //         Gizmos.DrawCube(pos, new Vector3(blockSize, blockSize, -10));
        //     }
        // }
        // Gizmos.color = color;
        // for (int i = 0; i < blocks.Count; i++)
        // {
        //     Vector2 position = board.GetPositionFromIndex(currentBoardIndex.y + blocks[i].y, currentBoardIndex.x + blocks[i].x);
        //     Gizmos.DrawCube(new Vector3(position.x + blockSize / 2, position.y + blockSize / 2), new Vector3(blockSize, blockSize, 10));
        // }
        // Handles.Label(transform.position, "HERE");
        // Gizmos.color = Color.red;
        // Gizmos.DrawWireCube(tile.GetCenter(), new Vector2(tileSize, tileSize));

    }

}
