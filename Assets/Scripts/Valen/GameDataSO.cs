using UnityEngine;

public enum GameGenre { Acción, Aventura, Rol, Generos }

[CreateAssetMenu(fileName = "NewGameData", menuName = "Catalogue/Game Data")]
public class GameDataSO : ScriptableObject
{
    public string gameTitle;
    public GameGenre genre;
    public Sprite icon;
    public bool isUnlocked = false;

    [Header("Categorías que puntúan para este título")]
    [Tooltip("Un sticker suma punto si comparte al menos una de estas categorías")]
    public StickerCategory associatedCategories;
}