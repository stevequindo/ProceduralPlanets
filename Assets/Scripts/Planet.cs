using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour {

    [Range(2,256)]
    public int resolution = 50;

    [SerializeField]
    MeshFilter[] meshFilters;
    TerrainFace[] terrainFaces;

    TerrainFace currentFace;

    // Editor-only function that Unity calls when the script is loaded or a value changes in the Inspector.
    // You would usually use this to perform an action after a value changes in the Inspector; for example, making sure that data stays within a certain range.

    void OnValidate()
	{
        Initialize();
        GenerateMesh();
    }

    void Initialize()
    {
        if (meshFilters == null || meshFilters.Length == 0)
        {
            meshFilters = new MeshFilter[6];
        }
        terrainFaces = new TerrainFace[6];

        Vector3[] directions = { Vector3.up, Vector3.down, Vector3.left, Vector3.right, Vector3.forward, Vector3.back };

        for (int i = 0; i < 6; i++)
        {
            if (meshFilters[i] == null)
            {
                GameObject meshObj = new GameObject("mesh");
                meshObj.transform.parent = transform;

                // Mesh renderer and mesh filter work together within a game object.
                meshObj.AddComponent<MeshRenderer>().sharedMaterial = new Material(Shader.Find("Standard"));
                meshFilters[i] = meshObj.AddComponent<MeshFilter>();
                meshFilters[i].sharedMesh = new Mesh();
            }

            Debug.Log(resolution);

            terrainFaces[i] = new TerrainFace(meshFilters[i].sharedMesh, resolution, directions[i]);
        }
    }

    void GenerateMesh()
    {
        foreach (TerrainFace face in terrainFaces)
        {

            face.ConstructMesh();

        }

    }
}