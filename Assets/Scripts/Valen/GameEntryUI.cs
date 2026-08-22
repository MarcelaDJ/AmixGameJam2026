using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameEntryUI : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private Image gameIcon;
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private GameObject lockedOverlay;
    [SerializeField] private TextMeshProUGUI lockedText;

    public void Setup(GameDataSO data)
    {
        if (data.isUnlocked)
        {
            if (gameIcon != null) gameIcon.sprite = data.icon;
            if (titleText != null) titleText.text = data.gameTitle;
            if (lockedOverlay != null) lockedOverlay.SetActive(false);
        }
        else
        {
            if (titleText != null) titleText.text = "???";
            if (lockedOverlay != null) lockedOverlay.SetActive(true);
            if (lockedText != null) lockedText.text = "Bloqueado";
        }
    }
}