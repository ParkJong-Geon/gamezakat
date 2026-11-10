
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scene2FlowManager : MonoBehaviour
{
    [Header("Pengaturan Scene")]
    [SerializeField] private string namaSceneBerikutnya = "scene3";

    [Header("Daftar NPC")]
    [SerializeField] private List<MustahikNPCData> semuaNPC =
        new List<MustahikNPCData>();

    [Header("Rute NPC")]
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private Transform area1;
    [SerializeField] private Transform servicePoint;
    [SerializeField] private Transform exitPoint1;
    [SerializeField] private Transform exitPoint2;

    [Header("Referensi Sistem")]
    [SerializeField] private MustahikDialogueManager dialogueManager;

    private List<MustahikNPCData> npcTerpilih =
        new List<MustahikNPCData>();

    private int indeksAktif = 0;
    private bool sesiDimulai = false;
    private bool npcSedangDiproses = false;

    public void MulaiSesi()
    {
        if (semuaNPC.Count < 3)
        {
            Debug.LogWarning("Minimal harus tersedia 3 NPC.");
            return;
        }

        if (spawnPoint == null || area1 == null ||
            servicePoint == null || exitPoint1 == null ||
            exitPoint2 == null || dialogueManager == null)
        {
            Debug.LogError("Referensi rute atau Dialogue Manager belum lengkap.");
            return;
        }

        npcTerpilih.Clear();

        List<MustahikNPCData> kandidat =
            new List<MustahikNPCData>();

        foreach (MustahikNPCData npc in semuaNPC)
        {
            if (npc != null && npc.data != null)
            {
                kandidat.Add(npc);
            }

            if (npc != null)
                npc.gameObject.SetActive(false);
        }

        if (kandidat.Count < 3)
        {
            Debug.LogWarning("NPC dengan data valid kurang dari 3.");
            return;
        }

        // Acak urutan NPC
        for (int i = kandidat.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);

            MustahikNPCData temp = kandidat[i];
            kandidat[i] = kandidat[j];
            kandidat[j] = temp;
        }

        // Ambil 3 NPC tanpa pengulangan
        for (int i = 0; i < 3; i++)
        {
            npcTerpilih.Add(kandidat[i]);
        }

        indeksAktif = 0;
        sesiDimulai = true;

        PanggilNPCBerikutnya();
        GameSessionData.ResetData();
        npcTerpilih.Clear();
    }

    private void PanggilNPCBerikutnya()
    {
        if (!sesiDimulai) return;

        if (indeksAktif >= npcTerpilih.Count)
        {
            sesiDimulai = false;

            Debug.Log("Semua NPC selesai diproses.");

            Debug.Log(
                "Jumlah mustahik yang diterima: " +
                GameSessionData.mustahikDiterima.Count
            );

            SceneManager.LoadScene(namaSceneBerikutnya);
            return;
        }

        MustahikNPCData npcData = npcTerpilih[indeksAktif];

        npcData.gameObject.SetActive(true);

        NPCController controller =
            npcData.GetComponent<NPCController>();

        if (controller == null)
        {
            Debug.LogError("NPC tidak memiliki NPCController.");
            return;
        }

        // Tempatkan NPC di Spawn Point
        controller.SetGroundPosition(spawnPoint.position);

        // Berjalan dari Spawn -> Area 1 -> Meja
        controller.MoveThroughPoints(
            area1.position,
            servicePoint.position
        );

        StartCoroutine(TungguNPCSampaiMeja(controller, npcData));
    }

    private IEnumerator TungguNPCSampaiMeja(
        NPCController controller,
        MustahikNPCData npcData)
    {
        while (controller.IsMoving())
            yield return null;

        // Mulai wawancara setelah NPC sampai di meja
        dialogueManager.StartDialogue(npcData.data);
    }

    public void SelesaikanNPC()
    {
        if (!sesiDimulai || npcSedangDiproses) return;

        npcSedangDiproses = true;
        StartCoroutine(ProsesNPCKeluar());
    }

    private IEnumerator ProsesNPCKeluar()
    {
        MustahikNPCData npcData = npcTerpilih[indeksAktif];

        NPCController controller =
            npcData.GetComponent<NPCController>();

        // Berjalan dari meja -> Exit 1 -> Exit 2
        controller.MoveThroughPoints(
            exitPoint1.position,
            exitPoint2.position
        );

        while (controller.IsMoving())
            yield return null;

        npcData.gameObject.SetActive(false);

        indeksAktif++;
        npcSedangDiproses = false;

        PanggilNPCBerikutnya();
    }
}
