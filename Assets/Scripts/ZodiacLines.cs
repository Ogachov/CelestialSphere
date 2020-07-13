using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class ZodiacLines : MonoBehaviour
{
    [SerializeField] private float LineWidth = 1.0f;
    [SerializeField] private float LineMargin = 1.0f;
    
    private Vector3[] _vertices;
    private int[] _triangles;
    private Color[] _colors;
    private Vector2[] _uvs;
    
    private Renderer _renderer;
    private static readonly int Center = Shader.PropertyToID("_Center");
    private static readonly int Width = Shader.PropertyToID("_Width");
    private static readonly int Margin = Shader.PropertyToID("_Margin");

    void Start()
    {
        var numLines = Stars.Lines.Length;
        _vertices = new Vector3[numLines * 3];
        _triangles = new int[numLines * 3];
        _colors = new Color[numLines * 3];
        _uvs = new Vector2[numLines * 3];

        for (var i = 0; i < numLines; i++)
        {
            var p0 = Stars.Infos[Stars.Lines[i].HipId0].CalcPosition();
            var p1 = Stars.Infos[Stars.Lines[i].HipId1].CalcPosition();
            _vertices[i * 3 + 0] = p0;
            _vertices[i * 3 + 1] = p1;
            _vertices[i * 3 + 2] = p1;

            _triangles[i * 3 + 0] = i * 3 + 0;
            _triangles[i * 3 + 1] = i * 3 + 1;
            _triangles[i * 3 + 2] = i * 3 + 2;

            _colors[i * 3 + 0] = Color.red;
            _colors[i * 3 + 1] = Color.red;
            _colors[i * 3 + 2] = Color.red;

            _uvs[i * 3 + 0] = new Vector2(0f, 0f);
            _uvs[i * 3 + 1] = new Vector2(1f, 0f);
            _uvs[i * 3 + 2] = new Vector2(0f, 1f);
        }
        
        var mesh = new Mesh();
        mesh.name = "ZodiacLines";
        mesh.vertices = _vertices;
        mesh.triangles = _triangles;
        mesh.colors = _colors;
        mesh.uv = _uvs;
        mesh.bounds = new Bounds(Vector3.zero, Vector3.one * 100000.0f);
        var meshFilter = GetComponent<MeshFilter>();
        meshFilter.sharedMesh = mesh;

        _renderer = GetComponent<Renderer>();
    }

    private void LateUpdate()
    {
        if (Camera.main != null)
        {
            _renderer.material.SetVector(Center, Camera.main.transform.position);
            _renderer.material.SetFloat(Width, LineWidth);
            _renderer.material.SetFloat(Margin, LineMargin);
        }
    }
}
