using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class WaveGenerator : MonoBehaviour
{
    public int numSegment = 16;
    public int numTrack = 9;
    public float minRadius = 1.0f;
    public float maxRadius = 10.0f;
    public float bias = 0.0f;
    public float speed = 0.01f;
    private float offset = 0.0f;

    private Material mat = null;
    private Texture2D tex = null;
    private float[] heights = null;
    private float intensity = 1.0f;

    private int nextIndex = 0;

    // Use this for initialization
    void Start()
    {
        GetComponent<MeshFilter>().sharedMesh = GenerateMesh();

        GenerateHeightmap();
        mat = GetComponent<MeshRenderer>().sharedMaterial;
        mat.SetTexture("_HeightMap", tex);
        mat.SetInt("_NumTrack", numTrack);
        mat.SetFloat("_TrackWidth", (maxRadius - minRadius) / numTrack);

        intensity = mat.GetFloat("_Intensity");

        offset = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        offset = Mathf.Repeat(offset + Time.deltaTime * speed, 1.0f);
        mat.SetFloat("_Offset", offset);
    }

    Mesh GenerateMesh()
    {
        Mesh mesh = new Mesh();

        int numVertices = (numTrack + 1) * (numSegment + 1);
        Vector3[] vertices = new Vector3[numVertices];
        Vector3[] normals = new Vector3[numVertices];
        Vector2[] uv = new Vector2[numVertices];

        float radiusStep = (maxRadius - minRadius) / numTrack;
        float thetaStep = Mathf.PI * 2 / numSegment;

        for (int i = 0, k = 0; i <= numTrack; i++)
        {
            float radius = minRadius + radiusStep * i;

            for (int j = 0; j <= numSegment; j++, k++)
            {
                float theta = j * thetaStep;
                if (j == numSegment) theta = 0.0f;

                float z = radius * Mathf.Cos(theta);
                float x = radius * Mathf.Sin(theta);

                vertices[k] = new Vector3(x, 0.0f, z);
                normals[k] = Vector3.up;
                uv[k] = new Vector2((float)i / numTrack, (float)j / numSegment);
            }
        }

        mesh.vertices = vertices;
        mesh.uv = uv;

        int[] indices = new int[numTrack * numSegment * 6];

        for (int i = 0, k = 0; i < numTrack; i++)
        {
            for (int j = 0; j < numSegment; j++, k += 6)
            {
                //int a = i * numSegment + j;
                //int b = i * numSegment + (j + 1) % numSegment;
                //int c = (i + 1) * numSegment + j;
                //int d = (i + 1) * numSegment + (j + 1) % numSegment;

                int a = i * (numSegment + 1) + j;
                int b = a + 1;
                int c = (i + 1) * (numSegment + 1) + j;
                int d = c + 1;

                indices[k] = a;
                indices[k + 1] = c;
                indices[k + 2] = b;
                indices[k + 3] = c;
                indices[k + 4] = d;
                indices[k + 5] = b;
            }
        }

        mesh.triangles = indices;
        return mesh;
    }

    Texture2D GenerateHeightmap()
    {
        tex = new Texture2D(128, 1, TextureFormat.RGBA32, false);
        heights = new float[128];

        for (int i = 0; i < 128; i++)
        {
            float height = Mathf.Sin(i) * 0.2f;
            tex.SetPixel(i, 0, new Color(height, 0, 0, 0));
            heights[i] = height;
        }
        tex.Apply();

        nextIndex = 127;

        return tex;
    }

    void AppendHeight(float height)
    {
        
    }

    public float SampleHeight(Vector3 position)
    {
        Vector3 pos = transform.InverseTransformPoint(position);
        pos.y = 0;
        float r = (pos.magnitude - minRadius) / (maxRadius - minRadius);

        float u = r * 0.5f - offset + bias;
        if (u < 0.0f) u += 1.0f;
        u *= 127;

        int u_0 = Mathf.FloorToInt(u);
        int u_1 = Mathf.CeilToInt(u);

        float t = u - u_0;
        u_0 = Mathf.Clamp(u_0, 0, 127);
        u_1 = Mathf.Clamp(u_1, 0, 127);

        return (heights[u_0] * (1 - t) + heights[u_1] * t) * intensity;
    }

    public float SampleHeight(float x, float z)
    {
        return SampleHeight(new Vector3(x, 0, z));
    }

}
