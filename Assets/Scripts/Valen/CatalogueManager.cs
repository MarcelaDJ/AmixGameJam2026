using System.Collections.Generic;
using UnityEngine;

public class CatalogueManager : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject catalogPanel;          
    [SerializeField] private GameObject stickerInventoryPanel; 

    [System.Serializable]
    public class GameCardBinding
    {
        [Tooltip("El GameCard ya armado a mano en la escena (con GameEntryUI agregado)")]
        public GameEntryUI entryUI;

        [Tooltip("El juego que le corresponde a esta tarjeta")]
        public GameDataSO gameData;
    }

    [Header("Tarjetas ya armadas en la escena (Group1Games + Group2Games)")]
    [SerializeField] private List<GameCardBinding> gameCards; 

    [Header("External Managers")]
    [SerializeField] private StickerCollectionManager stickerManager;

    private void Start()
    {
        PopulateCatalog();
    }

    /// Conecta cada GameCard ya existente en la escena con su GameDataSO
    /// correspondiente, llamando a Setup() para completar ícono, texto,
    /// y el listener del botón.

    private void PopulateCatalog()
    {
        if (gameCards == null) return;

        foreach (var binding in gameCards)
        {
            if (binding.entryUI == null || binding.gameData == null)
            {
                Debug.LogWarning("CatalogueManager: hay una tarjeta sin GameEntryUI o sin GameDataSO asignado.");
                continue;
            }

            binding.entryUI.Setup(binding.gameData, this);
        }
    }

    public void OpenStickerInventoryForGame(GameDataSO selectedGame)
    {
        
        if (catalogPanel != null) catalogPanel.SetActive(false);

        
        if (stickerInventoryPanel != null) stickerInventoryPanel.SetActive(true);

        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.SetCurrentGame(selectedGame);
        }

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