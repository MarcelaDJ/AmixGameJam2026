using UnityEngine;

public class CatalogNavigation : MonoBehaviour
{
    [Header("UI Panels")]
    [SerializeField] private GameObject gamesCatalogPanel;  
    [SerializeField] private GameObject stickerInventoryPanel; 
    [SerializeField] private GameObject invButton;           

    private void Start()
    {
        
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySFX(AudioManager.Instance.catalogoOpen);
        }
    }
    
    public void OpenGamesCatalog()
    {
        if (gamesCatalogPanel != null) gamesCatalogPanel.SetActive(true);
        if (stickerInventoryPanel != null) stickerInventoryPanel.SetActive(false);

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySFX(AudioManager.Instance.catalogoOpen);
        }
    }

    public void CloseAllPanels()
    {
        if (gamesCatalogPanel != null) gamesCatalogPanel.SetActive(false);
        if (stickerInventoryPanel != null) stickerInventoryPanel.SetActive(false);

        
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySFX(AudioManager.Instance.catalogoClose);
        }
    }

    public void SelectGameAndOpenStickers()
    {
        if (gamesCatalogPanel != null) gamesCatalogPanel.SetActive(false);
        if (stickerInventoryPanel != null) stickerInventoryPanel.SetActive(true);

        
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySFX(AudioManager.Instance.cajonOpen);
        }
    }
}