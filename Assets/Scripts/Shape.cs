using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shape
{


    private Vector2Int[] default_blocks;
    public Vector2Int[] blocks;
    public Color color;

    public Shape(Vector2Int[] blocks, Color color)
    {
        this.default_blocks = blocks;
        this.blocks = default_blocks;
        this.color = color;
    }

    public Vector2Int GetMaxDimensions()
    {
        int minX = 0;
        int maxX = 0;
        int minY = 0;
        int maxY = 0;
        foreach (Vector2Int block in default_blocks)
        {
            maxX = (int)Mathf.Max(maxX, block.x);
            minX = (int)Mathf.Min(minX, block.x);
            maxY = (int)Mathf.Max(maxY, block.y);
            minY = (int)Mathf.Min(minY, block.y);
        }
        return new Vector2Int(maxX - minX, maxY - minY);
    }

    public void Reset()
    {
        this.blocks = this.default_blocks;
    }

    public Vector2Int[] RotateRight()
    {
        Vector2Int[] rotated_blocks = new Vector2Int[blocks.Length];
        for (int i = 0; i < blocks.Length; i++)
        {
            rotated_blocks[i] = new Vector2Int(blocks[i].y, -blocks[i].x);
        }
        return rotated_blocks;
    }
}
