using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter))]
public class FaceGenerator : MonoBehaviour
{
    public Mesh mesh;
    int[] triangles;
    Vertex[] vertices;
    public Patch[] patches;
    public Color[] colors;
    bool outside = true;
    int size = 10;
    float range = 1.0f;
    public Color faceColor;
    float lightInstance;
    int verticesCount;
    int patchesCount;
    public float reflectFactor;

    public Vector3 normal
    {
        get
        {
            return transform.forward * (outside ? -1 : 1);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<MeshCollider>().sharedMesh = mesh = GetComponent<MeshFilter>().mesh = new Mesh();
        CreateMesh();
    }

    private void CreateMesh()
    {
        float patchWidth = range / size;
        float patchArea = patchWidth * patchWidth;

        for (int h = 0; h <= size; h++)
        {
            for (int v = 0; v <= size; v++)
            {
                var vertexId = v * (size + 1) + h;
                var vertexPos
                    = new Vector3(1, 1, 0) * -(range / 2.0f)
                    + patchWidth * new Vector3(h, v, 0);

                vertices[vertexId] = new Vertex(this, vertexId, vertexPos);
                colors[vertexId] = vertices[vertexId].color;

                if (v == 0 || h == 0)
                    continue;

                int patch_v = (v - 1);
                int patch_h = (h - 1);
                var patchId = patch_v * size + patch_h;

                var lt = vertices[(v - 1) * (size + 1) + (h - 1)];
                var lb = vertices[(v) * (size + 1) + (h - 1)];
                var rb = vertices[(v) * (size + 1) + (h)]; // vertexId
                var rt = vertices[(v - 1) * (size + 1) + (h)];

                if (!outside)
                {
                    var tmp = lb;
                    lb = rt;
                    rt = tmp;
                }

                triangles[patchId * 6 + 0] = lt.id;
                triangles[patchId * 6 + 1] = lb.id;
                triangles[patchId * 6 + 2] = rt.id;
                triangles[patchId * 6 + 3] = lb.id;
                triangles[patchId * 6 + 4] = rb.id;
                triangles[patchId * 6 + 5] = rt.id;

                patches[patch_v * size + patch_h] = new Patch(patchId, this, patchArea, lt, lb, rt, rb);


            }
        }

        mesh.Clear();

        mesh.vertices = vertices.Select(vertex => vertex.position).ToArray();
        mesh.triangles = triangles;
        mesh.colors = colors;

        mesh.RecalculateNormals();
        mesh.RecalculateTangents();
    }

    public FaceGenerator Setting(bool outside, Quaternion localRotation, Vector3 localPosition, float range, Color color, float lightInstance, float reflectFactor)
    {
        this.reflectFactor = reflectFactor;
        transform.localPosition = localPosition;
        transform.localRotation = localRotation;
        this.outside = outside;
        this.faceColor = color;
        this.lightInstance = lightInstance;
        this.range = range;

        verticesCount = (size + 1) * (size + 1);
        patchesCount = size * size;
        vertices = new Vertex[verticesCount];
        patches = new Patch[patchesCount];
        triangles = new int[patchesCount * 2 * 3];
        colors = new Color[verticesCount];

        return this;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        foreach (var patch in patches)
        {
            Gizmos.DrawLine(patch.center, patch.center + patch.normal);
        }
    }

    public void ApplyFaceToFace(FaceGenerator anotherFace)
    {
        if (this == anotherFace)
            return;

        foreach (var patch in patches)
            foreach (var anotherPatch in anotherFace.patches)
                patch.ApplyPatchToPatch(anotherPatch);
    }

    public void CheckVisibleFaceToFace(FaceGenerator anotherFace)
    {
        if (this == anotherFace)
            return;

        foreach (var patch in patches)
            foreach (var anotherPatch in anotherFace.patches)
                patch.CheckVisiblePatchToPatch(anotherPatch);
    }

    public void ApplyPatchColor()
    {
        foreach (var patch in patches)
            patch.ApplyPatchColor();
    }

    public void ApplyVertexColor()
    {
        foreach (var patch in patches)
            patch.ApplyVertexColor();

        mesh.colors = colors;
    }

    public Color GetSelfLightInstance()
    {
        return faceColor * lightInstance;
    }
}
