using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// Configuración de una pestaña de página: qué botón la representa
/// y qué imagen de fondo corresponde a esa página del álbum.

[System.Serializable]
public class AlbumPageTab
{
    public Button tabButton;
    public Sprite backgroundSprite;
}

/// Orquestador visual de la página derecha del álbum (la grilla de slots)
/// y controla qué sticker se muestra en la vista de zoom (página izquierda).
///
/// Divide la colección en páginas de tamaño fijo (por defecto 9, como en
/// el boceto: una grilla de 3x3 por página). Cada página muestra un rango
/// consecutivo de slots de la colección.

public class StickerAlbumView : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private StickerCollectionManager collectionManager;
    [SerializeField] private StickerZoomView zoomView;
    [SerializeField] private Image backgroundImage;

    [Header("Grilla de slots")]
    [SerializeField] private Transform slotContainer;
    [SerializeField] private StickerSlotUI slotPrefab;

    [Header("Cierre del álbum al levantar un sticker (doble clic)")]
    [Tooltip("El panel del libro/álbum que se oculta al levantar un sticker, igual que el catálogo de juegos")]
    [SerializeField] private GameObject albumPanel;

    [Header("Configuración de paginación")]
    [SerializeField] private int slotsPerPage = 9;

    [Header("Pestañas de página (una por cada fondo distinto)")]
    [SerializeField] private List<AlbumPageTab> pageTabs = new List<AlbumPageTab>();


    [Header("Referencias para arrastrar hacia la hoja")]
    [SerializeField] private StickerBoard stickerBoard;
    [SerializeField] private RectTransform sheetRect;
    [SerializeField] private Transform dragCanvasRoot;

    private readonly List<StickerSlotUI> spawnedSlots = new List<StickerSlotUI>();
    private int currentPage = 0;


    private void Awake()
    {
        // Conectamos los clics de las pestañas una sola vez (en Awake, no en
        // OnEnable) para evitar suscribir el mismo listener varias veces si
        // el objeto se desactiva y reactiva.
        for (int i = 0; i < pageTabs.Count; i++)
        {
            int pageIndex = i; // captura local: evita el bug clásico de closures en loops
            if (pageTabs[i].tabButton != null)
                pageTabs[i].tabButton.onClick.AddListener(() => GoToPage(pageIndex));
        }
    }

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

    ///Crea (una sola vez) los slots visuales necesarios para una página
    private void BuildSlotPool()
    {
        if (spawnedSlots.Count > 0) return;

        for (int i = 0; i < slotsPerPage; i++)
        {
            var slotUI = Instantiate(slotPrefab, slotContainer);
            slotUI.ConfigureBoardReferences(stickerBoard, sheetRect, dragCanvasRoot, CloseAlbum);
            spawnedSlots.Add(slotUI);
        }
    }

    /// Cantidad total de páginas según la capacidad de la colección.
    public int GetTotalPages()
    {
        int capacity = collectionManager.collection.capacity;
        return Mathf.CeilToInt(capacity / (float)slotsPerPage);
    }

    /// Llamado por una pestaña: cambia de página Y de fondo a la vez.
    private void GoToPage(int pageIndex)
    {
        SetPage(pageIndex);
        ApplyBackgroundForPage(pageIndex);
    }

    /// Cambia a una página específica y refresca la grilla.
    public void SetPage(int pageIndex)
    {
        currentPage = Mathf.Clamp(pageIndex, 0, GetTotalPages() - 1);
        zoomView.Clear();
        RefreshCurrentPage();
    }

    ///Cambia el sprite de fondo según la pestaña/página correspondiente.
    private void ApplyBackgroundForPage(int pageIndex)
    {
        if (backgroundImage == null) return;
        if (pageIndex < 0 || pageIndex >= pageTabs.Count)
        {
            Debug.LogWarning($"No hay una pestaña/fondo configurado para la página {pageIndex}.");
            return;
        }

        backgroundImage.sprite = pageTabs[pageIndex].backgroundSprite;
    }

    public void NextPage() => SetPage(currentPage + 1);
    public void PreviousPage() => SetPage(currentPage - 1);

    /// Vuelve a pintar los slots visuales según el estado actual de la colección
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

    ///Cuando el jugador toca un slot ocupado, lo muestra en la vista de zoom.
    private void HandleSlotClicked(StickerSlot slot)
    {
        zoomView.Show(slot.data);
    }

    /// <summary>Cierra el panel del álbum, igual que el catálogo se cierra al elegir un juego.</summary>
    private void CloseAlbum()
    {
        if (albumPanel != null)
            albumPanel.SetActive(false);
    }
}
