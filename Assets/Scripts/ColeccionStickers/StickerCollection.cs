using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// logic of the sticker collection, modeled as a fixed-size
/// inventory: a limited number of slots, filled in order as new
/// sticker types arrive. If a sticker type is already in a slot,
/// new copies simply stack onto it instead of taking a new slot.
///
/// This class only tracks OWNERSHIP (how many of each sticker you have,
/// and how many are free to place). It does NOT know about boards or
/// positions — that's handled by StickerBoard, which reserves/releases
/// units from here when placing/removing a sticker.

[Serializable]
public class StickerCollection
{
    [Tooltip("Total number of inventory slots available")]
    public int capacity = 20;

    public List<StickerSlot> slots = new List<StickerSlot>();

    ///Fired whenever the collection changes (to refresh UI, etc.)
    public event Action OnCollectionChanged;

    private void EnsureInitialized()
    {
        if (slots.Count == capacity) return;

        slots.Clear();
        for (int i = 0; i < capacity; i++)
            slots.Add(new StickerSlot());
    }

    /// Central entry point to add a sticker to the collection.
    /// Stacks onto an existing slot of the same type if present,
    /// otherwise takes the next empty slot. Returns false if the
    /// collection is full and the sticker is a new type.
   
    public bool AddSticker(StickerData newSticker, int amount = 1)
    {
        if (newSticker == null) return false;
        EnsureInitialized();

        var existingSlot = slots.Find(s => !s.IsEmpty && s.data == newSticker);
        if (existingSlot != null)
        {
            existingSlot.count += amount;
            OnCollectionChanged?.Invoke();
            return true;
        }

        var emptySlot = slots.Find(s => s.IsEmpty);
        if (emptySlot == null)
        {
            Debug.LogWarning("Collection is full — no empty slots available.");
            return false;
        }

        emptySlot.data = newSticker;
        emptySlot.count = amount;
        OnCollectionChanged?.Invoke();
        return true;
    }
    
    /// Reserves one available unit of this sticker (called by a board when
    /// the player places it). Returns false if there's none available.
    
    public bool TryReserve(StickerData sticker)
    {
        var slot = slots.Find(s => !s.IsEmpty && s.data == sticker);
        if (slot == null || slot.count <= 0) return false;

        slot.count--;
        OnCollectionChanged?.Invoke();
        return true;
    }

    
    /// Releases one unit back to the collection (called by a board when
    /// the player removes a sticker from it).
   
    public void Release(StickerData sticker)
    {
        var slot = slots.Find(s => !s.IsEmpty && s.data == sticker);
        if (slot != null)
        {
            slot.count++;
            OnCollectionChanged?.Invoke();
            return;
        }

        // Edge case: the slot for this type no longer exists (e.g. it was
        // somehow cleared). Put it back into any empty slot if possible.
        var emptySlot = slots.Find(s => s.IsEmpty);
        if (emptySlot != null)
        {
            emptySlot.data = sticker;
            emptySlot.count = 1;
            OnCollectionChanged?.Invoke();
        }
        else
        {
            Debug.LogWarning($"Could not return '{sticker.displayName}' to the collection — no empty slots.");
        }
    }

    ///How many empty slots are left
    public int EmptySlotCount()
    {
        EnsureInitialized();
        return slots.Count(s => s.IsEmpty);
    }

    /// Returns every non-empty slot belonging to a given category.
    public List<StickerSlot> GetByCategory(StickerCategory category)
    {
        return slots.Where(s => !s.IsEmpty && s.data.HasCategory(category)).ToList();
    }

    ///Checks whether a specific sticker is owned at all
    public bool Owns(StickerData sticker)
    {
        return slots.Any(s => !s.IsEmpty && s.data == sticker);
    }

    /// Total points across the whole collection (points x count).
    public int TotalPoints()
    {
        return slots.Where(s => !s.IsEmpty).Sum(s => s.data.points * s.count);
    }

    /// Number of distinct sticker types currently occupying a slot.
    public int DistinctCount()
    {
        return slots.Count(s => !s.IsEmpty);
    }
}
