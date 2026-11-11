using UnityEngine;

[ExecuteAlways]
[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class CandyWrapper : MonoBehaviour
{
    [Header("Dimensões")]
    public float length = 1f;          
    public float height = 0.5f;        
    public float thickness = 0.18f;    
    [Range(0.05f, 0.4f)] public float crimpRatio = 0.12f; 
    [Range(0f, 0.4f)] public float neckPinch = 0.12f;     

    [Header("Pontas serrilhadas")]
    public int teeth = 14;
    public float toothDepth = 0.025f;
    public int pleats = 10;            
    public float pleatAmplitude = 0.008f;

    [Header("Resolução")]
    [Range(8, 128)] public int segmentsX = 64;
    [Range(8, 64)] public int segmentsY = 32;

    [Header("Física")]
    public bool addBoxCollider = true;

    Mesh mesh;
    bool dirty = true;

    void OnEnable() => dirty = true;
    void OnValidate() => dirty = true;

    void Update()
    {
        if (!dirty) return;
        Build();
        dirty = false;
    }

    void Build()
    {
        if (mesh == null) mesh = new Mesh { name = "CandyWrapper" };
        mesh.Clear();

        int vx = segmentsX + 1, vy = segmentsY + 1;
        int perSide = vx * vy;
        var verts = new Vector3[perSide * 2];
        var uvs = new Vector2[perSide * 2];

        float halfL = length * 0.5f, halfH = height * 0.5f;
        float crimp = length * crimpRatio;
        float bodyHalf = halfL - crimp;
        float blend = Mathf.Max(0.0001f, crimp * 0.6f);
        const float eps = 0.0005f;

        for (int j = 0; j < vy; j++)
        {
            float v = j / (float)segmentsY;
            float baseY = Mathf.Lerp(-halfH, halfH, v);
            float ny = baseY / halfH;
            float puffY = Mathf.Sqrt(Mathf.Max(0f, 1f - ny * ny));

            for (int i = 0; i < vx; i++)
            {
                float u = i / (float)segmentsX;
                float x = Mathf.Lerp(-halfL, halfL, u);
                float ax = Mathf.Abs(x);

                
                float t = Mathf.InverseLerp(bodyHalf - blend, bodyHalf, ax);
                float puffX = 1f - Mathf.SmoothStep(0f, 1f, t);

               
                float c = Mathf.InverseLerp(bodyHalf - blend * 0.5f, bodyHalf, ax);
                float pleat = pleatAmplitude * Mathf.Sin(v * pleats * Mathf.PI * 2f) * c;

                
                float d = (ax - bodyHalf) / (blend * 0.5f);
                float y = baseY * (1f - neckPinch * Mathf.Exp(-d * d));

                
                if (i == 0 || i == segmentsX)
                {
                    float tri = Mathf.PingPong(v * teeth * 2f, 1f);
                    x = Mathf.Sign(x) * (halfL - toothDepth * tri);
                }

                float puff = thickness * 0.5f * puffY * puffX;
                int idx = j * vx + i;

                verts[idx] = new Vector3(x, y, -(puff + eps) + pleat);           
                verts[idx + perSide] = new Vector3(x, y, (puff + eps) + pleat);  

                uvs[idx] = new Vector2(u, v);
                uvs[idx + perSide] = new Vector2(1f - u, v);
            }
        }

        int quads = segmentsX * segmentsY;
        var front = new int[quads * 6];
        var back = new int[quads * 6];
        int k = 0;
        for (int j = 0; j < segmentsY; j++)
        {
            for (int i = 0; i < segmentsX; i++)
            {
                int a = j * vx + i, b = a + 1, cc = a + vx, dd = cc + 1;

                front[k] = a; front[k + 1] = cc; front[k + 2] = dd;
                front[k + 3] = a; front[k + 4] = dd; front[k + 5] = b;

                int ab = a + perSide, bb = b + perSide, cb = cc + perSide, db = dd + perSide;
                back[k] = ab; back[k + 1] = db; back[k + 2] = cb;
                back[k + 3] = ab; back[k + 4] = bb; back[k + 5] = db;

                k += 6;
            }
        }

        mesh.vertices = verts;
        mesh.uv = uvs;
        mesh.subMeshCount = 2;
        mesh.SetTriangles(front, 0);
        mesh.SetTriangles(back, 1);
        mesh.RecalculateNormals();
        mesh.RecalculateTangents();
        mesh.RecalculateBounds();

        GetComponent<MeshFilter>().sharedMesh = mesh;

        if (addBoxCollider)
        {
            var box = GetComponent<BoxCollider>();
            if (box == null) box = gameObject.AddComponent<BoxCollider>();
            box.center = Vector3.zero;
            box.size = new Vector3(length, height, thickness);
        }
    }
}
