using UnityEngine;
using UnityEngine.UI;

/// Página izquierda del álbum: muestra en grande el sticker seleccionado.

public class StickerZoomView : MonoBehaviour
{
    [SerializeField] private Image zoomImage;

    /// Muestra el sticker indicado en grande.
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

    /// Vacía la vista de zoom 
    public void Clear()
    {
        zoomImage.enabled = false;
        zoomImage.sprite = null;
    }
}
