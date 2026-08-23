using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneFlowManager : MonoBehaviour
{
    private void Start()
    {
        if (AudioManager.Instance != null)
        {
            string currentScene = SceneManager.GetActiveScene().name;

            if (currentScene == "01_MainMenu")
            {
                AudioManager.Instance.PlayMusic(AudioManager.Instance.menuMusic);
            }
            else if (currentScene == "02_GamePlay" || currentScene == "03_GameOver" || currentScene == "04_Winner")
            {
                AudioManager.Instance.PlayMusic(AudioManager.Instance.gameMusic);
            }
        }
    }

    public void LoadSceneByName(string sceneName)
    {
        // Reproduce el SFX de clic antes de cambiar de escena
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySFX(AudioManager.Instance.menuClick);
        }

        Time.timeScale = 1f; 
        SceneManager.LoadScene(sceneName);
    }

    public void LoadSceneByIndex(int sceneIndex)
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySFX(AudioManager.Instance.menuClick);
        }

        Time.timeScale = 1f;
        SceneManager.LoadScene(sceneIndex);
    }

    public void ReloadCurrentScene()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySFX(AudioManager.Instance.menuClick);
        }

        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitGame()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySFX(AudioManager.Instance.menuClick);
        }

        Debug.Log("Saliendo del juego...");
        Application.Quit();
    }
}