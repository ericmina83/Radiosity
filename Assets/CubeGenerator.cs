using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeGenerator : MonoBehaviour
{
    public FaceGenerator facePrefab;
    public List<FaceGenerator> faces = new List<FaceGenerator>();
    public Color color;
    public FaceGenerator left;
    public FaceGenerator right;
    public FaceGenerator front;
    public FaceGenerator back;
    public FaceGenerator top;
    public FaceGenerator bottom;
    bool outside = true;
    float range;
    public bool isLight;
    float _lightInstance;
    public float lightInstance
    {
        get
        {
            return _lightInstance;
        }

        set
        {
            _lightInstance = value;

            if (isLight)
                foreach (var face in faces)
                    face.lightInstance = value;
        }
    }

    void Start()
    {
        CreateCube();
    }

    public void CreateCube()
    {
        float halfRange = range / 2;
        faces.Add(front = Instantiate(facePrefab, transform).Setting(outside, Quaternion.Euler(0, 0, 0), new Vector3(0, 0, -halfRange), range, color, isLight));
        faces.Add(left = Instantiate(facePrefab, transform).Setting(outside, Quaternion.Euler(0, 90, 0), new Vector3(-halfRange, 0, 0), range, color, isLight));
        faces.Add(back = Instantiate(facePrefab, transform).Setting(outside, Quaternion.Euler(0, 180, 0), new Vector3(0, 0, halfRange), range, color, isLight));
        faces.Add(right = Instantiate(facePrefab, transform).Setting(outside, Quaternion.Euler(0, -90, 0), new Vector3(halfRange, 0, 0), range, color, isLight));
        faces.Add(top = Instantiate(facePrefab, transform).Setting(outside, Quaternion.Euler(90, 0, 0), new Vector3(0, halfRange, 0), range, color, isLight));
        faces.Add(bottom = Instantiate(facePrefab, transform).Setting(outside, Quaternion.Euler(-90, 0, 0), new Vector3(0, -halfRange, 0), range, color, isLight));
    }

    public CubeGenerator Setting(bool outside, Quaternion localRotation, Vector3 localPosition, float range, Color color, bool idLight)
    {
        this.range = range;
        transform.localPosition = localPosition;
        transform.localRotation = localRotation;
        this.outside = outside;
        this.color = color;
        this.isLight = idLight;
        return this;
    }

    public void ApplyCubeToCube(CubeGenerator anotherCube)
    {
        if (outside && this == anotherCube)
            return;

        foreach (var face in faces)
            foreach (var anotherFace in anotherCube.faces)
                face.ApplyFaceToFace(anotherFace);
    }

    public void ApplyPatchColor()
    {
        if (isLight)
            return;

        foreach (var face in faces)
            face.ApplyPatchColor();
    }

    public void ApplyVertexColor()
    {
        if (isLight)
            return;

        foreach (var face in faces)
            face.ApplyVertexColor();
    }
}
