using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Shape : MonoBehaviour
{

    public List<Vector2Int> blocks; //this is a list because the blocks can be removed by lines.
    public Color color;
    [HideInInspector]
    public bool isSet;

    [HideInInspector]
    public Vector2Int currentBoardIndex;
    [HideInInspector]
    public bool onStandby = true;
    Board board; // The board that the piece is on.
    float blockSize;
    float timeUntilTick;
    float timeUntilDown;
    float downDelay;
    const float edgeWidth = .005f;
    MeshRenderer meshRenderer;
    MeshFilter meshFilter;

    private void Start()
    {
        meshRenderer = gameObject.AddComponent<MeshRenderer>();
        meshFilter = gameObject.AddComponent<MeshFilter>();
        meshRenderer.sortingOrder = 10;

        meshRenderer.sharedMaterial = new Material(Shader.Find("Standard"));
        meshRenderer.material.SetColor("_Color", color);

        meshFilter.mesh = new Mesh();
    }

    private void Update()
    {
        Render();

        if (onStandby)
        {
            return;
        }
        downDelay = board.tickSpeed * board.repeatPercentage;

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
    }

    public void SetBoard(Board board)
    {
        this.board = board;
        blockSize = board.tileSize;
    }

    private void HandleGravity()
    {
        if (isSet)
        {
            return;
        }
        bool collisions = DetectCollision(new Vector2Int(0, -1));
        MoveShape(collisions, true);
    }

    public void MoveShape(bool collisions, bool downwards)
    {
        // if we have collided with something while moving down then we are "set" and we should fill the tilemap
        // and check for completed rows
        if (collisions && downwards)
        {
            List<int> completedRows = new List<int>();
            isSet = true;
            for (int i = 0; i < blocks.Count; i++)
            {
                board.Fill(currentBoardIndex + blocks[i], this, blocks[i]);
                if (++board.fillCounts[currentBoardIndex.y + blocks[i].y] == board.boardCols)
                {
                    completedRows.Add(currentBoardIndex.y + blocks[i].y);
                };
            }
            completedRows.Sort();
            board.HandleCompletedRows(completedRows);
        }
        else if (!collisions)
        {
            //as long as we didn't get any collisions we can move.
            transform.position = board.GetPositionFromIndex(currentBoardIndex.y, currentBoardIndex.x);
        }
    }

    public Vector2Int MoveBlock(Vector2Int block, int shift)
    {
        int blockIndex = blocks.IndexOf(block);
        if (blockIndex >= 0)
        {
            blocks[blockIndex] = new Vector2Int(block.x, block.y - shift);
            return blocks[blockIndex];
        }
        Debug.LogError("Block does not exist in this shape");
        return new Vector2Int(-1, -1);
    }

    public void MoveToBoardIndex(int row, int col)
    {
        transform.position = board.GetPositionFromIndex(row, col);
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

    private void Render()
    {
        Mesh mesh = GetComponent<MeshFilter>().mesh;
        mesh.Clear();
        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();
        for (int i = 0; i < blocks.Count; i++)
        {
            vertices.AddRange(GetVertices(blocks[i]));
            triangles.AddRange(GetTriangles(i));
        }
        string s = "";
        for (int i = 0; i < vertices.Count; i++)
        {
            s += "" + vertices[i];
        }
        Debug.Log("vertices: " + s);
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.RecalculateNormals();
    }

    private Vector3[] GetVertices(Vector2Int blockIndex)
    {
        Vector2 block = new Vector2(blockIndex.x * blockSize, blockIndex.y * blockSize);
        Vector3[] vertices = new Vector3[4]
        {
                new Vector3(block.x, block.y, -5),
                new Vector3(block.x + blockSize-edgeWidth, block.y, -5),
                new Vector3(block.x, block.y + blockSize-edgeWidth, -5),
                new Vector3(block.x + blockSize-edgeWidth, block.y + blockSize-edgeWidth, -5)
        };
        return vertices;
    }

    private int[] GetTriangles(int block)
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

    public void Remove(int row, int col)
    {
        //row, col are the board index of a block that this shape needs to remove
        Vector2Int boardIndex = new Vector2Int(col, row);
        boardIndex -= currentBoardIndex;
        blocks.Remove(boardIndex);
        if (blocks.Count == 0)
        {
            Destroy(gameObject);
        }
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
