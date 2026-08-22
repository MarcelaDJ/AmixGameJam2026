using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Orquestador visual de la página derecha del álbum (la grilla de slots)
/// y controla qué sticker se muestra en la vista de zoom (página izquierda).
///
/// Divide la colección en páginas de tamaño fijo (por defecto 9, como en
/// el boceto: una grilla de 3x3 por página). Cada página muestra un rango
/// consecutivo de slots de la colección.
/// </summary>
public class StickerAlbumView : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private StickerCollectionManager collectionManager;
    [SerializeField] private StickerZoomView zoomView;

    [Header("Grilla de slots")]
    [SerializeField] private Transform slotContainer;
    [SerializeField] private StickerSlotUI slotPrefab;

    [Header("Configuración de paginación")]
    [SerializeField] private int slotsPerPage = 9;

    private readonly List<StickerSlotUI> spawnedSlots = new List<StickerSlotUI>();
    private int currentPage = 0;

    private void OnEnable()
    {
        collectionManager.collection.OnCollectionChanged += RefreshCurrentPage;
        BuildSlotPool();
        RefreshCurrentPage();
        zoomView.Clear(); // arranca vacío hasta que se haga clic en un sticker
    }

    private void OnDisable()
    {
        collectionManager.collection.OnCollectionChanged -= RefreshCurrentPage;
    }

    /// <summary>Crea (una sola vez) los slots visuales necesarios para una página.</summary>
    private void BuildSlotPool()
    {
        if (spawnedSlots.Count > 0) return;

        for (int i = 0; i < slotsPerPage; i++)
        {
            var slotUI = Instantiate(slotPrefab, slotContainer);
            spawnedSlots.Add(slotUI);
        }
    }

    /// <summary>Cantidad total de páginas según la capacidad de la colección.</summary>
    public int GetTotalPages()
    {
        int capacity = collectionManager.collection.capacity;
        return Mathf.CeilToInt(capacity / (float)slotsPerPage);
    }

    /// <summary>Cambia a una página específica y refresca la grilla.</summary>
    public void SetPage(int pageIndex)
    {
        currentPage = Mathf.Clamp(pageIndex, 0, GetTotalPages() - 1);
        zoomView.Clear();
        RefreshCurrentPage();
    }

    public void NextPage() => SetPage(currentPage + 1);
    public void PreviousPage() => SetPage(currentPage - 1);

    /// <summary>Vuelve a pintar los slots visuales según el estado actual de la colección.</summary>
    private void RefreshCurrentPage()
    {
        var slots = collectionManager.collection.slots;
        int startIndex = currentPage * slotsPerPage;

        for (int i = 0; i < spawnedSlots.Count; i++)
        {
            int slotIndex = startIndex + i;
            StickerSlot slotData = slotIndex < slots.Count ? slots[slotIndex] : null;

            spawnedSlots[i].Setup(slotData, HandleSlotClicked);
        }
    }

    /// <summary>Cuando el jugador toca un slot ocupado, lo muestra en la vista de zoom.</summary>
    private void HandleSlotClicked(StickerSlot slot)
    {
        zoomView.Show(slot.data);
    }
}
