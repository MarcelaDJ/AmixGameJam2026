using UnityEngine;

public enum GameGenre { Acción, Aventura, Rol, Generos }

[CreateAssetMenu(fileName = "NewGameData", menuName = "Catalogue/Game Data")]
public class GameDataSO : ScriptableObject
{
    public string gameTitle;
    public GameGenre genre;
    public Sprite icon;
    public bool isUnlocked = false;
}