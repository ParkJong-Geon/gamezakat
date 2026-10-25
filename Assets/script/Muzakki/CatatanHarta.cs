using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Globalization;

public class CatatanHarta : MonoBehaviour
{

    public GameObject panel;
    
    public TMP_Text namaNPCText;
    public TMP_Text jenisHartaText;
    public TMP_Text jumlahHartaText;

    private readonly CultureInfo budaya = new CultureInfo("id-ID");

    public void TampilkanCatatan(MuzakkiData data){
        if( data == null){
            SembunyikanCatatan();
            return;
        }

        panel.SetActive(true);
        namaNPCText.text = data.nama;

        switch(data.jenisHarta){
            case JenisHarta.Gabah:
            jenisHartaText.text = "Jenis: Gabah Kering";
            jumlahHartaText.text = data.jumlahHarta.ToString("N0", budaya) + " kg";
            break;
            case JenisHarta.Emas:
            jenisHartaText.text = "Jenis: Emas";
            jumlahHartaText.text = data.jumlahHarta.ToString("N0", budaya) + " gram";
            break;
            case JenisHarta.Kambing:
            jenisHartaText.text = "Jenis: Ternak Kambing";
            jumlahHartaText.text = data.jumlahHarta.ToString("N0", budaya) + " ekor";
            break;
            case JenisHarta.Gaji:
            jenisHartaText.text = "Jenis: Gaji (Rupiah)";
            jumlahHartaText.text = data.jumlahHarta.ToString("N0", budaya) + " rupiah";
            break;
            case JenisHarta.Perdagangan:
            jenisHartaText.text = "Jenis: Penghasilan (Rupiah)";
            jumlahHartaText.text = data.jumlahHarta.ToString("N0", budaya) + " rupiah";
            break;
        }
    }

    public void SembunyikanCatatan(){
        panel.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
