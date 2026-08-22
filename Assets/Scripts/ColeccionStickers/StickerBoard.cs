using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A board (e.g. a sheet of paper) where stickers from the collection
/// can be stuck at ANY position — free placement, not fixed slots.
/// Placing a sticker here reserves one unit from the collection;
/// removing it releases that unit back.
/// </summary>
public class StickerBoard : MonoBehaviour
{
    [Tooltip("Reference to the player's sticker collection (ownership source of truth)")]
    public StickerCollectionManager collectionManager;

    private readonly List<StickerPlacement> placements = new List<StickerPlacement>();

    public Action OnBoardChanged;

    /// <summary>
    /// Places a sticker freely at the given position. Fails if the
    /// collection has no available copies of that sticker.
    /// </summary>
    public StickerPlacement PlaceSticker(StickerData sticker, Vector2 position, float rotation = 0f)
    {
        if (sticker == null) return null;

        bool reserved = collectionManager.collection.TryReserve(sticker);
        if (!reserved)
        {
            Debug.LogWarning($"No available copies of '{sticker.displayName}' to place.");
            return null;
        }

        var placement = new StickerPlacement(sticker, position, rotation);
        placements.Add(placement);
        OnBoardChanged?.Invoke();
        return placement;
    }

    /// <summary>Removes a placed sticker from the board and returns it to the collection.</summary>
    public bool RemoveSticker(StickerPlacement placement)
    {
        if (placement == null || !placements.Remove(placement)) return false;

        collectionManager.collection.Release(placement.data);
        OnBoardChanged?.Invoke();
        return true;
    }

    /// <summary>Moves an already-placed sticker to a new position/rotation (e.g. dragging).</summary>
    public bool MoveSticker(StickerPlacement placement, Vector2 newPosition, float newRotation)
    {
        if (placement == null || !placements.Contains(placement)) return false;

        placement.position = newPosition;
        placement.rotation = newRotation;
        OnBoardChanged?.Invoke();
        return true;
    }

    public IReadOnlyList<StickerPlacement> GetAllPlacements() => placements;
}
