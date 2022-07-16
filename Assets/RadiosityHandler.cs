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
        var enviroment = Instantiate(cubePrefab).Setting(false, Quaternion.identity, Vector3.zero, 50.0f, Color.white, 0.0f, 0.5f);
        cubes.Add(enviroment);
        var myLight1 = Instantiate(cubePrefab).Setting(true, Quaternion.Euler(30, 50, 70), new Vector3(5, -7, 9), 12.0f, Color.red, 0.8f, 0.2f);
        cubes.Add(myLight1);
        var myLight2 = Instantiate(cubePrefab).Setting(true, Quaternion.Euler(30, 50, 70), new Vector3(10, 15, 1), 3.0f, Color.blue, 5.0f, 0.2f);
        cubes.Add(myLight2);
    }

    // Update is called once per frame
    public void Algorithm()
    {
        // foreach (var cube1 in cubes)
        // {
        //     foreach (var cube2 in cubes)
        //     {
        //         cube1.CheckRayCubeToCube(cube2);
        //     }
        // }

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
