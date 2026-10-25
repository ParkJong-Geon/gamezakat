using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager1 : MonoBehaviour
{
    [Header("Manager")]
    public MuzakkiGenerator generator;
    public DialogueManager dialogueManager;

    [Header("NPC")]
    public NPCController hajiFaisal;
    public NPCController dhinita;
    public NPCController budi;
    public NPCController suzan;
    public NPCController bangDodo;
    private NPCController currentNPC;

    [Header("Jalur Masuk")]
    public Transform spawnPoint;

    public Transform area1;
    public Transform servicePoint;

    [Header("Jalur Keluar")]
    public Transform exitPoint1;
    public Transform exitPoint2;

    [Header("Catatan Harta")]
    public CatatanHarta catatanHartaUI;

    private MuzakkiData currentMuzakki;
    
    private int currentNPCIndex = 0;

    private bool serviceStarted = false;
    private bool npcLeaving = false;


    // Start is called before the first frame update
    private void Start()
    {
        dialogueManager.dialoguePanel.SetActive(false);
        catatanHartaUI.SembunyikanCatatan();

        SpawnNextMuzakki();
        // currentMuzakki = generator.BuatDhinita();

        // Debug.Log(
        //     "Dhinita memiliki " + dhinita.jumlahHarta + " gram emas."
        // );

        // Debug.Log(
        //     "zakat = Rp "+ dhinita.jawabanZakat.ToString("N0")
        // );

//        currentNPC.MoveThroughPoints(area1.position, servicePoint.position);
        //dialogueManager.StartDialogue(dhinita);
    }

    private void Update(){
        //npc diservpo
        if(!currentNPC.IsMoving() && !serviceStarted && !npcLeaving){
            StartService();
        }
        //npc kluar
        if(!currentNPC.IsMoving() && npcLeaving){
            npcLeaving = false;
            currentNPCIndex++;
            if(currentNPCIndex < 5){
                SpawnNextMuzakki();
            }else{
                FinishScene();
            }
        }
    }

    private void SpawnNextMuzakki(){
        serviceStarted = false;
        npcLeaving = false;

        HideAllNPC();
//datamuz ambil
        currentMuzakki = GetMuzakkiData(currentNPCIndex);
    //ambil karakter urutan
        currentNPC = GetNPCController(currentNPCIndex);
        currentNPC.gameObject.SetActive(true);
        currentNPC.SetGroundPosition(spawnPoint.position);
        currentNPC.MoveThroughPoints(area1.position, servicePoint.position);
        catatanHartaUI.TampilkanCatatan(currentMuzakki);
    }

    private MuzakkiData GetMuzakkiData(int index){
        switch (index){
            case 0:
            return generator.BuatBangDodo();
  //          return generator.BuatHajiFaisal();
            case 1:
   //         return generator.BuatHajiFaisal();
            return generator.BuatDhinita();
            case 2:
            return generator.BuatBudi();
            case 3:
            return generator.BuatSuzan();
            case 4:
            return generator.BuatBangDodo();
            default:
            return null;
        }
    }

    private NPCController GetNPCController(int index){
        switch (index){
            case 0:
            return bangDodo;
       //     return hajiFaisal;
            case 1:
            return dhinita;
     //       return hajiFaisal;
            case 2:
            return budi;
            case 3:
            return suzan;
            case 4:
            return bangDodo;
            default:
            return null;

        }
    }

    private void HideAllNPC(){
        hajiFaisal.gameObject.SetActive(false);
        dhinita.gameObject.SetActive(false);
        budi.gameObject.SetActive(false);
        suzan.gameObject.SetActive(false);
        bangDodo.gameObject.SetActive(false);
    }

//    private bool serviceStarted = false;

    private void StartService(){
        // if(serviceStarted)
        // return;
        serviceStarted = true;

        dialogueManager.StartDialogue(currentMuzakki);
    }

    public void FinishNPC(){
        dialogueManager.dialoguePanel.SetActive(false);
        npcLeaving = true;
        currentNPC.MoveThroughPoints(exitPoint1.position, exitPoint2.position);
    }

    private void FinishScene(){
        Debug.Log("done");
    }

}
