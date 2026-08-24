using System;
using UnityEngine;

/// <summary>
/// Maneja el título de juego actualmente seleccionado y el puntaje
/// de la partida: cada vez que se pega un sticker en la hoja, evalúa
/// si comparte alguna categoría con el título actual y, si es así,
/// suma un punto.
///
/// Es un singleton persistente (igual que StickerCollectionManager)
/// para sobrevivir el cambio entre escenas (Catálogo → GamePlay → GameOver).
/// </summary>
public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    public GameDataSO CurrentGame { get; private set; }
    public int Score { get; private set; }

    /// <summary>Se dispara cada vez que el puntaje cambia (para actualizar UI).</summary>
    public event Action<int> OnScoreChanged;

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

    /// <summary>
    /// Evalúa un sticker recién pegado en la hoja: si comparte al menos
    /// una categoría con el título actual, suma un punto.
    /// </summary>
    public void EvaluateStickerPlacement(StickerData sticker)
    {
        if (CurrentGame == null || sticker == null) return;

        bool matches = (sticker.categories & CurrentGame.associatedCategories) != 0;
        if (matches)
        {
            Score++;
            OnScoreChanged?.Invoke(Score);
        }
    }
}
