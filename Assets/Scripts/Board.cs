using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Board : MonoBehaviour
{
    public float tileSize = 1; //Tiles are squares
    public int boardWidth = 10;
    public int boardHeight = 30;
    public float tickSpeed = 1; //Every one second our shape shapes to go down.
    float timeUntilTick;
    private Tile[,] board;

    private Shape currentShape;
    public Vector2Int[] boardPosition;

    void Start()
    {
        InitializeBoard();

        //Test shape:
        Vector2Int[] blocks = { new Vector2Int(0, 0), new Vector2Int(1, 0), new Vector2Int(1, 1), new Vector2Int(2, 1), new Vector2Int(2, 2) };
        currentShape = new Shape(blocks, Color.red);
        PlaceShape(currentShape);
        timeUntilTick = tickSpeed;
    }

    void Update()
    {
        timeUntilTick -= Time.deltaTime;
        if (timeUntilTick <= 0)
        {
            //This is a tick
            HandleGravity();
            timeUntilTick = tickSpeed;
        }

    }

    public void PlaceShape(Shape shape)
    {
        Vector2Int maxDimensions = shape.GetMaxDimensions();
        boardPosition = new Vector2Int[shape.blocks.Length];
        if (maxDimensions.x >= 0 && maxDimensions.x < boardWidth && maxDimensions.y >= 0 && maxDimensions.y < boardHeight)
        {
            Vector2Int initialShapePosition = new Vector2Int(maxDimensions.x, boardHeight - maxDimensions.y - 1);
            for (int i = 0; i < shape.blocks.Length; i++)
            {
                Vector2Int block = shape.blocks[i];
                int posX = initialShapePosition.x + block.x;
                int posY = initialShapePosition.y + block.y;
                boardPosition[i] = new Vector2Int(posX, posY);
            }
        }
        Debug.Log("INITIAL BOARD POSITION: " + boardPosition);
    }

    void InitializeBoard()
    {
        Vector2 startingPosition = new Vector2(transform.position.x - boardWidth * tileSize / 2,
                                               transform.position.y - boardHeight * tileSize / 2);
        board = new Tile[boardWidth, boardHeight];
        for (int i = 0; i < boardWidth; i++)
        {
            for (int j = 0; j < boardHeight; j++)
            {
                Vector2 position = new Vector2(startingPosition.x + tileSize * i, startingPosition.y + tileSize * j);
                board[i, j] = new Tile(position, tileSize, i, j);
            }
        }
    }


    private void OnDrawGizmos()
    {
        if (board != null)
        {
            foreach (Tile tile in board)
            {
                Gizmos.color = tile.color;
                Gizmos.DrawCube(tile.GetCenter(), new Vector2(tileSize, tileSize));
                Gizmos.color = Color.red;
                Gizmos.DrawWireCube(tile.GetCenter(), new Vector2(tileSize, tileSize));
                Handles.Label(new Vector3(tile.GetCenter().x, tile.GetCenter().y, 10), "" + tile.GetBoardIndex());

            }
        }
    }

    public void HandleGravity()
    {
        HandleCollision(new Vector2Int(0, -1));
    }

    public void HandleCollision(Vector2Int directionVector)
    {
        // handles collisions in the direction of this vector
        Vector2Int[] newBoardPosition = new Vector2Int[boardPosition.Length];
        for (int i = 0; i < boardPosition.Length; i++)
        {
            ClearTile(boardPosition[i]);
            newBoardPosition[i] = boardPosition[i] + directionVector;
        }

        _DebugPositions("old positions: ");
        bool collisions = false;
        for (int i = 0; i < newBoardPosition.Length; i++)
        {
            if (TileFilled(newBoardPosition[i]))
            {
                collisions = true;
                break;
            }
        }
        if (!collisions)
        {
            boardPosition = newBoardPosition;
        }
        for (int i = 0; i < boardPosition.Length; i++)
        {
            FillTile(boardPosition[i]);
        }
        _DebugPositions("new positions: ");
    }

    void _DebugPositions(string header)
    {
        string s = "";
        foreach (Vector2Int v in boardPosition)
        {
            s += v + ", ";
        }
        Debug.Log(header + s);
    }

    //Tile Filling/Clearing Methods
    public bool TileFilled(Vector2Int boardPosition)
    {
        return TileFilled(boardPosition.x, boardPosition.y);
    }
    public bool TileFilled(int x, int y)
    {
        if (x >= 0 && x < boardWidth && y >= 0 && y < boardHeight)
        {
            return board[x, y].IsFilled();
        }
        return true;
    }
    public void FillTile(Vector2Int boardPosition)
    {
        FillTile(boardPosition.x, boardPosition.y);
    }
    public void FillTile(int x, int y)
    {
        if (x >= 0 && x < boardWidth && y >= 0 && y < boardHeight)
        {
            board[x, y].Fill(currentShape.color);
        }
    }
    public void ClearTile(Vector2Int boardPosition)
    {
        ClearTile(boardPosition.x, boardPosition.y);
    }
    public void ClearTile(int x, int y)
    {
        if (x >= 0 && x < boardWidth && y >= 0 && y < boardHeight)
        {
            board[x, y].Clear();
        }
    }
}
