using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shape
{

    public Vector2Int[] blocks;
    public Color color;
    public Board board;

    public Shape(Vector2Int[] blocks, Color color)
    {
        this.blocks = blocks;
        this.color = color;
    }

    void HandleGravity()
    {

    }

    // void HandleCollision(Vector2Int directionVector)
    // {
    //     // handles collisions in the direction of this vector
    //     bool collision = false;
    //     foreach (Vector2Int block in boardPosition)
    //     {
    //         if (board.TileFilled(block + directionVector))
    //         {
    //             collision = true;
    //             break;
    //         }
    //     }
    //     if (!collision)
    //     {
    //         for (int i = 0; i < boardPosition.Length; i++)
    //         {
    //             boardPosition[i] += directionVector;
    //         }
    //     }
    // }
    public Vector2Int GetMaxDimensions()
    {
        int minX = 0;
        int maxX = 0;
        int minY = 0;
        int maxY = 0;
        foreach (Vector2Int block in blocks)
        {
            Debug.Log("block: " + block);
            maxX = (int)Mathf.Max(maxX, block.x);
            minX = (int)Mathf.Min(minX, block.x);
            maxY = (int)Mathf.Max(maxY, block.y);
            minY = (int)Mathf.Min(minY, block.y);
        }
        return new Vector2Int(maxX - minX, maxY - minY);
    }

    public void SetBoard(Board board)
    {
        this.board = board;
    }
}
