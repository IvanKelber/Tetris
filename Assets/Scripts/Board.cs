using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Board : MonoBehaviour
{
    public float tileSize = 1; //Tiles are squares
    public int boardWidth = 10;
    public int boardHeight = 30;

    private Tile[,] board;

    // Start is called before the first frame update
    void Start()
    {
        InitializeBoard();

        //Test shape:
        Vector2Int[] blocks = { new Vector2Int(0, 0), new Vector2Int(1, 0), new Vector2Int(1, 1), new Vector2Int(2, 1), new Vector2Int(2, 2) };
        Shape shape = new Shape(blocks, Color.red);
        PlaceShape(shape);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PlaceShape(Shape shape)
    {
        Vector2Int maxDimensions = shape.GetMaxDimensions();
        Debug.Log(maxDimensions);
        if (maxDimensions.x >= 0 && maxDimensions.x < boardWidth && maxDimensions.y >= 0 && maxDimensions.y < boardHeight)
        {
            Vector2Int initialShapePosition = new Vector2Int(maxDimensions.x, boardHeight - maxDimensions.y - 1);
            Debug.Log("initialshape position: " + initialShapePosition);
            foreach (Vector2Int block in shape.blocks)
            {
                board[initialShapePosition.x + block.x, initialShapePosition.y + block.y].Fill(shape.color);
            }
            shape.SetBoard(this);
        }
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
        return false;
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
}
