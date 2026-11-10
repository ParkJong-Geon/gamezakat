using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoopingScroller : MonoBehaviour
{
[Header("Pengaturan")]
    public Transform[] segments;
    public float speed = 5f;
    public float gap = 0f;

    public Camera mainCamera;
    private float leftEdge;

    void Start()
    {
        Debug.Log("LoopingScroller: START BERJALAN");

        mainCamera = Camera.main;

        if (mainCamera != null)
        {
            leftEdge = mainCamera.ViewportToWorldPoint(
                new Vector3(0, 0, 0)
            ).x;

            Debug.Log("Main Camera ditemukan: " + mainCamera.name);
        }
        else
        {
            Debug.LogError("Main Camera TIDAK ditemukan!");
        }

        Debug.Log("Jumlah Segments: " + segments.Length);

        foreach (Transform segment in segments)
        {
            if (segment == null)
            {
                Debug.LogError("Ada Segment yang kosong!");
                continue;
            }

            SpriteRenderer sr = segment.GetComponent<SpriteRenderer>();

            if (sr == null)
            {
                Debug.LogError(
                    segment.name + " TIDAK memiliki SpriteRenderer!"
                );
            }
            else
            {
                Debug.Log(segment.name + " memiliki SpriteRenderer.");
            }
        }
    }

    void Update()
    {

    if (mainCamera == null || segments.Length < 2)
    {
        Debug.LogWarning("Update dihentikan: Camera kosong atau Segments kurang dari 2");
        return;
    }

        if (mainCamera == null || segments.Length < 2)
            return;

        foreach (Transform segment in segments)
        {
            segment.Translate(
                Vector3.left * speed * Time.deltaTime
            );
        }

        foreach (Transform segment in segments)
        {
            SpriteRenderer sr =
                segment.GetComponent<SpriteRenderer>();

            if (sr == null)
                continue;

            // Jika seluruh sprite sudah keluar layar kiri
            if (sr.bounds.max.x < leftEdge)
            {
                float rightmost = float.MinValue;

                foreach (Transform other in segments)
                {
                    if (other == segment)
                        continue;

                    SpriteRenderer otherSR =
                        other.GetComponent<SpriteRenderer>();

                    if (otherSR != null)
                    {
                        rightmost = Mathf.Max(
                            rightmost,
                            otherSR.bounds.max.x
                        );
                    }
                }

                // Pindahkan ke sebelah kanan segmen terjauh
                segment.position = new Vector3(
                    rightmost + gap + sr.bounds.extents.x,
                    segment.position.y,
                    segment.position.z
                );
            }
        }
    }
}
