using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class ModelarEmbalagem : MonoBehaviour
{
    void Start()
    {
        int n = 32;
        int rings = 9;

        Vector3[] v = new Vector3[(n + 1) * rings];
        Vector2[] uv = new Vector2[v.Length];
        int[] tris = new int[n * (rings - 1) * 6];

        for (int j = 0; j < rings; j++)
        {
            float t = (float)j / (rings - 1);
            float x = Mathf.Lerp(-0.21f, 0.21f, t);

            float factor = 1f;

            if (t < 0.18f)
                factor = Mathf.Lerp(0.35f, 1f, t / 0.18f);
            else if (t > 0.82f)
                factor = Mathf.Lerp(1f, 0.35f, (t - 0.82f) / 0.18f);

            for (int i = 0; i <= n; i++)
            {
                float a = i * Mathf.PI * 2f / n;
                float wave = (t < 0.18f || t > 0.82f)
                    ? Mathf.Sin(a * 8f) * 0.004f : 0f;

                int index = j * (n + 1) + i;

                v[index] = new Vector3(
                    x,
                    Mathf.Cos(a) * 0.12f * factor + wave,
                    Mathf.Sin(a) * 0.065f * factor + wave
                );

                uv[index] = new Vector2(t, (float)i / n);
            }
        }

        int k = 0;

        for (int j = 0; j < rings - 1; j++)
        {
            for (int i = 0; i < n; i++)
            {
                int a = j * (n + 1) + i;
                int b = a + n + 1;

                tris[k++] = a;
                tris[k++] = a + 1;
                tris[k++] = b;

                tris[k++] = a + 1;
                tris[k++] = b + 1;
                tris[k++] = b;
            }
        }

        Mesh mesh = new Mesh();
        mesh.vertices = v;
        mesh.uv = uv;
        mesh.triangles = tris;
        mesh.RecalculateNormals();

        GetComponent<MeshFilter>().mesh = mesh;
    }
}