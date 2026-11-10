
using System;
using UnityEngine;

public enum StatusFakta
{
    BelumDiketahui,
    Benar,
    Salah
}

public enum KategoriMustahik
{
    BelumDisimpulkan,
    Fakir,
    Miskin,
    Gharim,
    IbnuSabil
}

[Serializable]
public class CalonMustahikData
{
    [Header("Identitas NPC")]
    public string idNPC;

    [Header("Informasi Pengajuan")]
    public string nama;
    public string kategoriDiajukan;

    [TextArea(3, 6)]
    public string deskripsi;

    [Header("Fakta Hasil Wawancara")]
    public StatusFakta K01_Bekerja;
    public StatusFakta K02_TidakBekerja;

    public StatusFakta K03_MampuMemenuhiKebutuhan;
    public StatusFakta K04_TidakMampuMemenuhiKebutuhan;

    public StatusFakta K05_Berhutang;
    public StatusFakta K06_TidakBerhutang;

    public StatusFakta K07_HutangMasihLama;
    public StatusFakta K08_HutangJatuhTempo;

    public StatusFakta K09_HutangUntukMaksiatPemborosan;
    public StatusFakta K10_HutangBukanUntukMaksiat;

    public StatusFakta K11_Menetap;
    public StatusFakta K12_TidakMenetap;

    public StatusFakta K13_PerjalananUntukMaksiat;
    public StatusFakta K14_PerjalananBukanUntukMaksiat;
}
    