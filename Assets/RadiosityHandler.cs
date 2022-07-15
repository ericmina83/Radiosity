using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadiosityHandler : MonoBehaviour
{
    public CubeGenerator cubePrefab;
    public List<CubeGenerator> cubes = new List<CubeGenerator>();


    // Start is called before the first frame update
    void Start()
    {
        var enviroment = Instantiate(cubePrefab).Setting(false, Quaternion.identity, Vector3.zero, 50.0f, Color.blue, false);
        cubes.Add(enviroment);
        var myLight1 = Instantiate(cubePrefab, enviroment.transform).Setting(true, Quaternion.Euler(30, 50, 70), new Vector3(5, -7, 9), 12.0f, Color.white, true);
        cubes.Add(myLight1);
    }

    // Update is called once per frame
    void Update()
    {
        foreach (var cube1 in cubes)
        {
            if (cube1.isLight)
                continue;

            foreach (var cube2 in cubes)
            {
                if (cube1 == cube2)
                    continue;

                cube1.ApplyCubeToCube(cube2);
            }
        }

        foreach (var cube in cubes)
        {
            cube.ApplyPatchColor();
        }

        foreach (var cube in cubes)
        {
            cube.ApplyVertexColor();
        }
    }
}
