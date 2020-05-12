using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Board : MonoBehaviour
{
    public float tileSize = 0.2f; //Tiles are squares
    public int boardCols = 10;
    public int boardRows = 20;
    public float tickSpeed = 1; //Every one second our shapes go down.
    public float repeatPercentage = .1f;
    public int basePointsPerRow = 100;
    public int nextBenchmarkLineCount = 5;
    int pointTotal = 0;
    int lineCount = 0;

    private BlockInfo[][] board; //2d array referencing the shapes/blocks that occupy the tile

    [HideInInspector]
    public int[] fillCounts;
    Shape currentShape;

    public ShapeSpawner shapeSpawner;
    private Vector2 bottomLeft;
    void Start()
    {
        bottomLeft = new Vector2(transform.position.x - boardCols * tileSize / 2,
                                                      transform.position.y - boardRows * tileSize / 2);
        fillCounts = new int[boardRows];
        InitializeBoard();
        shapeSpawner.SetBoard(this);
        PlaceShape(shapeSpawner.GetNextShape()); //initial shape

    }

    void Update()
    {
        if (currentShape.isSet)
        {
            PlaceShape(shapeSpawner.GetNextShape());
        }
    }


    public int CalculateScore(int rowsCompleted)
    {
        return (int)(Mathf.Pow(2, rowsCompleted - 1) * basePointsPerRow / tickSpeed);
    }

    public void IncreaseTickSpeed()
    {
        if (lineCount > nextBenchmarkLineCount)
        {
            tickSpeed -= 0.1f;
            nextBenchmarkLineCount *= 2;
        }
    }

    public void PlaceShape(Shape shape)
    {
        currentShape = shape;
        currentShape.SetBoard(this);
        Vector2Int maxDimensions = currentShape.GetMaxDimensions();
        if (maxDimensions.x >= 0 && maxDimensions.x < boardCols && maxDimensions.y >= 0 && maxDimensions.y < boardRows)
        {
            currentShape.currentBoardIndex = new Vector2Int(maxDimensions.x, boardRows - maxDimensions.y - 1);
        }
        currentShape.isSet = false;
        currentShape.onStandby = false;
        if (currentShape.DetectCollision(new Vector2Int(0, 0)))
        {
            Debug.LogError("Final Score: " + pointTotal);
            GameStateManager.Menu();
        }
    }

    void InitializeBoard()
    {
        board = new BlockInfo[boardRows][];
        for (int i = 0; i < boardRows; i++)
        {
            board[i] = new BlockInfo[boardCols];
            for (int j = 0; j < boardCols; j++)
            {
                board[i][j] = new BlockInfo(null, new Vector2Int(-1, -1));
            }
        }
        RenderBoard();
    }

    void RenderBoard()
    {
        Debug.Log("rendering board with bottomLeft: " + bottomLeft + " and size: " + tileSize);
        MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
        MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
        meshRenderer.sortingOrder = 5;

        meshRenderer.sharedMaterial = new Material(Shader.Find("Standard"));
        Mesh mesh = new Mesh();


        Vector3[] vertices = new Vector3[4]
        {
            new Vector3(bottomLeft.x, bottomLeft.y, 0), //bottom left
            new Vector3(bottomLeft.x + boardCols * tileSize, bottomLeft.y, 0), //bottom right
            new Vector3(bottomLeft.x, bottomLeft.y + boardRows * tileSize, 0), //top left
            new Vector3(bottomLeft.x + boardCols * tileSize, bottomLeft.y + boardRows * tileSize, 0) //top right
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

    public void HandleCompletedRows(List<int> completedRows)
    {
        if (completedRows.Count > 0)
        {
            pointTotal += CalculateScore(completedRows.Count);
            foreach (int row in completedRows)
            {
                fillCounts[row] = 0;
                for (int col = 0; col < boardCols; col++)
                {
                    board[row][col].shape.Remove(row, col);
                    board[row][col].Clear();
                }
            }
            int shift = 1;
            foreach (int row in completedRows)
            {
                int nextRow = row + 1;

                while (nextRow < boardRows && fillCounts[nextRow] > 0)
                {
                    fillCounts[nextRow - shift] = fillCounts[nextRow];
                    for (int col = 0; col < boardCols; col++)
                    {
                        board[nextRow - shift][col].Clear();
                        BlockInfo blockInfo = board[nextRow][col];
                        if (!blockInfo.IsNull())
                        {
                            fillCounts[nextRow]--;
                            BlockInfo newBlockInfo = new BlockInfo(blockInfo.shape,
                                                    blockInfo.shape.MoveBlock(blockInfo.block, shift));
                            board[nextRow - shift][col] = newBlockInfo;
                        }
                        board[nextRow][col].Clear();
                    }
                    nextRow++;
                }
                shift++;
            }
            lineCount += completedRows.Count;
            Messenger<int>.Broadcast(GameEvent.ROW_COMPLETED, lineCount);
            Messenger<int>.Broadcast(GameEvent.SCORE_UPDATED, pointTotal);
            IncreaseTickSpeed();
        }
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
            if (fillCounts.Length > 0)
            {
                Vector2 rowPosition = GetPositionFromIndex(i, 0);
                Handles.Label(new Vector2(rowPosition.x - .5f, rowPosition.y + tileSize / 2), "" + fillCounts[i]);
            }
        }
    }

    public struct BlockInfo
    {
        public Shape shape;
        public Vector2Int block;

        public BlockInfo(Shape shape, Vector2Int block)
        {
            this.shape = shape;
            this.block = block;
        }

        public bool IsNull()
        {
            return shape == null && block.x == -1;
        }

        public void Clear()
        {
            shape = null;
            block = new Vector2Int(-1, -1);
        }
    }


    public Vector2 GetPositionFromIndex(int row, int col)
    {
        return new Vector2(bottomLeft.x + tileSize * col, bottomLeft.y + tileSize * row);
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
            return !board[row][col].IsNull();
        }
        return true;
    }
    public void Fill(Vector2Int boardPosition, Shape shape, Vector2Int block)
    {
        Fill(boardPosition.y, boardPosition.x, new BlockInfo(shape, block));
    }
    public void Fill(int row, int col, BlockInfo blockInfo)
    {
        if (col >= 0 && col < boardCols && row >= 0 && row < boardRows)
        {
            board[row][col] = blockInfo;
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
            board[row][col].Clear();
        }
    }
}
