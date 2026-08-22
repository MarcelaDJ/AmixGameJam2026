using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameEntryUI : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private Image gameIcon;
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private Button selectButton;

    private GameDataSO currentGameData;
    private CatalogueManager manager;

    public void Setup(GameDataSO data, CatalogueManager catalogueManager)
    {
        currentGameData = data;
        manager = catalogueManager;

        if (titleText != null) titleText.text = data.gameTitle;
        if (gameIcon != null && data.icon != null) gameIcon.sprite = data.icon;

        
        if (selectButton != null)
        {
            selectButton.onClick.RemoveAllListeners();
            selectButton.onClick.AddListener(OnGameSelected);
        }
    }

    private void OnGameSelected()
    {
        if (manager != null && currentGameData != null)
        {
            manager.OpenStickerInventoryForGame(currentGameData);
        }
    }
}