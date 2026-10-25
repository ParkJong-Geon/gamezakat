using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ZakatCalculationManager : MonoBehaviour
{
    [Header("Panel")]
    public GameObject calculationPanel;

    [Header("UI")]
    public TMP_Text questionText;
    public TMP_InputField answerInput;
    public Button checkButton;
    public TMP_Text attemptText;

    [Header("Feedback")]
    public GameObject feedbackPopup;
    public TMP_Text feedbackText;
    public Button okButton;

    [Header("Dialogue")]
    public DialogueManager dialogueManager;

    private MuzakkiData currentMuzakki;

    private int attempts= 0;
    private const int maxAttempts = 3;

    private bool answerIsCorrect = false;
    private bool attemptsExhausted = false;

    public void StartCalculation(MuzakkiData data){
        currentMuzakki = data;
        attempts = 0;
        answerIsCorrect = false;
        attemptsExhausted = false;

        calculationPanel.SetActive(true);
        feedbackPopup.SetActive(false);

        answerInput.text = "";
        checkButton.interactable = true;

        UpdateAttemptText();
        ShowQuestion();
    }

    private void ShowQuestion(){
        questionText.text = "Berapakah zakat yang harus dibayarkan oleh " + currentMuzakki.nama + "?";
    }

    private bool TryParsePlayerAnswer(out float result){
        string input = answerInput.text.Trim();
        if(currentMuzakki.jenisHarta == JenisHarta.Gabah){
            input = input.Replace(',','.'); //titik koma sama aja
            return float.TryParse(
                input, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out result
            );
        }
        input = input.Replace(".", "").Replace(",", "").Replace(" ","");
        return float.TryParse(
            input, System.Globalization.NumberStyles.Number, System.Globalization.CultureInfo.InvariantCulture, out result
        );
    }   

    public void CheckAnswer(){
        if (string.IsNullOrWhiteSpace(answerInput.text)){
            ShowFeedback("Silahkan masukkan jawaban terlebih dahulu.");
            return;
        }

        float playerAnswer;

        if(!TryParsePlayerAnswer(out playerAnswer)){
            ShowFeedback( "Masukkan jawaban dalam bentuk format angka.");
            return;
        }

        attempts++;

        if (Mathf.Approximately(playerAnswer, currentMuzakki.jawabanZakat)){
            CorrectAnswer();
        }
        else{
            WrongAnswer();
        }
    }

    private void CorrectAnswer(){
        answerIsCorrect = true;
        checkButton.interactable = false;

    ShowFeedback(
        "Jawaban benar!\n\n" +
        "Zakat yang harus dibayarkan adalah " +
        FormatJawabanZakat()
    );
        Debug.Log(currentMuzakki.nama + " menjawab benar. Zakatnya adalah" + FormatJawabanZakat());
    }

    private void WrongAnswer(){
        int remainingAttempts = maxAttempts - attempts;

        if(attempts >= maxAttempts){
            attemptsExhausted = true;

            checkButton.interactable = false;

        ShowFeedback(
            "Jawaban masih kurang tepat.\n\n" +
            "Jawaban yang benar adalah " +
            FormatJawabanZakat()
        );            // checkButton.interactable = false;
            Debug.Log("Percobaan habis. Jawaban Benar adalah " + FormatJawabanZakat());
        }
        else{
            ShowFeedback( "Saya kira perhitungannya kurang tepat, " + "Boleh anda cek lagi?\n\n" + "Kesempatan tersisa: " + remainingAttempts);
            // answerInput.text= "";
            // answerInput.ActivateInputField();
        }
        UpdateAttemptText();
        
    }

    private void ShowFeedback(string message){
        feedbackText.text = message;
        feedbackPopup.SetActive(true);
    }

    public void CloseFeedBack(){
        feedbackPopup.SetActive(false);

        if(answerIsCorrect){
            FinishCalculation();
        }
        else if (attemptsExhausted){
            FinishCalculation();
        }
        else
        {            
            answerInput.text="";
            answerInput.ActivateInputField();

        }
    }

    private void FinishCalculation(){
        calculationPanel.SetActive(false);
        dialogueManager.ShowSuccessDialogue(
            currentMuzakki
        );
    }

    private void UpdateAttemptText(){
        attemptText.text= "Percobaan: " + attempts + " / " + maxAttempts;
    }
    
    private string FormatJawabanZakat()
    {
        var budaya = new System.Globalization.CultureInfo("id-ID");
        if (currentMuzakki.jenisHarta == JenisHarta.Kambing)
        {
            return currentMuzakki.jumlahKambingZakat
                + " ekor kambing/domba";
        } else if (currentMuzakki.jenisHarta == JenisHarta.Gabah) {
            return currentMuzakki.jawabanZakat.ToString("0.##", budaya) + " kilogram gabah";
        }

        return "Rp" + currentMuzakki.jawabanZakat.ToString("N0", budaya);
    }

    private string FormatJumlahHarta()
    {
        if (currentMuzakki.jenisHarta == JenisHarta.Kambing)
        {
            return currentMuzakki.jumlahHarta + " ekor kambing";
        }
        else if (currentMuzakki.jenisHarta == JenisHarta.Gabah){
            return currentMuzakki.jumlahHarta + "kilogram gabah kering";
        }

        return currentMuzakki.jumlahHarta.ToString("N0");
    }
}
