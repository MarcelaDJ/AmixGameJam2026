using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Muestra el resultado final en la escena de Game Over: la foto de la
/// hoja con los stickers pegados, el nombre del juego, y la imagen de
/// la nota (A-F) correspondiente al porcentaje de acierto.
/// </summary>
public class GameOverDisplay : MonoBehaviour
{
    [Serializable]
    public class GradeSpriteEntry
    {
        public char grade; // 'A', 'B', 'C', 'D', 'F'
        public Sprite sprite;
    }

    [Header("Referencias UI")]
    [SerializeField] private RawImage boardScreenshotImage;
    [SerializeField] private TextMeshProUGUI gameTitleText;
    [SerializeField] private Image letterGradeImage;

    [Header("Imágenes por nota (una por cada letra A-F)")]
    [SerializeField] private List<GradeSpriteEntry> gradeSprites;

    private void Start()
    {
        if (ScoreManager.Instance == null)
        {
            Debug.LogWarning("GameOverDisplay: no hay ScoreManager en la escena.");
            return;
        }

        DisplayScreenshot();
        DisplayGameTitle();
        DisplayGrade();
    }

    private void DisplayScreenshot()
    {
        var screenshot = ScoreManager.Instance.CapturedScreenshot;
        if (screenshot != null && boardScreenshotImage != null)
        {
            boardScreenshotImage.texture = screenshot;
        }
    }

    private void DisplayGameTitle()
    {
        var game = ScoreManager.Instance.CurrentGame;
        if (game != null && gameTitleText != null)
        {
            gameTitleText.text = game.gameTitle;
        }
    }

    private void DisplayGrade()
    {
        char grade = ScoreManager.Instance.GetLetterGrade();
        var entry = gradeSprites.FirstOrDefault(g => g.grade == grade);

        if (entry != null && letterGradeImage != null)
        {
            letterGradeImage.sprite = entry.sprite;
        }
        else
        {
            Debug.LogWarning($"No hay sprite configurado para la nota '{grade}'.");
        }
    }
}
