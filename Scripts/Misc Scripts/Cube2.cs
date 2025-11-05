using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube2 : MonoBehaviour
{
    public Mesh mesh;
    public MeshFilter Filter;

    // Start is called before the first frame update
    void Start()
    {
        faceGenerate();
        /*
        Mesh front_mesh = new Mesh();
        Mesh right_mesh = new Mesh();
        Mesh back_mesh = new Mesh();
        Mesh left_mesh = new Mesh();
        Mesh top_mesh = new Mesh();
        Mesh bottom_mesh = new Mesh();

        //vertices
        Vector3[] front_vertices = new Vector3[4];
        Vector3[] right_vertices = new Vector3[4];
        Vector3[] back_vertices = new Vector3[4];
        Vector3[] left_vertices = new Vector3[4];
        Vector3[] top_vertices = new Vector3[4];
        Vector3[] bottom_vertices = new Vector3[4];

        //uv
        Vector2[] uv = new Vector2[4];
        //tringles
        int[] front_triangles = new int[6];
        int[] right_triangles = new int[6];
        int[] back_triangles = new int[6];
        int[] left_triangles = new int[6];
        int[] top_triangles = new int[6];
        int[] bottom_triangles = new int[6];

        //defining vertices
        front_vertices[0] = new Vector3(0, 0, 0);
        front_vertices[1] = new Vector3(0, 1, 0);
        front_vertices[2] = new Vector3(1, 1, 0);
        front_vertices[3] = new Vector3(1, 0, 0);

        uv[0] = new Vector2(0, 0);
        
        {
            front_triangles[0] = 0;
            front_triangles[1] = 1;
            front_triangles[2] = 2;

            front_triangles[3] = 0;
            front_triangles[4] = 2;
            front_triangles[5] = 3;
        }

        right_vertices[0] = new Vector3(1, 0, 0);
        right_vertices[1] = new Vector3(1, 1, 0);
        right_vertices[2] = new Vector3(1, 1, 1);
        right_vertices[3] = new Vector3(1, 0, 1);

        {
            right_triangles[0] = 0;
            right_triangles[1] = 1;
            right_triangles[2] = 2;

            right_triangles[3] = 0;
            right_triangles[4] = 2;
            right_triangles[5] = 3;
        }

        front_mesh.vertices = front_vertices;
        right_mesh.vertices = right_vertices;
        back_mesh.vertices = back_vertices;
        left_mesh.vertices = left_vertices;
        top_mesh.vertices = top_vertices;
        bottom_mesh.vertices = bottom_vertices;


        front_mesh.uv = right_mesh.uv = back_mesh.uv = left_mesh.uv = top_mesh.uv = bottom_mesh.uv = uv;  

        front_mesh.triangles = front_triangles;
        right_mesh.triangles = right_triangles;
        back_mesh.triangles = back_triangles;
        left_mesh.triangles = left_triangles;
        top_mesh.triangles = top_triangles;
        bottom_mesh.triangles = bottom_triangles;

        GetComponent<MeshFilter>().mesh = front_mesh;
        GetComponent<MeshFilter>().mesh = right_mesh;
        GetComponent<MeshFilter>().mesh = back_mesh;
        GetComponent<MeshFilter>().mesh = left_mesh;
        GetComponent<MeshFilter>().mesh = top_mesh;
        GetComponent<MeshFilter>().mesh = bottom_mesh;
        */
        //mesh.RecalculateBounds();
        //mesh.RecalculateNormals();
        //mesh.RecalculateTangents();

    }

    void faceGenerate()
    {
        Mesh front_mesh = new Mesh();
        Vector3[] front_vertices = new Vector3[4];
        Vector2[] uv = new Vector2[4];
        int[] front_triangles = new int[6];
    }

}