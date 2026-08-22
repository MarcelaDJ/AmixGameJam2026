using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Página izquierda del álbum: muestra en grande el sticker seleccionado.
/// No sabe nada de colección ni de slots, solo "mostrame este sticker".
/// </summary>
public class StickerZoomView : MonoBehaviour
{
    [SerializeField] private Image zoomImage;

    /// <summary>Muestra el sticker indicado en grande.</summary>
    public void Show(StickerData data)
    {
        Debug.Log("Show llamado con: " + (data != null ? data.displayName : "null"));
        if (data == null)
        {
            Clear();
            return;
        }

        zoomImage.enabled = true;
        zoomImage.sprite = data.icon;
    }

    /// <summary>Vacía la vista de zoom (por ejemplo, al cambiar de página).</summary>
    public void Clear()
    {
        zoomImage.enabled = false;
        zoomImage.sprite = null;
    }
}
