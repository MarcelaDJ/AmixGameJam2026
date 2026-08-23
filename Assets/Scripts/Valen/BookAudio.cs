using UnityEngine;

public class BookAudio : MonoBehaviour
{
    private bool isFirstEnable = true;

    private void OnEnable()
    {
        
        if (isFirstEnable)
        {
            isFirstEnable = false;
            return;
        }

        
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySFX(AudioManager.Instance.catalogoOpen);
        }
    }
}