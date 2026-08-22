using UnityEngine;


/// Possible sticker categories. [Flags] allows a sticker to belong
/// to several categories at once (e.g. Animals | Space).

[System.Flags]
public enum StickerCategory
{
    None = 0,
    Animals = 1 << 0,
    Nature = 1 << 1,
    Space = 1 << 2,
    Retro = 1 << 3,
    Fantasy = 1 << 4
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
