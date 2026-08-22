using UnityEngine;

public class CatalogNavigation : MonoBehaviour
{
    [Header("UI Panels")]
    [SerializeField] private GameObject gamesCatalogPanel; 
    [SerializeField] private GameObject stickersPanel;    
   
    public void OpenStickersPanel()
    {
        if (gamesCatalogPanel != null) gamesCatalogPanel.SetActive(false);
        if (stickersPanel != null) stickersPanel.SetActive(true);
    }


    public void CloseCatalogToGame()
    {
        if (gamesCatalogPanel != null) gamesCatalogPanel.SetActive(false);
        if (stickersPanel != null) stickersPanel.SetActive(false);
    }

    
    public void OpenGamesCatalog()
    {
        if (stickersPanel != null) stickersPanel.SetActive(false);
        if (gamesCatalogPanel != null) gamesCatalogPanel.SetActive(true);
    }
}