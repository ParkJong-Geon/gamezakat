
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MustahikDialogueManager : MonoBehaviour
{
    [Header("Referensi Dialog")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text dialogueText;
    [SerializeField] private Button nextButton;

    [Header("Manajer Scene 2")]
    [SerializeField] private Scene2Manager scene2Manager;

    private CalonMustahikData currentCalon;
    private int dialogueIndex;
    private bool dialogAktif;

    public void StartDialogue(CalonMustahikData data)
    {
        if (data == null)
        {
            Debug.LogWarning("Data calon mustahik kosong.");
            return;
        }

        currentCalon = data;
        dialogueIndex = 0;
        dialogAktif = true;

        dialoguePanel.SetActive(true);
        ShowDialogue();
    }

    private void ShowDialogue()
    {
        nameText.text = currentCalon.nama;

        // Gunakan deskripsi calon sebagai isi wawancara
        // sementara. Nanti bisa diganti dengan dialog khusus.
        if (dialogueIndex == 0)
        {
            dialogueText.text =
                "Halo, nama saya " + currentCalon.nama + ". " +
                "Saya ingin mengajukan bantuan zakat.";
        }
        else
        {
            dialogueText.text = currentCalon.deskripsi;
        }

        nextButton.gameObject.SetActive(true);
    }

    public void NextDialogue()
    {
        if (!dialogAktif)
            return;

        dialogueIndex++;

        if (dialogueIndex >= 2)
        {
            EndDialogue();
            return;
        }

        ShowDialogue();
    }

    private void EndDialogue()
    {
        dialogAktif = false;
        dialoguePanel.SetActive(false);

        Debug.Log(
            "Wawancara calon mustahik selesai: " +
            currentCalon.nama
        );

        scene2Manager.TampilkanCalon(currentCalon);
    }

    private int GetIndeksCalon(CalonMustahikData data)
    {
        return scene2Manager.CariIndeksCalon(data);
    }
}
