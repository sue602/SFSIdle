using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour
{
    public float OrbitRadius = 2f;
    public Vector2 ScaleRange;
    private SpaceObject star;

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        float scaleValue = Random.Range(ScaleRange.x, ScaleRange.y);
        transform.localScale = new Vector3(scaleValue, scaleValue, scaleValue);

        star = transform.parent.GetComponent<Star>();

        var angle = Random.Range(0, 2 * Mathf.PI);
        var x = Mathf.Cos(angle) * OrbitRadius;
        var z = Mathf.Sin(angle) * OrbitRadius;

        Vector3 randomPoint = new Vector3(x, transform.position.y, z) + star.transform.position;
        transform.position = randomPoint;
    }

    private void GenerateHeight()
    {
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        Mesh mesh = meshFilter.mesh;
        var vertices = mesh.vertices;
        var normals = mesh.normals;
        var triangles = mesh.triangles;

        for (int i = 0; i < vertices.Length; i++)
        {
            Vector3 vertice = transform.TransformPoint(vertices[i]);
            Vector3 center = transform.position;

            Vector3 difference = vertice - center;

            float sizeScale = Random.Range(-0.05f,0.15f);

            Vector3 result = difference.normalized * sizeScale;

            vertices[i] = vertices[i] + result;

        }

        mesh.SetVertices(vertices);
        mesh.SetNormals(vertices);

        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
    }
}
