using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuzakkiGenerator : MonoBehaviour
{
    public MuzakkiData BuatHajiFaisal()
    {
        MuzakkiData data = new MuzakkiData();

        data.nama = "Haji Faisal";
        data.pekerjaan = "Petani";
        data.jenisHarta = JenisHarta.Gabah;

        // 655 - 1000 kg
        data.jumlahHarta = Random.Range(1640, 2501);

        data.teks1 =
            "Halo, perkenalkan nama saya Haji Faisal. " +
            "Saya bekerja sebagai petani.";

        data.teks2 =
            "Hasil panen gabah kering saya sebanyak " +
            data.jumlahHarta.ToString("0") +
            " kilogram dengan irigasi berbayar. Berapa kg gabah kering yang harus saya zakatkan?";

        data.jawabanZakat =
            data.jumlahHarta * 0.05f;

        return data;
    }

    public MuzakkiData BuatDhinita()
    {
        MuzakkiData data = new MuzakkiData();

        data.nama = "Dhinita";
        data.pekerjaan = "Ibu Rumah Tangga";
        data.jenisHarta = JenisHarta.Emas;

        // 100 - 300 gram
        data.jumlahHarta = Random.Range(100, 301);

        // Harga emas tetap
        data.hargaPerUnit = 1500000f;

        data.teks1 =
            "Halo, perkenalkan nama saya Dhinita. " +
            "Saya merupakan ibu rumah tangga.";

        data.teks2 =
            "Saya memiliki tabungan senilai " +
            data.jumlahHarta.ToString("0") +
            " gram emas. Berapakah yang harus saya zakati?";

        float nilaiEmas =
            data.jumlahHarta * data.hargaPerUnit;

        data.jawabanZakat =
            nilaiEmas * 0.025f;

        return data;
    }

    public MuzakkiData BuatBudi()
    {
        MuzakkiData data = new MuzakkiData();

        int jumlahKambing = Random.Range(40, 501);
        int zakatKambing;

        if(jumlahKambing <= 120){
            zakatKambing = 1;
        } else if (jumlahKambing <= 200){
            zakatKambing = 2;
        }else if (jumlahKambing <=399){
            zakatKambing = 3;
        }else{
            zakatKambing = 4+ ((jumlahKambing -400)/100);
        }
       return new MuzakkiData
        {
            nama = "Budi",
            pekerjaan = "Peternak",
            jenisHarta = JenisHarta.Kambing,
            jumlahHarta = jumlahKambing,
            jumlahKambingZakat = zakatKambing,
            jawabanZakat = zakatKambing,
            teks1 = "Halo, nama saya Budi. Saya seorang peternak kambing.",
            teks2 = "Saya memiliki " + jumlahKambing +
                    " ekor kambing. Berapa ekor kambing yang harus saya keluarkan sebagai zakat?"
        };
    }

    public MuzakkiData BuatSuzan()
    {
        MuzakkiData data = new MuzakkiData();

        data.nama = "Suzan";
        data.pekerjaan = "Pegawai BUMN";
        data.jenisHarta = JenisHarta.Gaji;

        // Rp10.000.000 - Rp15.000.000
        data.jumlahHarta =
            Random.Range(10_000_000, 15_000_001);

        data.teks1 =
            "Halo, nama saya Suzan. " +
            "Saya bekerja sebagai pegawai BUMN.";

        data.teks2 =
            "Gaji saya sebesar Rp" +
            data.jumlahHarta.ToString("N0") +
            " per bulan. " +
            "Berapakah zakat yang harus saya keluarkan?";

        data.jawabanZakat =
            data.jumlahHarta * 0.025f;

        return data;
    }

    public MuzakkiData BuatBangDodo()
    {
        MuzakkiData data = new MuzakkiData();

        data.nama = "Bang Dodo";
        data.pekerjaan = "Wiraswasta Toko Olahraga";
        data.jenisHarta = JenisHarta.Perdagangan;

        // Rp100.000.000 - Rp135.000.000
        data.jumlahHarta =
            Random.Range(100_000_000, 135_000_001);

        data.teks1 =
            "Halo, nama saya Bang Dodo. " +
            "Saya menjalankan usaha toko olahraga.";

        data.teks2 =
            "Pendapatan bersih toko saya tahun ini " +
            "sebesar Rp" +
            data.jumlahHarta.ToString("N0") +
            ". Berapakah zakat yang harus saya keluarkan?";

        data.jawabanZakat =
            data.jumlahHarta * 0.025f;

        return data;
    }
}