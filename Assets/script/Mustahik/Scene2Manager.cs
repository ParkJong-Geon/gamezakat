
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Scene2Manager : MonoBehaviour
{
    [Header("Panel Dokumen")]
    [SerializeField] private GameObject panelDataMustahik;
    [SerializeField] private TMP_Text namaCalonText;
    [SerializeField] private TMP_Text kategoriPengajuanText;
    [SerializeField] private TMP_Text deskripsiText;

    [Header("Tombol Keputusan")]
    [SerializeField] private Button terimaButton;
    [SerializeField] private Button tolakButton;

    [Header("Panel Petunjuk")]
    [SerializeField] private GameObject petunjukPanel;
    [SerializeField] private TMP_Text petunjukText;
    [SerializeField] private Button lanjutButton;

    [Header("Data Calon Mustahik")]
    [SerializeField]
    private List<CalonMustahikData> daftarCalon =
        new List<CalonMustahikData>();

    [Header("Pengaturan Alur NPC")]
    [SerializeField] private Scene2FlowManager flowManager;

    private int indeksCalon = 0;
    private bool pemeriksaanSelesai = false;
    private int jumlahKesalahan = 0;
    private CalonMustahikData dataCalonAktif;

    private void Awake()
    {
        terimaButton.onClick.AddListener(
            () => PeriksaKeputusan(true));

        tolakButton.onClick.AddListener(
            () => PeriksaKeputusan(false));

        lanjutButton.onClick.AddListener(LanjutCalon);
    }

    private void Start()
    {
        panelDataMustahik.SetActive(false);
        petunjukPanel.SetActive(false);
        lanjutButton.gameObject.SetActive(false);

        terimaButton.interactable = true;
        tolakButton.interactable = true;
    }

    // Panggil metode ini setelah wawancara selesai.
    public void TampilkanCalon(int indeks)
    {
        jumlahKesalahan = 0;
        if (indeks < 0 || indeks >= daftarCalon.Count)
        {
            Debug.LogWarning(
                "Indeks calon mustahik tidak valid.");
            return;
        }

        indeksCalon = indeks;
        pemeriksaanSelesai = false;

        CalonMustahikData data = daftarCalon[indeksCalon];

        namaCalonText.text = data.nama;
        kategoriPengajuanText.text =
            "Kategori yang diajukan: " + data.kategoriDiajukan;
        deskripsiText.text = data.deskripsi;

        panelDataMustahik.SetActive(true);
        petunjukPanel.SetActive(false);
        lanjutButton.gameObject.SetActive(false);

        terimaButton.interactable = true;
        tolakButton.interactable = true;
    }

        
    public void TampilkanCalon(CalonMustahikData data)
    {
        if (data == null)
        {
            Debug.LogWarning("Data calon mustahik kosong.");
            return;
        }

        // Simpan data calon yang sedang diperiksa.
        dataCalonAktif = data;
        pemeriksaanSelesai = false;
        jumlahKesalahan = 0;

        namaCalonText.text = data.nama;
        kategoriPengajuanText.text =
            "Kategori yang diajukan: " + data.kategoriDiajukan;
        deskripsiText.text = data.deskripsi;

        panelDataMustahik.SetActive(true);
        petunjukPanel.SetActive(false);
        lanjutButton.gameObject.SetActive(false);

        terimaButton.interactable = true;
        tolakButton.interactable = true;
    }

    private void PeriksaKeputusan(bool pemainMenerima)
    {
        if (pemeriksaanSelesai)
            return;

        CalonMustahikData data = dataCalonAktif;
        KategoriMustahik hasil =
            ForwardChainingManager.Analisis(data);

        KategoriMustahik kategoriDiajukan =
            UbahKategori(data.kategoriDiajukan);

        // Pengajuan hanya sesuai jika hasil FC cocok
        // dengan kategori yang diajukan.
        bool pengajuanSesuai =
            hasil != KategoriMustahik.BelumDisimpulkan &&
            kategoriDiajukan != KategoriMustahik.BelumDisimpulkan &&
            hasil == kategoriDiajukan;

        bool keputusanBenar =
            pemainMenerima == pengajuanSesuai;

        petunjukPanel.SetActive(true);

        // Keputusan benar: selesaikan pemeriksaan.
        if (keputusanBenar)
        {
            pemeriksaanSelesai = true;
    
            if (pemainMenerima)
            {
                // Simpan calon yang diterima pemain.
                GameSessionData.TambahkanMustahik(data);

                petunjukText.text =
                    "Keputusan kamu benar. Pengajuan sesuai " +
                    "dengan hasil pemeriksaan, yaitu " + hasil + ".";
            }
            else
            {
                petunjukText.text =
                    "Keputusan kamu benar. Berdasarkan aturan " +
                    "dalam game, pengajuan ini tidak sesuai " +
                    "dengan kriteria kategori yang diajukan.";
            }

            terimaButton.interactable = false;
            tolakButton.interactable = false;
            lanjutButton.gameObject.SetActive(true);
            return;
        }

        // Keputusan salah: naikkan tingkat petunjuk.
        jumlahKesalahan++;

        if (jumlahKesalahan == 1)
        {
            petunjukText.text = BuatPetunjukUmum(data);
        }
        else if (jumlahKesalahan == 2)
        {
            petunjukText.text = BuatPetunjukRinci(data, hasil);
        }
        else
        {
            pemeriksaanSelesai = true;

            petunjukText.text = BuatJawabanAkhir(data, hasil);

            terimaButton.interactable = false;
            tolakButton.interactable = false;
            lanjutButton.gameObject.SetActive(true);
        }
    }


    private string BuatPetunjukUmum(CalonMustahikData data)
    {
        return "Coba perhatikan kembali kondisi calon mustahik " +
            "berdasarkan deskripsi dan hasil wawancara. " +
            "Apakah kondisi tersebut sudah sesuai dengan " +
            "kategori yang diajukan?";
    }

    private string BuatPetunjukRinci(
        CalonMustahikData data,
        KategoriMustahik hasil)
    {
        string kategori = data.kategoriDiajukan;

        if (hasil != KategoriMustahik.BelumDisimpulkan)
        {
            return "Perhatikan kembali kesesuaian antara kondisi " +
                "calon mustahik dan kategori yang diajukan. " +
                "Berdasarkan pemeriksaan, kondisi calon lebih " +
                "sesuai dengan kategori " + hasil + ". " +
                "Pertimbangkan apakah pengajuan perlu diterima " +
                "atau ditolak.";
        }

        switch (kategori.Trim().ToLowerInvariant())
        {
            case "fakir":
                return "Perhatikan apakah calon mampu memenuhi" +
                    "kebutuhan sehari-hari.";

            case "miskin":
                return "Perhatikan apakah calon mampu memenuhi" +
                    "kebutuhan sehari-hari.";

            case "gharim":
            case "ghorim":
                return "Perhatikan tujuan calon, apakah " +
                    "utang tersebut genting? ";
            case "ibnu sabil":
            case "ibnusabil":
                return "Perhatikan apakah kondisi calon" +
                    "dalam kondisi tertentu " +
                    "dan mengalami kesulitan memenuhi kebutuhan.";

            default:
                return "Periksa kembali kondisi ekonomi, " +
                    " dan situasi calon mustahik.";
        }
    }

    
    private string BuatJawabanAkhir(
        CalonMustahikData data,
        KategoriMustahik hasil)
    {
        KategoriMustahik diajukan =
            UbahKategori(data.kategoriDiajukan);

        if (hasil == KategoriMustahik.BelumDisimpulkan)
        {
            return "Jawaban: calon ini tidak memenuhi kriteria " +
                "mustahik berdasarkan aturan yang diterapkan " +
                "dalam game. Kondisinya tidak cocok dengan " +
                "kriteria empat kategori yang tersedia.";
        }

        if (diajukan == hasil)
        {
            return "Jawaban: pengajuan dapat diterima. " +
                "Kategori yang diajukan sesuai dengan hasil " +
                "pemeriksaan, yaitu " + hasil + ". " +
                "Perhatikan kembali kondisi calon pada " +
                "dokumen dan hasil wawancara.";
        }

        return "Jawaban: pengajuan tidak sesuai dengan hasil " +
            "pemeriksaan. Calon mengajukan kategori " +
            data.kategoriDiajukan + ", tetapi kondisinya " +
            "lebih sesuai dengan kategori " + hasil + ".";
    }

    
    public int CariIndeksCalon(CalonMustahikData data)
    {
        return daftarCalon.IndexOf(data);
    }
    private void LanjutCalon()
    {
        if (panelDataMustahik == null)
        {
            Debug.LogError("Panel Data Mustahik belum dihubungkan!");
            return;
        }

        if (petunjukPanel == null)
        {
            Debug.LogError("Petunjuk Panel belum dihubungkan!");
            return;
        }

        if (flowManager == null)
        {
            Debug.LogError("Flow Manager belum dihubungkan ke Scene2Manager!");
            return;
        }

        panelDataMustahik.SetActive(false);
        petunjukPanel.SetActive(false);

        flowManager.SelesaikanNPC();
    }

    private KategoriMustahik UbahKategori(string kategori)
    {
        if (string.IsNullOrWhiteSpace(kategori))
            return KategoriMustahik.BelumDisimpulkan;

        string teks = kategori.Trim().ToLowerInvariant()
            .Replace(" ", "")
            .Replace("_", "");

        switch (teks)
        {
            case "fakir":
                return KategoriMustahik.Fakir;

            case "miskin":
                return KategoriMustahik.Miskin;

            case "gharim":
            case "ghorim":
                return KategoriMustahik.Gharim;

            case "ibnusabil":
                return KategoriMustahik.IbnuSabil;

            default:
                return KategoriMustahik.BelumDisimpulkan;
        }
    }
}
