using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class VillagerInteractionController : MonoBehaviour
{
    GameObject _roof;
    public GameObject Roof { get { return _roof; } set { _roof = value; } }
    private MeshFilter _blastGuideMeshFilter;
    private MeshRenderer _blastGuideMeshRenderer;
    private Mesh _blastGuideMesh;

    private const int BlastGuideSegments = 64;

    void Start()
    {
        InitBlastGuide();
        Vector3 center = transform.position;
        center.y += 0.01f;
        DrawBlastGuide(center, 5f);
    }


    private void OnTriggerEnter(Collider other)
    {
        bool player = other.CompareTag("Player");
        bool throwDelivery = other.CompareTag("Bread") && Managers.Player.PlayerStat.Abilities.Contains(Ability.ThrowDelivery);
        if (!player && !throwDelivery)
            return;

        Managers.Deliver.CompleteDelivery(this, other.tag);

        if (throwDelivery)
            Destroy(other);
    }

    private void InitBlastGuide()
    {
        _blastGuideMeshFilter = gameObject.GetorAddComponent<MeshFilter>();
        _blastGuideMeshRenderer = gameObject.GetorAddComponent<MeshRenderer>();
        _blastGuideMeshRenderer.material.color = new Color32(0x6C, 0xDF, 0x33, 0xFF);

        _blastGuideMesh = new Mesh();
        _blastGuideMeshFilter.mesh = _blastGuideMesh;
    }

    private void DrawBlastGuide(Vector3 worldCenter, float radius)
    {
        Vector3[] vertices = new Vector3[BlastGuideSegments + 1];
        int[] triangles = new int[BlastGuideSegments * 3];

        vertices[0] = transform.InverseTransformPoint(worldCenter);

        for (int i = 0; i < BlastGuideSegments; i++)
        {
            float angle = 2f * Mathf.PI * i / BlastGuideSegments;

            Vector3 worldPosition = worldCenter + new Vector3(
                Mathf.Cos(angle) * radius,
                0f,
                Mathf.Sin(angle) * radius
            );
            vertices[i + 1] =
                transform.InverseTransformPoint(worldPosition);
        }

        for (int i = 0; i < BlastGuideSegments; i++)
        {
            int current = i + 1;
            int next = (i + 1) % BlastGuideSegments + 1;

            triangles[i * 3] = 0;
            triangles[i * 3 + 1] = next;
            triangles[i * 3 + 2] = current;
        }

        _blastGuideMesh.Clear();
        _blastGuideMesh.vertices = vertices;
        _blastGuideMesh.triangles = triangles;
        _blastGuideMesh.RecalculateNormals();
        _blastGuideMesh.RecalculateBounds();
    }
}
