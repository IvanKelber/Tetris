using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile
{
    private Vector2 position;
    private float size;
    private bool filled = false;
    public Color defaultColor = Color.white;
    public Color color;
    private Vector2Int boardIndex;

    public Tile(Vector2 position, float size, int i, int j)
    {
        this.position = position;
        this.size = size;
        color = defaultColor;
        boardIndex = new Vector2Int(i, j);
    }

    public Vector2 GetCenter()
    {
        return new Vector2(position.x + size / 2, position.y + size / 2);
    }

    public Vector2Int GetBoardIndex()
    {
        return boardIndex;
    }

    public bool IsFilled()
    {
        return filled;
    }

    public void Fill(Color newColor)
    {
        color = newColor;
        filled = true;
    }

    public void Clear()
    {
        color = defaultColor;
        filled = false;
    }
}
