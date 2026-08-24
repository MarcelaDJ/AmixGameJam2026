using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// Representa visualmente un único slot dentro de la grilla del álbum.
/// Va en el prefab del casillero (fondo + ícono).
/// No sabe nada de la colección completa, solo cómo pintarse a sí mismo.
public class StickerSlotUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
{
    [Header("Referencias del prefab")]
    [SerializeField] private Image iconImage;
    [SerializeField] private Button button;
    [SerializeField] private TextMeshProUGUI countText;

    private StickerBoard stickerBoard;
    private RectTransform sheetRect;
    private Transform dragCanvasRoot;
    private StickerSlot boundSlot;
    private System.Action<StickerSlot> onClicked;
    private StickerDragGhost activeGhost;
    private System.Action onPickedUp; // se llama al levantar el sticker con doble clic (para cerrar el álbum)

    private void Awake()
    {
        if (button != null)
            button.onClick.AddListener(HandleClick);
        gameObject.tag = "Sticker";
    }

    /// Configura las referencias de escena necesarias para poder arrastrar
    /// hacia la hoja. Se llama una sola vez, justo después de instanciar
    /// este slot (ver StickerAlbumView.BuildSlotPool).

    public void ConfigureBoardReferences(StickerBoard board, RectTransform sheet, Transform canvasRoot, System.Action onPickedUpCallback)
    {
        stickerBoard = board;
        sheetRect = sheet;
        dragCanvasRoot = canvasRoot;
        onPickedUp = onPickedUpCallback;
    }

    /// Configura este slot visual con los datos de un StickerSlot real.
    /// Si el slot está vacío, se muestra sin ícono (casillero vacío).

    public void Setup(StickerSlot slot, System.Action<StickerSlot> onClickedCallback)
    {
        boundSlot = slot;
        onClicked = onClickedCallback;

        bool isEmpty = slot == null || slot.IsEmpty;

        iconImage.enabled = !isEmpty;
        iconImage.sprite = isEmpty ? null : slot.data.icon;

        if (countText != null)
        {
            bool showCount = !isEmpty && slot.count > 1;
            countText.enabled = showCount;
            countText.text = showCount ? $"x{slot.count}" : string.Empty;
        }
    }

    private void HandleClick()
    {
        Debug.Log("Click detectado en el slot");
        if (boundSlot == null || boundSlot.IsEmpty) return;
        onClicked?.Invoke(boundSlot);
    }

    ///Detecta el doble clic 
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.clickCount != 2) return;
        if (boundSlot == null || boundSlot.IsEmpty || boundSlot.count <= 0) return;
        if (stickerBoard == null) return;

        bool reserved = stickerBoard.collectionManager.collection.TryReserve(boundSlot.data);
        if (!reserved) return;

        StickerCursorCarrier.Instance?.StartCarrying(boundSlot.data);
        onPickedUp?.Invoke();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        activeGhost = null;

        if (boundSlot == null || boundSlot.IsEmpty) return;
        if (boundSlot.count <= 0) return; // no quedan copias disponibles para pegar
        if (stickerBoard == null || sheetRect == null || dragCanvasRoot == null) return; // no configurado todavía

        activeGhost = StickerDragGhost.Create(
            boundSlot.data, stickerBoard, sheetRect, dragCanvasRoot,
            eventData.position, boundSlot.data.icon);
    }

    public void OnDrag(PointerEventData eventData)
    {
        activeGhost?.FollowPointer(eventData.position);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        activeGhost?.ReleaseAt(eventData);
        activeGhost = null;
    }

  
}
