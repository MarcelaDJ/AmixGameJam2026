using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Audio Sources")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;

    [Header("Música")]
    public AudioClip menuMusic;
    public AudioClip gameMusic;

    [Header("Efectos UI & Paneles")]
    public AudioClip menuClick;
    public AudioClip menuHover;
    public AudioClip cajonOpen;
    public AudioClip cajonClose;
    public AudioClip catalogoOpen;
    public AudioClip catalogoClose;

    [Header("Efectos Feedback & Gameplay")]
    public AudioClip puntosWin;
    public AudioClip puntosLose;

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

    public void PlayMusic(AudioClip clip)
    {
        if (clip == null) return;
        if (musicSource.clip == clip && musicSource.isPlaying) return;

        musicSource.clip = clip;
        musicSource.loop = true;
        musicSource.Play();
    }

    
    public void PlaySFX(AudioClip clip, float volume = 1f)
    {
        if (clip != null && sfxSource != null)
        {
            sfxSource.PlayOneShot(clip, volume);
        }
    }
}