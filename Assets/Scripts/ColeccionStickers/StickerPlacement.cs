using System;
using UnityEngine;

/// One sticker stuck freely onto a board, at an arbitrary position
/// (and optional rotation) — like a real sticker on a sheet of paper.
/// Lives inside StickerBoard, not inside the collection: the collection
/// only knows about ownership, not where things are placed.

[Serializable]
public class StickerPlacement
{
    public StickerData data;
    public Vector2 position;
    public float rotation;

    public StickerPlacement(StickerData data, Vector2 position, float rotation = 0f)
    {
        this.data = data;
        this.position = position;
        this.rotation = rotation;
    }
}
