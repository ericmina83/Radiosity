using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatchToPatchData
{
    public Vector3 dis;
    public Vector3 dir;
    public float length;
    Patch patch1;
    Patch patch2;
    public float f12;
    public float f21;
    public float fCommon;
    public bool visible = false;

    public PatchToPatchData(Patch patch1, Patch patch2)
    {
        this.patch1 = patch1;
        this.patch2 = patch2;

        dis = patch2.center - patch1.center;
        length = dis.magnitude;
        dir = dis.normalized;

        float dot1 = Vector3.Dot(dir, patch1.normal);

        if (dot1 < 0)
            return;

        float dot2 = Vector3.Dot(-dir, patch2.normal);

        if (dot2 < 0)
            return;

        fCommon = dot1 * dot2 / (Mathf.PI * length * length) * patch1.area * patch2.area;
        f12 = fCommon / patch1.area;
        f21 = fCommon / patch2.area;


        RaycastHit hit;

        if (Physics.Raycast(patch1.center, dir, out hit))
        {
            if (hit.transform == patch2.face.transform)
            {
                visible = true;
            }
        }
    }
}

public class Patch
{
    public int id;
    public FaceGenerator face;
    public float area;
    private Vector3 localCenter;
    public Vector3 center
    {
        get
        {
            return face.transform.TransformPoint(localCenter);
        }
    }
    public Vector3 normal
    {
        get
        {
            return face.normal;
        }
    }

    public Vertex lt;
    public Vertex lb;
    public Vertex rt;
    public Vertex rb;
    public Color color;
    public Color newColor;

    Dictionary<Patch, PatchToPatchData> p2pDatas = new Dictionary<Patch, PatchToPatchData>();

    public Patch(int id, FaceGenerator face, float area, Vertex lt, Vertex lb, Vertex rt, Vertex rb)
    {
        this.id = id;
        this.face = face;
        this.area = area;
        localCenter = (lt.position + lb.position + rb.position + rt.position) / 4;
        this.lt = lt;
        this.lb = lb;
        this.rt = rt;
        this.rb = rb;
        lt.patches.Add(this);
        lb.patches.Add(this);
        rt.patches.Add(this);
        rb.patches.Add(this);

        color = newColor = face.GetSelfLightInstance();
    }

    public void ApplyPatchToPatch(Patch anotherPatch)
    {
        if (anotherPatch == this)
            return;

        if (!p2pDatas.ContainsKey(anotherPatch))
            return;

        PatchToPatchData data = p2pDatas[anotherPatch];

        if (!data.visible)
            return;

        newColor += face.faceColor * anotherPatch.color * data.f12 * face.reflectFactor;
    }

    public void CheckVisiblePatchToPatch(Patch anotherPatch)
    {
        if (anotherPatch == this)
            return;

        if (!p2pDatas.ContainsKey(anotherPatch))
            p2pDatas.Add(anotherPatch, new PatchToPatchData(this, anotherPatch));
    }

    public void ApplyPatchColor()
    {
        color = newColor;
        newColor = face.GetSelfLightInstance();
    }

    public void ApplyVertexColor()
    {
        lb.ApplyColor();
        lt.ApplyColor();
        rb.ApplyColor();
        rt.ApplyColor();
    }
}