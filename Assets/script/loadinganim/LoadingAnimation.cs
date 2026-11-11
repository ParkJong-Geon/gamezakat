
using UnityEngine;

public class LoadingAnimation : MonoBehaviour
{
    [Header("Jump Animation")]
    [SerializeField] private float jumpHeight = 30f;
    [SerializeField] private float jumpSpeed = 3f;

    private RectTransform rectTransform;
    private Vector2 startPosition;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        startPosition = rectTransform.anchoredPosition;
    }

    private void Update()
    {
        float movement = Mathf.Abs(
            Mathf.Sin(Time.time * jumpSpeed)
        ) * jumpHeight;

        rectTransform.anchoredPosition =
            startPosition + Vector2.up * movement;
    }
}
