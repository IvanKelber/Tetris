using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Board : MonoBehaviour
{
    public float tileSize = 1; //Tiles are squares
    public int boardCols = 10;
    public int boardRows = 30;
    public float tickSpeed = 1; //Every one second our shape shapes to go down.
    public float repeatPercentage = .1f;
    float timeUntilDown;
    public GameObject tilePrefab;
    float timeUntilTick;
    private bool[][] board;

    Shape currentShape;
    Vector2Int boardPosition;

    public ShapeSpawner shapeSpawner;
    private Vector2 bottomLeft;

    bool isSet = false;
    void Start()
    {
        bottomLeft = new Vector2(transform.position.x - boardCols * tileSize / 2,
                                                      transform.position.y - boardRows * tileSize / 2);

        InitializeBoard();
        PlaceShape(shapeSpawner.GetNextShape()); //initial shape
    }

    void Update()
    {

        if (currentShape.isSet)
        {
            // bool cleared = ClearCompletedRows();
            // if (cleared)
            // {
            //     FillDownwards();
            // }
            PlaceShape(shapeSpawner.GetNextShape());
        }
    }

    public void PlaceShape(Shape shape)
    {
        currentShape = shape;
        currentShape.board = this;
        Vector2Int maxDimensions = currentShape.GetMaxDimensions();
        if (maxDimensions.x >= 0 && maxDimensions.x < boardCols && maxDimensions.y >= 0 && maxDimensions.y < boardRows)
        {
            currentShape.currentBoardIndex = new Vector2Int(maxDimensions.x, boardRows - maxDimensions.y);
        }

        currentShape.isSet = false;
    }

    void InitializeBoard()
    {
        board = new bool[boardRows][];
        for (int i = 0; i < boardRows; i++)
        {
            board[i] = new bool[boardCols]; // bool defaults to false
        }
        RenderBoard();
    }

    void RenderBoard()
    {
        Debug.Log("rendering board with bottomLeft: " + bottomLeft + " and size: " + tileSize);
        MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
        MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();

        meshRenderer.sharedMaterial = new Material(Shader.Find("Standard"));
        Mesh mesh = new Mesh();


        Vector3[] vertices = new Vector3[4]
        {
            new Vector3(bottomLeft.x, bottomLeft.y, 0), //bottom left
            new Vector3(bottomLeft.x + boardCols * tileSize, bottomLeft.y, 0), //bottom right
            new Vector3(bottomLeft.x, bottomLeft.y + boardRows * tileSize, 0), //top left
            new Vector3(bottomLeft.x + boardCols * tileSize, bottomLeft.y + boardRows * tileSize, 0) //top right
        };

        // //Remove this adjustment when we are actually rendering with quads instead of gizmos
        // Vector3 adjustmentVector = new Vector3(-tileSize / 2, -tileSize / 2);
        // for (int i = 0; i < vertices.Length; i++)
        // {
        //     vertices[i] += adjustmentVector;
        // }

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

    bool ClearCompletedRows()
    {
        // HashSet<int> rowsToCheck = new HashSet<int>();

        // bool cleared = false;
        // foreach (Vector2Int block in currentShape.blocks)
        // {
        //     rowsToCheck.Add((boardPosition + block).y);
        // }
        // foreach (int row in rowsToCheck)
        // {
        //     Debug.Log("checking row : " + row);
        //     bool complete = true;
        //     for (int col = 0; col < boardCols; col++)
        //     {
        //         if (!TileFilled(row, col))
        //         {
        //             Debug.Log("Row " + row + ", col " + col + " is unfilled");
        //             complete = false;
        //             break;
        //         }
        //     }
        //     if (complete)
        //     {
        //         cleared = true;
        //         for (int col = 0; col < boardCols; col++)
        //         {
        //             ClearTile(row, col);
        //         }
        //     }
        // }
        // return cleared;
        return false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        for (int i = 0; i < boardRows; i++)
        {
            for (int j = 0; j < boardCols; j++)
            {
                if (board != null && IsFilled(i, j))
                {
                    Vector2 position = GetPositionFromIndex(i, j);
                    Gizmos.DrawWireCube(new Vector3(position.x + tileSize / 2, position.y + tileSize / 2, 0), new Vector3(tileSize, tileSize, 0));
                }
            }
        }
    }

    void FillDownwards()
    {
        // for (int row = 0; row < boardRows; row++)
        // {
        //     for (int col = 0; col < boardCols; col++)
        //     {
        //         if (TileFilled(row, col))
        //         {
        //             Color fillColor = board[row][col].GetColor();
        //             ClearTile(row, col);
        //             int endRow = row;
        //             while (endRow > 0 && !TileFilled(--endRow, col)) ;
        //             Debug.Log("(" + row + ", " + col + ") is falling to (" + endRow + ", " + col + ")");
        //             FillTile(endRow, col, fillColor);

        //         }
        //     }
        // }
    }

    public Vector2 GetPositionFromIndex(int row, int col)
    {
        return new Vector2(bottomLeft.x + tileSize * col, bottomLeft.y + tileSize * row);
    }


    void _DebugPositions(string header)
    {
        Debug.Log(header + boardPosition);
    }

    //Tile Filling/Clearing Methods
    public bool IsFilled(Vector2Int boardPosition)
    {
        return IsFilled(boardPosition.y, boardPosition.x);
    }
    public bool IsFilled(int row, int col)
    {
        if (col >= 0 && col < boardCols && row >= 0 && row < boardRows)
        {
            return board[row][col];
        }
        return true;
    }
    public void Fill(Vector2Int boardPosition)
    {
        Fill(boardPosition.y, boardPosition.x, currentShape.color);
    }
    public void Fill(int row, int col, Color color)
    {
        if (col >= 0 && col < boardCols && row >= 0 && row < boardRows)
        {
            board[row][col] = true;
        }
    }
    public void Clear(Vector2Int boardPosition)
    {
        Clear(boardPosition.y, boardPosition.x);
    }
    public void Clear(int row, int col)
    {
        if (col >= 0 && col < boardCols && row >= 0 && row < boardRows)
        {
            board[row][col] = false;
        }
    }
}
