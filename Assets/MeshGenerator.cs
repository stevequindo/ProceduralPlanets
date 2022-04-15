using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(MeshFilter))]
public class MeshGenerator : MonoBehaviour
{
    Mesh mesh;
        
    Vector3[] vertices;
    public int[] triangles;


    public GameObject theOtherMesh;


    // Start is called before the first frame update
    void Start()
    {
        mesh = new Mesh();  

        GetComponent<MeshFilter>().mesh = mesh;

        theOtherMesh.GetComponent<MeshFilter>().mesh = mesh;
    }

    private void Update()
    {
        UpdateMesh();
    }



    void UpdateMesh()
    {
        mesh.Clear();

        vertices = new Vector3[] {
            new Vector3(0,0,0),
            new Vector3(0,0,1),
            new Vector3(1,0,1),
            new Vector3(1,0,0),
        };

        triangles = new int[]
        {
            0,1,3

        };

        mesh.vertices = vertices;
        mesh.triangles = triangles;


    }


}
