using System.Collections.Generic;
using UnityEngine;

public class Vertex
{
    public int id;
    public Vector3 position;
    public FaceGenerator face;

    public List<Patch> patches = new List<Patch>();
    public Color color;

    public Vertex(FaceGenerator face, int id, Vector3 position)
    {
        this.face = face;
        this.id = id;
        this.position = position;
        this.color = Color.black;
    }

    public void ApplyColor()
    {
        int patchCount = patches.Count;

        if (patchCount == 0)
            return;

        face.colors[id] = Color.black;

        foreach (var patch in patches)
            face.colors[id] += patch.color / patchCount;
    }
}