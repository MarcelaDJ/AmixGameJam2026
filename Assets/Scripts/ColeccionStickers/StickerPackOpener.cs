using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Vive en la escena. Se encarga de abrir un StickerPack: sortea los
/// stickers según su probabilidad y los agrega inmediatamente a la
/// colección del jugador (se "pegan" directo al cuaderno).
/// </summary>
public class StickerPackOpener : MonoBehaviour
{
    [Tooltip("Referencia al manager de la colección del jugador")]
    public StickerCollectionManager collectionManager;

    [Header("Prueba rápida en el Inspector")]
    [SerializeField] private StickerPack testPack;

    /// <summary>
    /// Abre un sobre: sortea 'stickersPerPack' stickers según su probabilidad
    /// y los agrega a la colección. Devuelve la lista de resultados, útil
    /// para mostrar una animación de "revelado" en la UI.
    /// </summary>
    public List<StickerData> OpenPack(StickerPack pack)
    {
        var results = new List<StickerData>();

        if (pack == null || pack.possibleStickers.Count == 0)
        {
            Debug.LogWarning("El sobre no tiene stickers posibles configurados.");
            return results;
        }

        for (int i = 0; i < pack.stickersPerPack; i++)
        {
            StickerData picked = PickWeightedRandom(pack.possibleStickers);
            results.Add(picked);
            collectionManager.AddSticker(picked, 1);
        }

        return results;
    }

    /// <summary>Sortea un sticker respetando los pesos (weight) de cada entrada.</summary>
    private StickerData PickWeightedRandom(List<StickerPackEntry> entries)
    {
        float totalWeight = entries.Sum(e => e.weight);
        float roll = Random.Range(0f, totalWeight);
        float cumulative = 0f;

        foreach (var entry in entries)
        {
            cumulative += entry.weight;
            if (roll <= cumulative)
                return entry.sticker;
        }

        // Fallback por seguridad (no debería llegar acá salvo error de redondeo)
        return entries[entries.Count - 1].sticker;
    }

    // --- Solo para probar rápido desde el Inspector con clic derecho ---
    [ContextMenu("Abrir sobre de prueba")]
    private void AbrirSobreDePruebaDesdeInspector()
    {
        if (testPack == null)
        {
            Debug.LogWarning("Asigná un StickerPack en 'Test Pack' antes de probar.");
            return;
        }

        var obtenidos = OpenPack(testPack);
        Debug.Log("Sobre abierto, stickers obtenidos: " + string.Join(", ", obtenidos.Select(s => s.displayName)));
    }
}
