
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Scene3DialogueManager : MonoBehaviour
{
    [Header("Panel")]
    [SerializeField] private GameObject panelDialog;
    [SerializeField] private GameObject panelBantuan;
    [SerializeField] private GameObject panelKonfirmasi;
    [SerializeField] private GameObject panelFeedback;

    [Header("Data Mustahik")]
    [SerializeField] private TMP_Text namaNPC;
    [SerializeField] private TMP_Text isiDialog;
    [SerializeField] private TMP_Text kategoriMustahik;
    [SerializeField] private TMP_Text namaNPC2;
    [SerializeField] private TMP_Text deskripsiMustahik;

    [Header("Dialog")]
    [SerializeField] private Button buttonLanjut;

    [Header("Pilihan Bantuan")]
    [SerializeField] private Button buttonKesehatan;
    [SerializeField] private Button buttonKeuangan;
    [SerializeField] private Button buttonSembako;
    [SerializeField] private Button buttonAlatKerja;

    [Header("Konfirmasi")]
    [SerializeField] private TMP_Text konfirmasiText;
    [SerializeField] private Button buttonYa;
    [SerializeField] private Button buttonTidak;

    [Header("Feedback")]
    [SerializeField] private TMP_Text feedbackText;
    [SerializeField] private Button buttonLanjutFeedback;

    [Header("Referensi Scene 3")]
    [SerializeField] private Scene3Manager scene3Manager;

    private CalonMustahikData dataAktif;
    private Scene3MustahikView viewAktif;

    private string bantuanDipilih;
    private int indeksDialog;
    private int jumlahKesalahan;

    private void Awake()
    {
        buttonLanjut.onClick.AddListener(LanjutDialog);

        buttonKesehatan.onClick.AddListener(
            () => PilihBantuan("Kesehatan"));

        buttonKeuangan.onClick.AddListener(
            () => PilihBantuan("Keuangan"));

        buttonSembako.onClick.AddListener(
            () => PilihBantuan("Sembako"));

        buttonAlatKerja.onClick.AddListener(
            () => PilihBantuan("AlatKerja"));

        buttonYa.onClick.AddListener(KonfirmasiYa);
        buttonTidak.onClick.AddListener(KonfirmasiTidak);

        buttonLanjutFeedback.onClick.AddListener(LanjutSetelahFeedback);
    }

    public void MulaiDialog(
        CalonMustahikData data,
        Scene3MustahikView view)
    {
        if (data == null || view == null)
        {
            Debug.LogError("Data dialog atau view kosong.");
            return;
        }

        dataAktif = data;
        viewAktif = view;

        indeksDialog = 0;
        jumlahKesalahan = 0;
        bantuanDipilih = "";

        panelDialog.SetActive(true);
        panelBantuan.SetActive(false);
        panelKonfirmasi.SetActive(false);
        panelFeedback.SetActive(false);

        namaNPC.text = data.nama;
        namaNPC2.text = data.nama;
        kategoriMustahik.text = data.kategoriDiajukan;
        deskripsiMustahik.text = data.deskripsi;

        TampilkanDialog();
    }

    private void TampilkanDialog()
    {
        if (indeksDialog == 0)
        {
            isiDialog.text =
                "Halo, saya " + dataAktif.nama +
                ". Terima kasih sudah datang mengunjungi saya.";
        }
        else
        {
            isiDialog.text =
                "Kondisi saya saat ini: " + dataAktif.deskripsi;
        }
    }

    private void LanjutDialog()
    {
        indeksDialog++;

        if (indeksDialog >= 2)
        {
            panelDialog.SetActive(false);
            TampilkanPilihanBantuan();
            return;
        }

        TampilkanDialog();
    }

    private void TampilkanPilihanBantuan()
    {
        panelBantuan.SetActive(true);

        buttonKesehatan.gameObject.SetActive(true);
        buttonKeuangan.gameObject.SetActive(true);
        buttonSembako.gameObject.SetActive(true);
        buttonAlatKerja.gameObject.SetActive(true);
    }

    private void PilihBantuan(string jenisBantuan)
    {
        bantuanDipilih = jenisBantuan;

        konfirmasiText.text =
            "Apakah kamu yakin ingin menyerahkan bantuan " +
            jenisBantuan + " kepada " + dataAktif.nama + "?";

        panelKonfirmasi.SetActive(true);
    }

    private void KonfirmasiTidak()
    {
        panelKonfirmasi.SetActive(false);

        bantuanDipilih = "";

        panelBantuan.SetActive(true);
    }

    private void KonfirmasiYa()
    {
        panelKonfirmasi.SetActive(false);

        string bantuanSesuai = viewAktif.jenisBantuanSesuai;

        if (bantuanDipilih == bantuanSesuai)
        {
            panelBantuan.SetActive(false);

            panelFeedback.SetActive(true);

            feedbackText.text =
                "Keputusan kamu benar. Bantuan " +
                bantuanSesuai + " sesuai dengan kebutuhan " +
                dataAktif.nama + ".";

            buttonLanjutFeedback.gameObject.SetActive(true);
            return;
        }

        jumlahKesalahan++;

        if (jumlahKesalahan >= 3)
        {
            panelBantuan.SetActive(false);
            panelFeedback.SetActive(true);

            feedbackText.text =
                "Kamu sudah melakukan tiga kali kesalahan. " +
                "Berdasarkan kebutuhan mustahik, bantuan yang " +
                "sesuai adalah " + bantuanSesuai + ".";

            bantuanDipilih = bantuanSesuai;

            buttonLanjutFeedback.gameObject.SetActive(true);
        }
        else
        {
            // Kesalahan pertama dan kedua tidak menampilkan feedback.
            // Pemain diberi kesempatan memilih kembali.
            bantuanDipilih = "";

            panelBantuan.SetActive(true);
        }
    }

    private void LanjutSetelahFeedback()
    {
        panelFeedback.SetActive(false);

        scene3Manager.ProsesDistribusi(bantuanDipilih);
    }
}
