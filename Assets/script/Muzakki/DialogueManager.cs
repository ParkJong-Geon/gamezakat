using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Globalization;

using TMPro;

public class DialogueManager : MonoBehaviour
{
    public ZakatCalculationManager calculationManager;
    public GameObject dialoguePanel;
    public SceneManager1 scene1Manager;

    public TMP_Text nameText;
    public TMP_Text dialogueText;
    public Button nextButton;

    private MuzakkiData currentMuzakki;
    private int dialogueIndex;

    public void StartDialogue(MuzakkiData data){
        currentMuzakki = data;
        dialogueIndex = 0;

        dialoguePanel.SetActive(true);

        ShowDialogue();
    }

    private void ShowDialogue(){
        nameText.text= currentMuzakki.nama;

        if(dialogueIndex == 0){
            dialogueText.text = currentMuzakki.teks1;
        }
        else if(dialogueIndex == 1){
            dialogueText.text = currentMuzakki.teks2;
        }

        nextButton.gameObject.SetActive(true);
    }

    public void NextDialogue(){
        // dialogueIndex++;

        if(dialogueIndex == 2){
            EndSuccessDialogue();
            return;
        }
        dialogueIndex++;
        if(dialogueIndex >=2){
            EndDialogue();
            return;
        }
        ShowDialogue();
    }

    public void ShowSuccessDialogue(MuzakkiData data){
        currentMuzakki = data;

        dialogueIndex = 2;
        dialoguePanel.SetActive(true);
        nameText.text = currentMuzakki.nama;

        if (currentMuzakki.jenisHarta == JenisHarta.Kambing){
            dialogueText.text = "Baiklah, saya akan mengeluarkan zakat senilai " + currentMuzakki.jumlahKambingZakat + " ekor, terimakasih!";
        }
        else if (currentMuzakki.jenisHarta == JenisHarta.Gabah){
            dialogueText.text = "Baiklah, saya akan mengeluarkan zakat senilai " + currentMuzakki.jawabanZakat + " kilogram gabah kering, terimakasih!";
            }
        else {
            dialogueText.text = "Baiklah, saya akan mengeluarkan zakat senilai Rp." + currentMuzakki.jawabanZakat.ToString("N0", new System.Globalization.CultureInfo("id-ID")) + " , terimakasih!";
        }
        nextButton.gameObject.SetActive(true);
    }

    private void EndDialogue(){
        dialoguePanel.SetActive(false);

        Debug.Log("Dialog " + currentMuzakki.nama + "selesai.");

        calculationManager.StartCalculation(currentMuzakki);
    }

    private void EndSuccessDialogue(){
        dialoguePanel.SetActive(false);
        Debug.Log("Dialog berhasil" +currentMuzakki.nama+ "selesai");
        scene1Manager.FinishNPC();
    }
    
}
