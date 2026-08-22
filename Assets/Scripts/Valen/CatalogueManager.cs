using System.Collections.Generic;
using UnityEngine;

public class CatalogueManager : MonoBehaviour
{
    [Header("Data & Prefabs")]
    [SerializeField] private List<GameDataSO> allGames;
    [SerializeField] private GameObject gameEntryPrefab;
    [SerializeField] private Transform gridContainer; 

    private List<GameObject> spawnedEntries = new List<GameObject>();

    private void Start()
    {
       
        FilterByGenre((int)GameGenre.Acción);
    }

    public void FilterByGenre(int genreIndex)
    {
        GameGenre selectedGenre = (GameGenre)genreIndex;
        ClearGrid();

        foreach (GameDataSO game in allGames)
        {
            if (game.genre == selectedGenre)
            {
                GameObject newEntry = Instantiate(gameEntryPrefab, gridContainer);
                GameEntryUI entryScript = newEntry.GetComponent<GameEntryUI>();
                if (entryScript != null)
                {
                    entryScript.Setup(game);
                }
                spawnedEntries.Add(newEntry);
            }
        }
    }

    private void ClearGrid()
    {
        foreach (GameObject entry in spawnedEntries)
        {
            Destroy(entry);
        }
        spawnedEntries.Clear();
    }
}