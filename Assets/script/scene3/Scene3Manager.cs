
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[System.Serializable]
public class RiwayatDistribusi
{
    public string namaMustahik;
    public string idNPC;
    public string jenisBantuan;
}

public class Scene3Manager : MonoBehaviour
{
    [Header("Panel Ronde 3")]
    [SerializeField] private GameObject panelPerjalanan;
    [SerializeField] private GameObject panelLoading;
    [SerializeField] private GameObject panelLokasiMustahik;

    [Header("Panel Akhir")]
    [SerializeField] private GameObject panelAkhirPermainan;
    [SerializeField] private TMP_Text ringkasanDistribusi;

    [Header("Data dan Tampilan NPC")]
    [SerializeField] private List<Scene3MustahikView> daftarView =
        new List<Scene3MustahikView>();

    [SerializeField] private Scene3DialogueManager dialogueManager;

    [Header("Pengaturan Waktu")]
    [SerializeField] private float durasiPerjalanan = 5f;
    [SerializeField] private float durasiLoading = 1.5f;
    [SerializeField] private float durasiPenyerahan = 1f;

    [Header("Placeholder Animasi Penyerahan")]
    [SerializeField] private GameObject objekAnimasiBantuan;
    [SerializeField] private Transform titikAwalBantuan;
    [SerializeField] private Transform titikTujuanBantuan;

    private List<CalonMustahikData> daftarMustahik =
        new List<CalonMustahikData>();

    private List<RiwayatDistribusi> riwayatDistribusi =
        new List<RiwayatDistribusi>();

    private Scene3MustahikView viewAktif;

    private int indeksAktif = 0;
    private bool prosesBerjalan = false;

    private void Start()
    {
        daftarMustahik =
            new List<CalonMustahikData>(
                GameSessionData.mustahikDiterima
            );

        SembunyikanSemuaLokasi();

        if (panelAkhirPermainan != null)
            panelAkhirPermainan.SetActive(false);

        if (objekAnimasiBantuan != null)
            objekAnimasiBantuan.SetActive(false);

        if (daftarMustahik.Count == 0)
        {
            Debug.LogWarning(
                "Tidak ada mustahik yang diterima dari Ronde 2."
            );
            TampilkanAkhirPermainan();
            return;
        }

        MulaiPerjalanan();
    }

    private void SembunyikanSemuaLokasi()
    {
        foreach (Scene3MustahikView view in daftarView)
        {
            if (view == null) continue;

            if (view.rumah != null)
                view.rumah.SetActive(false);

            if (view.karakter != null)
                view.karakter.SetActive(false);
        }

        panelPerjalanan.SetActive(false);
        panelLoading.SetActive(false);
        panelLokasiMustahik.SetActive(false);
    }

    public void MulaiPerjalanan()
    {
        if (prosesBerjalan) return;

        if (indeksAktif >= daftarMustahik.Count)
        {
            TampilkanAkhirPermainan();
            return;
        }

        StartCoroutine(ProsesPerjalanan());
    }

    private IEnumerator ProsesPerjalanan()
    {
        prosesBerjalan = true;

        panelLokasiMustahik.SetActive(false);
        panelLoading.SetActive(false);
        panelPerjalanan.SetActive(true);

        yield return new WaitForSeconds(durasiPerjalanan);

        panelPerjalanan.SetActive(false);
        panelLoading.SetActive(true);

        yield return new WaitForSeconds(durasiLoading);

        panelLoading.SetActive(false);

        TampilkanLokasiMustahik();

        prosesBerjalan = false;
    }

    private void TampilkanLokasiMustahik()
    {
        CalonMustahikData data = daftarMustahik[indeksAktif];

        viewAktif = daftarView.Find(
            x => x != null && x.idNPC == data.idNPC
        );

        if (viewAktif == null)
        {
            Debug.LogError(
                "View untuk NPC " + data.nama +
                " dengan ID " + data.idNPC + " tidak ditemukan."
            );
            return;
        }

        if (viewAktif.rumah == null ||
            viewAktif.karakter == null)
        {
            Debug.LogError(
                "Rumah atau karakter belum dihubungkan untuk " +
                data.nama
            );
            return;
        }

        viewAktif.rumah.SetActive(true);
        viewAktif.karakter.SetActive(true);

        panelLokasiMustahik.SetActive(true);

        dialogueManager.MulaiDialog(data, viewAktif);
    }

    public void ProsesDistribusi(string jenisBantuan)
    {
        if (prosesBerjalan) return;

        StartCoroutine(AnimasiPenyerahan(jenisBantuan));
    }

    private IEnumerator AnimasiPenyerahan(string jenisBantuan)
    {
        prosesBerjalan = true;

        // Placeholder animasi.
        // Ganti bagian ini dengan animasi sprite saat aset tersedia.

        if (objekAnimasiBantuan != null &&
            titikAwalBantuan != null &&
            titikTujuanBantuan != null)
        {
            objekAnimasiBantuan.SetActive(true);
            objekAnimasiBantuan.transform.position =
                titikAwalBantuan.position;

            float waktu = 0f;

            while (waktu < durasiPenyerahan)
            {
                waktu += Time.deltaTime;

                objekAnimasiBantuan.transform.position =
                    Vector3.Lerp(
                        titikAwalBantuan.position,
                        titikTujuanBantuan.position,
                        waktu / durasiPenyerahan
                    );

                yield return null;
            }

            objekAnimasiBantuan.transform.position =
                titikTujuanBantuan.position;

            objekAnimasiBantuan.SetActive(false);
        }
        else
        {
            yield return new WaitForSeconds(durasiPenyerahan);
        }

        CatatDistribusi(jenisBantuan);

        SembunyikanLokasiAktif();

        indeksAktif++;

        prosesBerjalan = false;

        if (indeksAktif < daftarMustahik.Count)
        {
            MulaiPerjalanan();
        }
        else
        {
            TampilkanAkhirPermainan();
        }
    }

    private void CatatDistribusi(string jenisBantuan)
    {
        CalonMustahikData data = daftarMustahik[indeksAktif];

        RiwayatDistribusi riwayat = new RiwayatDistribusi();

        riwayat.namaMustahik = data.nama;
        riwayat.idNPC = data.idNPC;
        riwayat.jenisBantuan = jenisBantuan;

        riwayatDistribusi.Add(riwayat);

        Debug.Log(
            "Distribusi berhasil: " + data.nama +
            " | Bantuan: " + jenisBantuan
        );
    }

    private void SembunyikanLokasiAktif()
    {
        if (viewAktif == null) return;

        viewAktif.rumah.SetActive(false);
        viewAktif.karakter.SetActive(false);
    }

    private void TampilkanAkhirPermainan()
    {
        panelPerjalanan.SetActive(false);
        panelLoading.SetActive(false);
        panelLokasiMustahik.SetActive(false);

        if (panelAkhirPermainan != null)
            panelAkhirPermainan.SetActive(true);

        if (ringkasanDistribusi != null)
        {
            string ringkasan = "Distribusi zakat selesai.\n\n";

            foreach (RiwayatDistribusi item in riwayatDistribusi)
            {
                ringkasan += item.namaMustahik +
                    " - " + item.jenisBantuan + "\n";
            }

            ringkasanDistribusi.text = ringkasan;
        }

        Debug.Log(
            "Seluruh distribusi selesai. Total: " +
            riwayatDistribusi.Count
        );
    }
}
