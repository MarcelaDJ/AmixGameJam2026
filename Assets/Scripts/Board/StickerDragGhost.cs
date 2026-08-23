using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// Objeto temporal que sigue al mouse mientras se arrastra un sticker
/// desde un slot del álbum hacia la hoja (StickerBoard).
///
/// Importante: este objeto NO recibe eventos de drag directamente del
/// EventSystem (Unity solo se los manda al objeto que inició el arrastre).
/// Por eso expone métodos públicos (FollowPointer / ReleaseAt) para que
/// el StickerSlotUI que lo creó le reenvíe manualmente los eventos.
/// </summary>
public class StickerDragGhost : MonoBehaviour
{
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;

    private StickerData data;
    private StickerBoard board;
    private RectTransform sheetRect;

    /// <summary>Crea y configura un ghost en tiempo de ejecución.</summary>
    public static StickerDragGhost Create(StickerData data, StickerBoard board, RectTransform sheetRect,
        Transform canvasRoot, Vector2 startScreenPosition, Sprite icon)
    {
        var go = new GameObject("StickerDragGhost", typeof(RectTransform), typeof(Image), typeof(CanvasGroup));
        go.transform.SetParent(canvasRoot, worldPositionStays: false);
        go.transform.SetAsLastSibling();

        var image = go.GetComponent<Image>();
        image.sprite = icon;
        image.raycastTarget = false; // el ghost no debe bloquear su propia detección de drop

        var rect = go.GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(80, 80); // tamaño del ícono mientras se arrastra
        rect.position = startScreenPosition;

        var ghost = go.AddComponent<StickerDragGhost>();
        ghost.data = data;
        ghost.board = board;
        ghost.sheetRect = sheetRect;
        ghost.rectTransform = rect;
        ghost.canvasGroup = go.GetComponent<CanvasGroup>();
        ghost.canvasGroup.alpha = 0.8f;
        ghost.canvasGroup.blocksRaycasts = false;

        return ghost;
    }

    /// <summary>Llamado por el slot en su propio OnDrag, para mover el ghost.</summary>
    public void FollowPointer(Vector2 screenPosition)
    {
        rectTransform.position = screenPosition;
    }

    /// <summary>Llamado por el slot en su propio OnEndDrag, para soltar el ghost.</summary>
    public void ReleaseAt(PointerEventData eventData)
    {
        bool insideSheet = RectTransformUtility.RectangleContainsScreenPoint(
            sheetRect, eventData.position, eventData.pressEventCamera);

        if (insideSheet)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                sheetRect, eventData.position, eventData.pressEventCamera, out Vector2 localPoint);

            StickerPlacement placement = board.PlaceSticker(data, localPoint);
            if (placement == null)
                Debug.LogWarning($"No se pudo pegar '{data.displayName}': no quedan copias disponibles.");
        }
        // Si cayó fuera de la hoja, simplemente se cancela (no se reservó nada todavía).

        Destroy(gameObject);
    }
}
