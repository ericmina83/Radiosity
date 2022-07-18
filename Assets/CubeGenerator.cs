using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeGenerator : MonoBehaviour
{
    public FaceGenerator facePrefab;
    public List<FaceGenerator> faces = new List<FaceGenerator>();
    Color color;
    public FaceGenerator left;
    public FaceGenerator right;
    public FaceGenerator front;
    public FaceGenerator back;
    public FaceGenerator top;
    public FaceGenerator bottom;
    public float reflectFactor;
    bool outside = true;
    float range;
    float lightInstance;

    void Start()
    {
        CreateCube();
    }

    public void CreateCube()
    {
        float halfRange = range / 2;
        faces.Add(front = Instantiate(facePrefab, transform).Setting(outside, Quaternion.Euler(0, 0, 0), new Vector3(0, 0, -halfRange), range, color, lightInstance, reflectFactor));
        faces.Add(left = Instantiate(facePrefab, transform).Setting(outside, Quaternion.Euler(0, 90, 0), new Vector3(-halfRange, 0, 0), range, color, lightInstance, reflectFactor));
        faces.Add(back = Instantiate(facePrefab, transform).Setting(outside, Quaternion.Euler(0, 180, 0), new Vector3(0, 0, halfRange), range, color, lightInstance, reflectFactor));
        faces.Add(right = Instantiate(facePrefab, transform).Setting(outside, Quaternion.Euler(0, -90, 0), new Vector3(halfRange, 0, 0), range, color, lightInstance, reflectFactor));
        faces.Add(top = Instantiate(facePrefab, transform).Setting(outside, Quaternion.Euler(90, 0, 0), new Vector3(0, halfRange, 0), range, color, lightInstance, reflectFactor));
        faces.Add(bottom = Instantiate(facePrefab, transform).Setting(outside, Quaternion.Euler(-90, 0, 0), new Vector3(0, -halfRange, 0), range, color, lightInstance, reflectFactor));
    }

    public CubeGenerator Setting(bool outside, Quaternion localRotation, Vector3 localPosition, float range, Color color, float lightInstance, float reflectFactor)
    {
        this.reflectFactor = reflectFactor;
        this.range = range;
        transform.localPosition = localPosition;
        transform.localRotation = localRotation;
        this.outside = outside;
        this.color = color;
        this.lightInstance = lightInstance;
        return this;
    }

    public void ApplyCubeToCube(CubeGenerator anotherCube)
    {
        foreach (var face in faces)
            foreach (var anotherFace in anotherCube.faces)
                face.ApplyFaceToFace(anotherFace);
    }

    public void CheckVisibleCubeToCube(CubeGenerator anotherCube)
    {
        foreach (var face in faces)
            foreach (var anotherFace in anotherCube.faces)
                face.CheckVisibleFaceToFace(anotherFace);
    }

    public void ApplyPatchColor()
    {
        foreach (var face in faces)
            face.ApplyPatchColor();
    }

    public void ApplyVertexColor()
    {
        foreach (var face in faces)
            face.ApplyVertexColor();
    }
}
