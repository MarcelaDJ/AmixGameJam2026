using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// Representa visualmente un sticker ya pegado en la hoja (StickerBoard).
/// Se puede volver a arrastrar para reacomodarlo dentro de la hoja,
/// o sacarlo del tablero arrastrándolo fuera de sus límites (se devuelve
/// a la colección automáticamente).
/// </summary>
public class PlacedStickerUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;

    private StickerBoard board;
    private StickerPlacement placement;
    private RectTransform sheetRect;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null) canvasGroup = gameObject.AddComponent<CanvasGroup>();

        gameObject.tag = "Sticker";
    }

    /// <summary>Configura este sticker visual con sus datos y dependencias.</summary>
    public void Setup(StickerBoard board, StickerPlacement placement, RectTransform sheetRect, Image iconImage)
    {
        this.board = board;
        this.placement = placement;
        this.sheetRect = sheetRect;

        iconImage.sprite = placement.data.icon;
        rectTransform.anchoredPosition = placement.position;
        rectTransform.localRotation = Quaternion.Euler(0, 0, placement.rotation);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        transform.SetAsLastSibling();
        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = 0.8f;
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / rectTransform.lossyScale.x;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 1.0f;

        bool insideSheet = RectTransformUtility.RectangleContainsScreenPoint(
            sheetRect, eventData.position, eventData.pressEventCamera);

        if (insideSheet)
        {
            // Sigue dentro de la hoja: guardamos la nueva posición.
            board.MoveSticker(placement, rectTransform.anchoredPosition, rectTransform.localEulerAngles.z);
        }
        else
        {
            // Se arrastró fuera de la hoja: se saca del tablero y vuelve a la colección.
            board.RemoveSticker(placement);
            Destroy(gameObject);
        }
    }
}
