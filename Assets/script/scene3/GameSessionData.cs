
using System.Collections.Generic;
using UnityEngine;

public static class GameSessionData
{
    public static List<CalonMustahikData> mustahikDiterima =
        new List<CalonMustahikData>();

    public static void ResetData()
    {
        mustahikDiterima.Clear();

        Debug.Log("Data sesi permainan berhasil direset.");
    }

    public static void TambahkanMustahik(CalonMustahikData data)
    {
        if (data == null)
        {
            Debug.LogWarning("Data mustahik kosong.");
            return;
        }

        if (string.IsNullOrWhiteSpace(data.idNPC))
        {
            Debug.LogError(
                "ID NPC belum diisi untuk " + data.nama
            );
            return;
        }

        foreach (CalonMustahikData calon in mustahikDiterima)
        {
            if (calon.idNPC == data.idNPC)
            {
                Debug.LogWarning(
                    "Mustahik sudah tercatat: " + data.nama
                );
                return;
            }
        }

        mustahikDiterima.Add(data);

        Debug.Log(
            "Mustahik diterima: " + data.nama +
            " | ID: " + data.idNPC
        );
    }
}
