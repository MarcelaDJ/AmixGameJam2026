using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Orquestador visual de la hoja (StickerBoard). Escucha cuándo cambia
/// el tablero y redibuja los stickers pegados en sus posiciones libres.
/// Va en el mismo GameObject que StickerBoard (la hoja/SheetPanel).
/// </summary>
[RequireComponent(typeof(StickerBoard))]
public class StickerBoardView : MonoBehaviour
{
    [Tooltip("Prefab de un sticker pegado: un Image + PlacedStickerUI")]
    [SerializeField] private PlacedStickerUI placedStickerPrefab;

    private StickerBoard board;
    private RectTransform sheetRect;

    private void Awake()
    {
        board = GetComponent<StickerBoard>();
        sheetRect = GetComponent<RectTransform>();
    }

    private void OnEnable()
    {
        board.OnBoardChanged += Redraw;
        Redraw();
    }

    private void OnDisable()
    {
        board.OnBoardChanged -= Redraw;
    }

    /// <summary>
    /// Redibuja todos los stickers pegados desde cero. Simple y suficiente
    /// para la cantidad de stickers que maneja este proyecto.
    /// </summary>
    private void Redraw()
    {
        // Borramos las visuales previas
        foreach (Transform child in sheetRect)
            Destroy(child.gameObject);

        foreach (var placement in board.GetAllPlacements())
        {
            var instance = Instantiate(placedStickerPrefab, sheetRect);
            var icon = instance.GetComponent<Image>();
            instance.Setup(board, placement, sheetRect, icon);
        }
    }
}
