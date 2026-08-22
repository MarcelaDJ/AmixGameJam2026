using UnityEngine;

public class CatalogNavigation : MonoBehaviour
{
    [Header("UI Panels")]
    [SerializeField] private GameObject gamesCatalogPanel;  
    [SerializeField] private GameObject stickerInventoryPanel; 
    [SerializeField] private GameObject invButton;           

   
    public void OpenGamesCatalog()
    {
        if (gamesCatalogPanel != null) gamesCatalogPanel.SetActive(true);
        if (stickerInventoryPanel != null) stickerInventoryPanel.SetActive(false);
    }

    
    public void CloseAllPanels()
    {
        if (gamesCatalogPanel != null) gamesCatalogPanel.SetActive(false);
        if (stickerInventoryPanel != null) stickerInventoryPanel.SetActive(false);
    }

    
    public void SelectGameAndOpenStickers()
    {
        if (gamesCatalogPanel != null) gamesCatalogPanel.SetActive(false);
        if (stickerInventoryPanel != null) stickerInventoryPanel.SetActive(true);
    }
}