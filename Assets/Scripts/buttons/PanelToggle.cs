using UnityEngine;

/// <summary>
/// Alterna la visibilidad de un GameObject (por ejemplo, un Canvas)
/// cada vez que se llama a Toggle(). Pensado para conectar al OnClick
/// de un botón que abre y cierra el mismo panel.
/// </summary>
public class PanelToggle : MonoBehaviour
{
    [SerializeField] private GameObject targetPanel;

    public void Toggle()
    {
        targetPanel.SetActive(!targetPanel.activeSelf);
    }
}
