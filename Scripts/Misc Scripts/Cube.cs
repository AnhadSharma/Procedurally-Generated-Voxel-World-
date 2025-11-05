using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Mesh mesh = new Mesh();

        //vertices
        Vector3[] vertices = new Vector3[8];
        //uv
        Vector2[] uv = new Vector2[8];
        //tringles
        int[] triangles = new int[36];

        //defining vertices
        vertices[0] = new Vector3(0, 0, 0);
        vertices[1] = new Vector3(0, 1, 0);
        vertices[2] = new Vector3(1, 1, 0);
        vertices[3] = new Vector3(1, 0, 0);
        vertices[4] = new Vector3(1, 0, 1);
        vertices[5] = new Vector3(1, 1, 1);
        vertices[6] = new Vector3(0, 1, 1);
        vertices[7] = new Vector3(0, 0, 1);

        uv[0] = new Vector2(0, 0);
        //uv[1] = new Vector2(0, 1);
        //uv[2] = new Vector2(1, 1);
        //uv[3] = new Vector2(1, 0);


        //uv[3] = new Vector2(0, 0);
        //uv[2] = new Vector2(0, 1);
        //uv[5] = new Vector2(1, 1);
        //uv[4] = new Vector2(1, 0);

        //uv[4] = new Vector2(0, 0);
        //uv[5] = new Vector2(0, 1);
        //uv[6] = new Vector2(1, 1);
        //uv[7] = new Vector2(1, 0);

        //uv[0] = new Vector2(0, 0);
        //uv[4] = new Vector2(0, 1);
        //uv[7] = new Vector2(1, 1);
        //uv[3] = new Vector2(1, 0);
        //front face: 0123
        //012 and 023
        {
            triangles[0] = 0;
            triangles[1] = 1;
            triangles[2] = 2;

            triangles[3] = 0;
            triangles[4] = 2;
            triangles[5] = 3;
        }

        //right
        // 325 and 354
        {
            triangles[6] = 3;
            triangles[7] = 2;
            triangles[8] = 5;

            triangles[9] = 3;
            triangles[10] = 5;
            triangles[11] = 4;
        }

        //back
        //456 and 467
        {
            triangles[12] = 4;
            triangles[13] = 5;
            triangles[14] = 6;

            triangles[15] = 4;
            triangles[16] = 6;
            triangles[17] = 7;
        }

        //left
        //761 and 710
        {
           triangles[18] = 7;
           triangles[19] = 6;
           triangles[20] = 1;

           triangles[21] = 7;
           triangles[22] = 1;
           triangles[23] = 0;
        }

        //top
        //165 and 152
        {
            triangles[24] = 1;
            triangles[25] = 6;
            triangles[26] = 5;

            triangles[27] = 1;
            triangles[28] = 5;
            triangles[29] = 2;
        }

        //bottom
        //703 and 734
        //{
        //    triangles[30] = 7;
        //    triangles[31] = 0;
        //    triangles[32] = 3;

        //    triangles[33] = 7;
        //    triangles[34] = 3;
        //    triangles[35] = 4;
        //}

        mesh.vertices = vertices;
        mesh.uv = uv;  
        mesh.triangles = triangles;

        GetComponent<MeshFilter>().mesh = mesh;
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
        mesh.RecalculateTangents();
    }
}
