using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    private float size;
    private bool filled = false;
    private Vector2Int boardIndex;

    public Tile(int row, int col)
    {
        boardIndex = new Vector2Int(row, col);
    }


    public Vector2Int GetBoardIndex()
    {
        return boardIndex;
    }

    public bool IsFilled()
    {
        return filled;
    }

    public void Fill()
    {
        filled = true;
    }

    public void Clear()
    {
        filled = false;
    }
}
