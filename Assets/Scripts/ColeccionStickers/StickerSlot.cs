using System;


/// One inventory-style slot in the collection. Empty when data is null.
/// Slots are filled in order as new sticker types arrive; if a sticker
/// type is already assigned to a slot, new copies just stack on it.

[Serializable]
public class StickerSlot
{
    public StickerData data;
    public int count;

    public bool IsEmpty => data == null;
}
