using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;

    [Header("Audio Sources")]
    public AudioSource menuMusicSource;
    public AudioSource gameplayMusicSource;

    private void Awake()
    {
        // Робимо об'єкт незнищуваним між сценами
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void PlayMenuMusic()
    {
        gameplayMusicSource.Stop();

        if (!menuMusicSource.isPlaying)
            menuMusicSource.Play();
    }

    public void PlayGameplayMusic()
    {
        menuMusicSource.Stop();

        if (!gameplayMusicSource.isPlaying)
            gameplayMusicSource.Play();
    }
}

