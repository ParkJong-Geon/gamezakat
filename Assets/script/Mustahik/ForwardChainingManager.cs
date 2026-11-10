
using System.Collections.Generic;
using System.Text;

public class ForwardChainingManager
{
    public static KategoriMustahik Analisis(
        CalonMustahikData data)
    {
        // R1: K02 AND K04 AND K06 AND K11 => Fakir
        if (SemuaBenar(
            data.K02_TidakBekerja,
            data.K04_TidakMampuMemenuhiKebutuhan,
            data.K06_TidakBerhutang,
            data.K11_Menetap))
        {
            return KategoriMustahik.Fakir;
        }

        // R2: K01 AND K04 AND K06 AND K11 => Miskin
        if (SemuaBenar(
            data.K01_Bekerja,
            data.K04_TidakMampuMemenuhiKebutuhan,
            data.K06_TidakBerhutang,
            data.K11_Menetap))
        {
            return KategoriMustahik.Miskin;
        }

        // R3: K04 AND K05 AND K08 AND K10 AND K11 => Gharim
        if (SemuaBenar(
            data.K04_TidakMampuMemenuhiKebutuhan,
            data.K05_Berhutang,
            data.K08_HutangJatuhTempo,
            data.K10_HutangBukanUntukMaksiat,
            data.K11_Menetap))
        {
            return KategoriMustahik.Gharim;
        }

        // R4: K04 AND K12 AND K14 => Ibnu Sabil
        if (SemuaBenar(
            data.K04_TidakMampuMemenuhiKebutuhan,
            data.K12_TidakMenetap,
            data.K14_PerjalananBukanUntukMaksiat))
        {
            return KategoriMustahik.IbnuSabil;
        }

        return KategoriMustahik.BelumDisimpulkan;
    }

    private static bool SemuaBenar(
        params StatusFakta[] fakta)
    {
        foreach (StatusFakta item in fakta)
        {
            if (item != StatusFakta.Benar)
            {
                return false;
            }
        }

        return true;
    }

   
    public static string BuatPetunjuk(
        CalonMustahikData data,
        KategoriMustahik hasil)
    {
        if (hasil != KategoriMustahik.BelumDisimpulkan)
        {
            return "Hasil pemeriksaan sistem: " + hasil +
                ". Bandingkan hasil ini dengan kategori " +
                "yang diajukan calon mustahik.";
        }

        System.Text.StringBuilder pesan =
            new System.Text.StringBuilder();

        pesan.AppendLine("Kategori belum dapat disimpulkan.");
        pesan.AppendLine("Periksa kembali fakta berikut:");

        // R1: Fakir
        TambahkanJikaBelumDiketahui(
            pesan, data.K02_TidakBekerja, "K02 - Tidak bekerja");
        TambahkanJikaBelumDiketahui(
            pesan, data.K04_TidakMampuMemenuhiKebutuhan,
            "K04 - Tidak mampu memenuhi kebutuhan");
        TambahkanJikaBelumDiketahui(
            pesan, data.K06_TidakBerhutang, "K06 - Tidak berutang");
        TambahkanJikaBelumDiketahui(
            pesan, data.K11_Menetap, "K11 - Menetap");

        // R2: Miskin
        TambahkanJikaBelumDiketahui(
            pesan, data.K01_Bekerja, "K01 - Bekerja");

        // R3: Gharim
        TambahkanJikaBelumDiketahui(
            pesan, data.K05_Berhutang, "K05 - Berutang");
        TambahkanJikaBelumDiketahui(
            pesan, data.K08_HutangJatuhTempo,
            "K08 - Utang jatuh tempo");
        TambahkanJikaBelumDiketahui(
            pesan, data.K10_HutangBukanUntukMaksiat,
            "K10 - Tujuan utang bukan untuk maksiat atau pemborosan");

        // R4: Ibnu Sabil
        TambahkanJikaBelumDiketahui(
            pesan, data.K12_TidakMenetap, "K12 - Tidak menetap");
        TambahkanJikaBelumDiketahui(
            pesan, data.K14_PerjalananBukanUntukMaksiat,
            "K14 - Perjalanan bukan untuk kemaksiatan");

        pesan.Append(
            "\nLengkapi informasi melalui wawancara atau dokumen. " +
            "Jangan menebak fakta yang belum diketahui.");

        return pesan.ToString();
    }

    private static void TambahkanJikaBelumDiketahui(
        System.Text.StringBuilder hasil,
        StatusFakta fakta,
        string namaKriteria)
    {
        if (fakta == StatusFakta.BelumDiketahui)
        {
            hasil.AppendLine("- " + namaKriteria);
        }
    }

}
