using UnityEngine;


/// Possible sticker categories. [Flags] allows a sticker to belong
/// to several categories at once (e.g. Animals | Space).

[System.Flags]
public enum StickerCategory
{
    None = 0,
    Accion = 1 << 0,
    Animales = 1 << 1,
    Armas = 1 << 2,
    Color = 1 << 3,
    Combate = 1 << 4,
    Comida = 1 << 5,
    Construccion = 1 << 6,
    Emocional = 1 << 7,
    Espacio = 1 << 8,
    Estrategia = 1 << 9,
    Exploracion = 1 << 10,
    Fantasia = 1 << 11,
    Gestion = 1 << 12,
    Magia = 1 << 13,
    Mitologia = 1 << 14,
    Naturaleza = 1 << 15,
    Plataformas = 1 << 16,
    Poder = 1 << 17,
    Mecanica = 1 << 18,
    SciFi = 1 << 19,
    Supervivencia = 1 << 20,
    Vehiculos = 1 << 21,
    Velocidad = 1 << 22,
    VidaCotidiana = 1 << 23
}


/// Fixed definition of a sticker. Created as an asset via
/// Assets > Create > Stickers > Sticker Data.
/// Does not change at runtime: it's the "catalog", not the player's progress.

[CreateAssetMenu(fileName = "NewSticker", menuName = "Stickers/Sticker Data")]
public class StickerData : ScriptableObject
{
    [Header("Identity")]
    public string stickerId;
    public string displayName;
    public Sprite icon;

    [Header("Gameplay properties")]
    public int points;

    [Header("Categories (multi-select)")]
    public StickerCategory categories;

    /// Checks whether this sticker belongs to a given category.
    public bool HasCategory(StickerCategory category)
    {
        return (categories & category) != 0;
    }
}
