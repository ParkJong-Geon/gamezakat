
using System;
using UnityEngine;

[Serializable]
public class Scene3MustahikView
{
    [Header("Identitas NPC")]
    public string idNPC;

    [Header("Objek Rumah dan Karakter")]
    public GameObject rumah;
    public GameObject karakter;

    [Header("Data Distribusi")]
    [TextArea(2, 4)]
    public string kebutuhanMustahik;

    public string jenisBantuanSesuai;
}
