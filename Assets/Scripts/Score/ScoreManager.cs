using System;
using UnityEngine;
using UnityEngine.SceneManagement;


/// Maneja el título de juego actualmente seleccionado y el puntaje
/// de la partida: cada vez que se pega un sticker en la hoja, evalúa
/// si comparte alguna categoría con el título actual y, si es así,
/// suma un punto.
///
/// Es un singleton persistente (igual que StickerCollectionManager)
/// para sobrevivir el cambio entre escenas (Catálogo → GamePlay → GameOver).

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    public GameDataSO CurrentGame { get; private set; }
    public int Score { get; private set; }

    ///Se dispara cada vez que el puntaje cambia (para actualizar UI).
    public event Action<int> OnScoreChanged;

    ///Cantidad TOTAL de stickers pegados (coincidan o no).
    public int TotalPlaced { get; private set; }

    ///Captura de pantalla de la hoja, tomada justo antes de ir a Game Over.
    public Texture2D CapturedScreenshot { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    /// Establece el título actual y reinicia el puntaje.
    /// Se llama al elegir un juego en el catálogo.
    /// </summary>
    public void SetCurrentGame(GameDataSO game)
    {
        CurrentGame = game;
        Score = 0;
        OnScoreChanged?.Invoke(Score);
    }

    /// Evalúa un sticker recién pegado en la hoja: cuenta el intento
    /// siempre, y suma un punto si comparte al menos una categoría
    /// con el título actual.
  
    public void EvaluateStickerPlacement(StickerData sticker)
    {
        if (CurrentGame == null || sticker == null) return;

        TotalPlaced++;

        bool matches = (sticker.categories & CurrentGame.associatedCategories) != 0;
        if (matches)
        {
            Score++;
        }

        OnScoreChanged?.Invoke(Score);
    }

    /// Porcentaje de acierto: aciertos ÷ total pegado.
    public float GetPercentage()
    {
        if (TotalPlaced <= 0) return 0f;
        return (Score / (float)TotalPlaced) * 100f;
    }

    /// Nota en sistema anglosajón A-F según el porcentaje.
    public char GetLetterGrade()
    {
        float pct = GetPercentage();

        if (pct >= 90f) return 'A';
        if (pct >= 80f) return 'B';
        if (pct >= 70f) return 'C';
        if (pct >= 60f) return 'D';
        return 'F';
    }

    /// Captura una foto de la pantalla actual (la hoja con los stickers
    /// pegados) y carga la escena de Game Over. Llamalo desde el botón
    /// "Calificar".
    
    public void SubmitAndGoToGameOver(string gameOverSceneName)
    {
        CapturedScreenshot = ScreenCapture.CaptureScreenshotAsTexture();
        SceneManager.LoadScene(gameOverSceneName);
    }
}
