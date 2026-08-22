using UnityEngine;

/// <summary>
/// Lives in the scene. Public entry point for other systems (a trigger,
/// a button, a minigame event) to add stickers to the player's collection.
///
/// Only owns the collection (ownership state). Placing stickers on boards
/// is handled by StickerBoard, which reads/reserves from this collection.
/// </summary>
public class StickerCollectionManager : MonoBehaviour
{
    [Tooltip("Current state of the player's sticker collection")]
    public StickerCollection collection = new StickerCollection();

    [Header("Quick test in the Inspector")]
    [SerializeField] private StickerData testSticker;
    [SerializeField] private int testAmount = 1;

    private void OnEnable()
    {
        collection.OnCollectionChanged += HandleCollectionChanged;
    }

    private void OnDisable()
    {
        collection.OnCollectionChanged -= HandleCollectionChanged;
    }

    /// <summary>Public entry point: call this from any other script.</summary>
    public void AddSticker(StickerData sticker, int amount = 1)
    {
        collection.AddSticker(sticker, amount);
    }

    private void HandleCollectionChanged()
    {
        Debug.Log($"[StickerCollection] Distinct: {collection.DistinctCount()} | Points: {collection.TotalPoints()}");
        // Hook up visual updates here (grid, album, on-screen counter, etc.)
    }

    // --- Quick testing from the Inspector via right-click ---
    [ContextMenu("Add test sticker")]
    private void AddTestStickerFromInspector()
    {
        if (testSticker == null)
        {
            Debug.LogWarning("Assign a StickerData in 'Test Sticker' before testing.");
            return;
        }
        AddSticker(testSticker, testAmount);
    }
}
