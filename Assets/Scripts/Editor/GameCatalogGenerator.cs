using UnityEditor;
using UnityEngine;
using System.IO;

/// <summary>
/// Herramienta de Editor: genera automáticamente los 9 assets de
/// GameDataSO con sus categorías ya asignadas, según la lista definida
/// para el catálogo del juego. Se accede desde el menú
/// Tools > Stickers > Generar Catálogo de Juegos.
///
/// Este script va dentro de una carpeta llamada "Editor" en tu proyecto
/// (por ejemplo, Assets/Scripts/Editor/), porque usa UnityEditor y no
/// puede formar parte del build final del juego.
/// </summary>
public static class GameCatalogGenerator
{
    private const string OutputFolder = "Assets/GameCatalogue";

    [MenuItem("Tools/Stickers/Generar Catálogo de Juegos")]
    public static void GenerateCatalog()
    {
        if (!AssetDatabase.IsValidFolder(OutputFolder))
        {
            AssetDatabase.CreateFolder("Assets", "GameCatalogue");
        }

        CreateGame("Celeste",
            StickerCategory.Plataformas | StickerCategory.Emocional | StickerCategory.Naturaleza |
            StickerCategory.Velocidad | StickerCategory.Supervivencia | StickerCategory.Comida |
            StickerCategory.Exploracion);

        CreateGame("Halo",
            StickerCategory.SciFi | StickerCategory.Espacio | StickerCategory.Accion |
            StickerCategory.Combate | StickerCategory.Armas | StickerCategory.Vehiculos |
            StickerCategory.Supervivencia);

        CreateGame("Mecha Chameleon",
            StickerCategory.Mecanica | StickerCategory.Animales | StickerCategory.Color |
            StickerCategory.Accion | StickerCategory.Plataformas | StickerCategory.Naturaleza |
            StickerCategory.Combate);

        CreateGame("God of War",
            StickerCategory.Mitologia | StickerCategory.Poder | StickerCategory.Accion |
            StickerCategory.Combate | StickerCategory.Armas | StickerCategory.Emocional |
            StickerCategory.Fantasia);

        CreateGame("Age of Mythology",
            StickerCategory.Mitologia | StickerCategory.Poder | StickerCategory.Estrategia |
            StickerCategory.Gestion | StickerCategory.Construccion | StickerCategory.Fantasia |
            StickerCategory.Combate);

        CreateGame("Mario Kart",
            StickerCategory.Vehiculos | StickerCategory.Velocidad | StickerCategory.Animales |
            StickerCategory.Accion | StickerCategory.Color | StickerCategory.Magia |
            StickerCategory.Fantasia);

        CreateGame("Minecraft",
            StickerCategory.Construccion | StickerCategory.Supervivencia | StickerCategory.Exploracion |
            StickerCategory.Naturaleza | StickerCategory.Animales | StickerCategory.Combate |
            StickerCategory.Fantasia);

        CreateGame("Los Sims",
            StickerCategory.VidaCotidiana | StickerCategory.Gestion | StickerCategory.Construccion |
            StickerCategory.Emocional | StickerCategory.Color | StickerCategory.Comida |
            StickerCategory.Vehiculos);

        CreateGame("Stardew Valley",
            StickerCategory.VidaCotidiana | StickerCategory.Gestion | StickerCategory.Naturaleza |
            StickerCategory.Animales | StickerCategory.Comida | StickerCategory.Exploracion |
            StickerCategory.Emocional);

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log("Catálogo de juegos generado en " + OutputFolder);
    }

    private static void CreateGame(string title, StickerCategory categories)
    {
        string path = Path.Combine(OutputFolder, title.Replace(" ", "") + ".asset");

        // Si ya existe, lo actualizamos en vez de duplicarlo.
        var existing = AssetDatabase.LoadAssetAtPath<GameDataSO>(path);
        var game = existing != null ? existing : ScriptableObject.CreateInstance<GameDataSO>();

        game.gameTitle = title;
        game.associatedCategories = categories;

        if (existing == null)
        {
            AssetDatabase.CreateAsset(game, path);
        }
        else
        {
            EditorUtility.SetDirty(game);
        }
    }
}
