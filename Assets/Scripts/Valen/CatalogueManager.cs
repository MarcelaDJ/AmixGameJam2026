using System.Collections.Generic;
using UnityEngine;

public class CatalogueManager : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject catalogPanel;          
    [SerializeField] private GameObject stickerInventoryPanel; 

    [Header("Data & Prefabs")]
    [SerializeField] private List<GameDataSO> allGames;
    [SerializeField] private GameObject gameEntryPrefab;
    [SerializeField] private Transform gridContainer;

    [Header("External Managers")]
    [SerializeField] private StickerCollectionManager stickerManager; 

    public void OpenStickerInventoryForGame(GameDataSO selectedGame)
    {
        
        if (catalogPanel != null) catalogPanel.SetActive(false);

        
        if (stickerInventoryPanel != null) stickerInventoryPanel.SetActive(true);

        
        if (stickerManager != null)
        {
            
        }
    }

    
    public void BackToCatalog()
    {
        if (stickerInventoryPanel != null) stickerInventoryPanel.SetActive(false);
        if (catalogPanel != null) catalogPanel.SetActive(true);
    }
}