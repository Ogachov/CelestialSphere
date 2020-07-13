using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class CelestialSphere : MonoBehaviour
{
    private Vector3[] _vertices;
    private int[] _triangles;
    private Color[] _colors;
    private Vector2[] _uvs;

    private Renderer _renderer;
    private static readonly int CamUp = Shader.PropertyToID("_CamUp");
    private static readonly int Center = Shader.PropertyToID("_Center");
    

    void Start()
    {
        var keys = Stars.Infos.Keys;
        var numStar = keys.Count;
        _vertices = new Vector3[numStar * 4];
        _triangles = new int[numStar * 6];
        _colors = new Color[numStar * 4];
        _uvs = new Vector2[numStar * 4];

        var i = 0;
        foreach (var key in keys)
        {
            var info = Stars.Infos[key];
            var pos = info.CalcPosition();
            _vertices[i * 4 + 0] = pos;
            _vertices[i * 4 + 1] = pos;
            _vertices[i * 4 + 2] = pos;
            _vertices[i * 4 + 3] = pos;

            _triangles[i * 6 + 0] = i * 4 + 0;
            _triangles[i * 6 + 1] = i * 4 + 1;
            _triangles[i * 6 + 2] = i * 4 + 2;
            _triangles[i * 6 + 3] = i * 4 + 2;
            _triangles[i * 6 + 4] = i * 4 + 1;
            _triangles[i * 6 + 5] = i * 4 + 3;

            var col = info.Color;
            col.a = (8.0f - info.Magnitude) / 7.0f;    // 等級の差を表現する
            _colors[i * 4 + 0] = col;
            _colors[i * 4 + 1] = col;
            _colors[i * 4 + 2] = col;
            _colors[i * 4 + 3] = col;

            _uvs[i * 4 + 0] = new Vector2(0f, 0f);
            _uvs[i * 4 + 1] = new Vector2(1f, 0f);
            _uvs[i * 4 + 2] = new Vector2(0f, 1f);
            _uvs[i * 4 + 3] = new Vector2(1f, 1f);
            i++;
        }
        
        var mesh = new Mesh();
        mesh.name = "CelestialSphere";
        mesh.vertices = _vertices;
        mesh.triangles = _triangles;
        mesh.colors = _colors;
        mesh.uv = _uvs;
        mesh.bounds = new Bounds(Vector3.zero, Vector3.one * 100000.0f);
        var meshFilter = GetComponent<MeshFilter>();
        meshFilter.sharedMesh = mesh;

        _renderer = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void LateUpdate()
    {
        if (Camera.main != null)
        {
            _renderer.material.SetVector(CamUp, Camera.main.transform.up);
            _renderer.material.SetVector(Center, Camera.main.transform.position);
        }
    }
}
