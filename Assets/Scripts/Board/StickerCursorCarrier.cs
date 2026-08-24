using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

/// <summary>
/// Singleton que "carga" un sticker levantado del álbum (doble clic) y lo
/// hace seguir al cursor hasta que el jugador hace clic sobre la hoja
/// para pegarlo. Si el sticker ya fue reservado (restado de la colección)
/// al levantarlo, este componente solo se encarga de la parte visual y
/// de la colocación final — no vuelve a tocar la colección salvo que se
/// cancele la carga.
/// </summary>
public class StickerCursorCarrier : MonoBehaviour
{
    public static StickerCursorCarrier Instance { get; private set; }

    [SerializeField] private Image carriedImage;
    [SerializeField] private StickerBoard board;
    [SerializeField] private RectTransform sheetRect;

    private StickerData carriedData;
    private bool isCarrying;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        if (carriedImage != null) carriedImage.enabled = false;
    }

    /// <summary>Empieza a cargar un sticker (ya reservado previamente) para pegarlo con el próximo clic.</summary>
    public void StartCarrying(StickerData data)
    {
        carriedData = data;
        isCarrying = true;

        carriedImage.enabled = true;
        carriedImage.sprite = data.icon;
    }

    /// <summary>Cancela la carga y devuelve el sticker a la colección (por si hace falta, ej: tecla Escape).</summary>
    public void CancelCarrying()
    {
        if (!isCarrying) return;

        board.collectionManager.collection.Release(carriedData);
        StopCarrying();
    }

    private void Update()
    {
        if (!isCarrying || Mouse.current == null) return;

        Vector2 mousePos = Mouse.current.position.ReadValue();
        carriedImage.rectTransform.position = mousePos;

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            TryPlaceAtCursor(mousePos);
        }
    }

    private void TryPlaceAtCursor(Vector2 screenPosition)
    {
        bool insideSheet = RectTransformUtility.RectangleContainsScreenPoint(sheetRect, screenPosition, null);
        if (!insideSheet) return; // seguimos cargando hasta que clickee dentro de la hoja

        RectTransformUtility.ScreenPointToLocalPointInRectangle(sheetRect, screenPosition, null, out Vector2 localPoint);
        board.PlaceAlreadyReservedSticker(carriedData, localPoint);
        StopCarrying();
    }

    private void StopCarrying()
    {
        isCarrying = false;
        carriedImage.enabled = false;
        carriedData = null;
    }
}
