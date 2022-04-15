using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheOtherMesh : MonoBehaviour
{

    Mesh mesh;

    Vector3[] vertices;
    public int[] triangles;

    [Range(1,20)]
    public int size = 1;

    void Start()
    {
        this.mesh = GetComponent<MeshFilter>().mesh;

    }

    void Update()
    {
        UpdateMesh();
        
    }



    void UpdateMesh()
    {
        mesh.Clear();

        vertices = new Vector3[] {
            new Vector3(0,0,0),
            new Vector3(0,0,1),
            new Vector3(1,0,-1),
            new Vector3(1,0,0),
        };

        triangles = new int[]
        {
            0,1,3,1,2,3
            
        };

        mesh.vertices = vertices;
        mesh.triangles = triangles;

    }

}
