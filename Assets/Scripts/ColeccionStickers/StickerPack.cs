using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Una entrada posible dentro de un sobre: qué sticker es y qué probabilidad
/// relativa tiene de salir (a mayor weight, más chances).
/// </summary>
[System.Serializable]
public class StickerPackEntry
{
    public StickerData sticker;

    [Tooltip("Probabilidad relativa de que salga este sticker (no hace falta que sumen 100)")]
    [Range(0.01f, 100f)]
    public float weight = 1f;
}

/// <summary>
/// Define un tipo de sobre: cuántos stickers trae y de qué pool posible
/// se sortean. Se crea como asset desde Assets > Create > Stickers > Sticker Pack.
/// </summary>
[CreateAssetMenu(fileName = "NewPack", menuName = "Stickers/Sticker Pack")]
public class StickerPack : ScriptableObject
{
    public string packName;

    [Tooltip("Cuántos stickers salen al abrir este sobre")]
    public int stickersPerPack = 5;

    [Tooltip("Pool de stickers posibles y su probabilidad relativa")]
    public List<StickerPackEntry> possibleStickers = new List<StickerPackEntry>();
}
