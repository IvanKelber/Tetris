using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    private float size;
    private bool filled = false;
    public Color defaultColor = Color.red;
    private Color color;
    private Vector2Int boardIndex;
    const float edgeWidth = .005f;
    private void Awake()
    {
        color = defaultColor;
    }

    public Vector2 GetCenter()
    {
        return new Vector2(transform.position.x + size / 2, transform.position.y + size / 2);
    }

    public void Render()
    {
        //render based on size
        MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
        MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();

        meshRenderer.sharedMaterial = new Material(Shader.Find("Standard"));
        meshRenderer.material.SetColor("_Color", defaultColor);
        Mesh mesh = new Mesh();

        Vector3[] vertices = new Vector3[4]
        {
            new Vector3(0, 0, 0),
            new Vector3(size, 0, 0),
            new Vector3(0, size, 0),
            new Vector3(size, size, 0)
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

    public void Update()
    {
        Renderer renderer = GetComponent<Renderer>();
        renderer.material.SetColor("_Color", color);
    }

    public void SetSize(float tileSize)
    {
        size = tileSize - edgeWidth;
    }

    public void SetBoardIndex(int x, int y)
    {
        boardIndex = new Vector2Int(x, y);
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

    public Color GetColor()
    {
        return color;
    }
}
