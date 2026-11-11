
using System.Collections;
using TMPro;
using UnityEngine;

public class TypewriterEffect : MonoBehaviour
{
    [Header("Text Settings")]
    [SerializeField] private TMP_Text loadingText;
    [SerializeField] private string message = "Memuat Permainan...";
    [SerializeField] private float typingSpeed = 0.08f;

    private void Start()
    {
        StartCoroutine(TypeText());
    }

    private IEnumerator TypeText()
    {
        loadingText.text = "";

        foreach (char letter in message)
        {
            loadingText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
    }
}
