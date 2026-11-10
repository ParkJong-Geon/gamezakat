
using UnityEngine;

public class SceneryScroller : MonoBehaviour
{
    [Header("Pengaturan")]
    public float speed = 3f;
    public float minGap = 2f;
    public float maxGap = 5f;

    private Camera mainCamera;
    private SpriteRenderer[] renderers;

    void Start()
    {
        mainCamera = Camera.main;
        renderers = GetComponentsInChildren<SpriteRenderer>();

        if (mainCamera == null)
            Debug.LogError("Main Camera tidak ditemukan!");

        if (renderers.Length == 0)
            Debug.LogError("SpriteRenderer tidak ditemukan!");
    }

    void Update()
    {
        if (mainCamera == null || renderers.Length == 0)
            return;

        // Gerakkan seluruh pohon ke kiri
        transform.position +=
            Vector3.left * speed * Time.deltaTime;

        float leftEdge = mainCamera.ViewportToWorldPoint(
            new Vector3(0, 0, 0)
        ).x;

        float rightEdge = mainCamera.ViewportToWorldPoint(
            new Vector3(1, 0, 0)
        ).x;

        float maxX = float.MinValue;
        float minX = float.MaxValue;

        foreach (SpriteRenderer sr in renderers)
        {
            if (!sr.enabled) continue;

            maxX = Mathf.Max(maxX, sr.bounds.max.x);
            minX = Mathf.Min(minX, sr.bounds.min.x);
        }

        // Tunggu sampai seluruh bagian pohon keluar layar
        if (maxX < leftEdge)
        {
            float gap = Random.Range(minGap, maxGap);

            // Pindahkan pohon ke kanan layar
            float offset = rightEdge + gap - minX;

            transform.position += Vector3.right * offset;
        }
    }
}
