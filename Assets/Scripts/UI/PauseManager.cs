using UnityEngine;
using UnityEngine.InputSystem; 

public class PauseManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject pauseMenuUI;

    public static bool IsPaused { get; private set; } = false;

    private void Start()
    {
        Resume();
    }

    private void Update()
    {
       
        if (Keyboard.current != null && Keyboard.current.pKey.wasPressedThisFrame)
        {
            if (IsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        if (pauseMenuUI != null)
        {
            pauseMenuUI.SetActive(false);
        }
        
        Time.timeScale = 1f;
        IsPaused = false;
    }

    public void Pause()
    {
        if (pauseMenuUI != null)
        {
            pauseMenuUI.SetActive(true);
        }

        Time.timeScale = 0f;
        IsPaused = true;
    }
}