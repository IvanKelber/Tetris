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
    float downDelay;
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
        downDelay = tickSpeed * repeatPercentage;
        timeUntilTick = tickSpeed;

        InitializeBoard();
        PlaceShape(shapeSpawner.GetNextShape()); //initial shape
    }

    void Update()
    {
        // timeUntilTick -= Time.deltaTime;
        // if (timeUntilTick <= 0)
        // {
        //     //This is a tick
        //     Vector2Int oldPosition = boardPosition;
        //     isSet = HandleGravity();

        //     timeUntilTick = tickSpeed;
        // }
        // if (Input.GetKeyDown(KeyCode.D))
        // {
        //     HandleCollision(new Vector2Int(1, 0));
        // }
        // else if (Input.GetKeyDown(KeyCode.A))
        // {
        //     HandleCollision(new Vector2Int(-1, 0));
        // }
        // if (Input.GetKeyDown(KeyCode.S))
        // {
        //     isSet = HandleGravity();
        // }
        // if (Input.GetKey(KeyCode.S))
        // {
        //     if (timeUntilDown <= 0)
        //     {
        //         isSet = HandleGravity();
        //         timeUntilDown = downDelay;
        //     }
        //     else
        //     {
        //         timeUntilDown -= Time.deltaTime;
        //     }
        // }
        // else
        // {
        //     timeUntilDown = downDelay;
        // }
        // if (Input.GetKeyDown(KeyCode.W))
        // {
        //     RotateRight();
        // }
        // if (isSet)
        // {
        //     isSet = true;
        //     bool cleared = ClearCompletedRows();
        //     if (cleared)
        //     {
        //         FillDownwards();
        //     }
        //     PlaceShape(shapeSpawner.GetNextShape());
        // }
    }

    public void PlaceShape(Shape shape)
    {
        shape.board = this;
        Vector2Int maxDimensions = shape.GetMaxDimensions();
        if (maxDimensions.x >= 0 && maxDimensions.x < boardCols && maxDimensions.y >= 0 && maxDimensions.y < boardRows)
        {
            shape.currentBoardIndex = new Vector2Int(maxDimensions.x, boardRows - maxDimensions.y);
        }
        currentShape = shape;

        shape.isSet = false;
    }

    void InitializeBoard()
    {
        board = new bool[boardRows][];

        for (int i = 0; i < boardRows; i++)
        {
            board[i] = new bool[boardCols];
            for (int j = 0; j < boardCols; j++)
            {
                Vector2 position = GetPositionFromIndex(i, j);
                board[i][j] = false; // to be created
                // board[i][j] = (Tile)Instantiate(tilePrefab, position, Quaternion.identity).GetComponent<Tile>();
                // board[i][j].SetSize(tileSize);
                // board[i][j].SetBoardIndex(i, j);
                // board[i][j].Render();
            }
        }
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

    private void OnDrawGizmos()
    {
        // for (int row = 0; row < boardRows; row++)
        // {
        //     for (int col = 0; col < boardCols; col++)
        //     {
        //         Gizmos.DrawCube(GetPositionFromIndex(row, col), new Vector3(1, 1, -10));
        //     }
        // }
    }

    public Vector2 GetPositionFromIndex(int row, int col)
    {
        return new Vector2(bottomLeft.x + tileSize * col, bottomLeft.y + tileSize * row);
    }


    void RotateRight()
    {
        // if (isSet)
        // {
        //     return; //Do nothing if the current shape is already set
        // }
        // for (int i = 0; i < currentShape.blocks.Length; i++)
        // {
        //     ClearTile(boardPosition + currentShape.blocks[i]);
        // }

        // // _DebugPositions("old positions: ");
        // bool collisions = false;
        // Vector2Int[] rotated_blocks = currentShape.RotateRight();
        // for (int i = 0; i < rotated_blocks.Length; i++)
        // {
        //     if (TileFilled(boardPosition + rotated_blocks[i]))
        //     {
        //         collisions = true;
        //         break;
        //     }
        // }
        // if (!collisions)
        // {
        //     currentShape.blocks = rotated_blocks;
        // }
        // for (int i = 0; i < currentShape.blocks.Length; i++)
        // {
        //     FillTile(boardPosition + currentShape.blocks[i]);
        // }
        // // _DebugPositions("new positions: ");
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
