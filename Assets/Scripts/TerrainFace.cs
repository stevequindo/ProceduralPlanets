using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainFace
{
    ShapeGenerator shapeGenerator;
    Mesh mesh;
    int resolution;
    Vector3 localUp;
    Vector3 axisA;
    Vector3 axisB;

    int[] triangles;
    Vector3[] vertices;

    public bool running = true;

    public TerrainFace(ShapeGenerator shapeGenerator, Mesh mesh, int resolution, Vector3 localUp)
    {
        this.shapeGenerator = shapeGenerator;
        this.mesh = mesh;
        this.resolution = resolution;
        this.localUp = localUp;

        this.triangles = new int[(resolution - 1) * (resolution - 1) * 2 * 3];
        this.vertices = new Vector3[resolution * resolution];

        // Why do we swap? I assume it gets the perpendicular?
        // Confirm if this is a cardinal direction. YES --> it is. SO swapping it wil give a perpendicular
        // I think this is wrong, I will change it to localRight and localFoward.
        axisA = new Vector3(localUp.z, localUp.x, localUp.y);

        // This will give you the perpendicular to both.
        axisB = Vector3.Cross(localUp, axisA);

        //this.axisA = localRight;
        //this.axisB = localForward
    }

    public void ConstructMesh()
    {

        int triIndex = 0;

        for (int y = 0; y < resolution; y++)
        {
            for (int x = 0; x < resolution; x++)
            {
                // same as putting an i = 0 outside both for loops, then incrementing it inside this inner for loop i++.
                int i = x + y * resolution;

                // This doesn't really need to be a vector2, it can just be two variables x and y.
                Vector2 percent = new Vector2(x, y) / (resolution - 1);

                // percent.x goes from 0 to 1, if you subtract 0.5 it will go from -0.5 to 0.5, and then multiplying by 2 it would go from -1 to 1
                // We needed to go from -1 to 1 because we're going around the origin.
                // You can think of localUp as the straight vectors that out straight from the origin and towards the faces of the cube.
                Vector3 pointOnUnitCube = localUp + (percent.x - .5f) * 2 * axisA + (percent.y - .5f) * 2 * axisB;

                // Normalising a vector means keeping a vectors direction but scaling it down to it's unit vector.
                // This means no matter where we are point from (0,0,0), normalising it will give us a unit of 1, and so equidistant
                // from the center, meaning we get a circle.
                Vector3 pointOnUnitSphere = pointOnUnitCube.normalized;

                vertices[i] = shapeGenerator.CalculatePointOnPlanet(pointOnUnitSphere);

                // If were in the last row or last col, then do it.
                // original x != resolution-1 && y != resolution-1
                if (!(x == resolution - 1 || y == resolution - 1))
                {
                    // First triangle
                    triangles[triIndex] = i;
                    triangles[triIndex + 1] = i + resolution + 1;
                    triangles[triIndex + 2] = i + resolution;

                    // Second stringle
                    triangles[triIndex + 3] = i;
                    triangles[triIndex + 4] = i + 1;
                    triangles[triIndex + 5] = i + resolution + 1;
                    triIndex += 6;
                }
            }

        }

        UpdateMesh();


    }

    public void UpdateMesh()
    {
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;

        // https://docs.unity3d.com/ScriptReference/Mesh.RecalculateNormals.
        mesh.RecalculateNormals();
    }

}
