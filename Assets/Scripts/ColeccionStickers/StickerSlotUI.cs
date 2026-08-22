using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// Representa visualmente un único slot dentro de la grilla del álbum.
/// Va en el prefab del casillero (fondo + ícono).
/// No sabe nada de la colección completa, solo cómo pintarse a sí mismo.
public class StickerSlotUI : MonoBehaviour
{
    [Header("Referencias del prefab")]
    [SerializeField] private Image iconImage;
    [SerializeField] private Button button;
    [SerializeField] private TextMeshProUGUI countText;

    private StickerSlot boundSlot;
    private System.Action<StickerSlot> onClicked;

    private void Awake()
    {
        if (button != null)
            button.onClick.AddListener(HandleClick);
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
}
