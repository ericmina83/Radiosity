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
        var enviroment = Instantiate(cubePrefab).Setting(false, Quaternion.identity, Vector3.zero, 60.0f, Color.white, 0.0f, 0.5f);
        cubes.Add(enviroment);
        var myLight = Instantiate(cubePrefab).Setting(true, Quaternion.identity, new Vector3(0, 25, 0), 20.0f, Color.white, 5f, 0f);
        cubes.Add(myLight);
        var cubeRed = Instantiate(cubePrefab).Setting(true, Quaternion.Euler(70, -50, -20), new Vector3(-11, -11, -11), 20.0f, Color.red, 0.0f, 1.0f);
        cubes.Add(cubeRed);
        var cubeGreen = Instantiate(cubePrefab).Setting(true, Quaternion.Euler(70, -50, -20), new Vector3(11, -11, 11), 20.0f, Color.green, 0.0f, 1.0f);
        cubes.Add(cubeGreen);
    }

    // Update is called once per frame
    public void Algorithm()
    {
        foreach (var cube1 in cubes)
        {
            foreach (var cube2 in cubes)
            {
                cube1.CheckVisibleCubeToCube(cube2);
            }
        }

        foreach (var cube1 in cubes)
        {
            foreach (var cube2 in cubes)
            {
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
