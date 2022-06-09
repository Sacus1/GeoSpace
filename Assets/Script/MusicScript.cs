using UnityEngine;

public class MusicScript : MonoBehaviour
{
    private static bool isActive = false;
    private AudioSource _audioSource;
    public AudioClip[] sound;
    public int currentSound = 1;
    private void Awake()
    {
        if (isActive) Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
        _audioSource = GetComponent<AudioSource>();
    }

    public void PlayMusic()
    {
        if (_audioSource.isPlaying) return;
        _audioSource.clip = sound[currentSound - 1];
        _audioSource.Play();
    }

    public void StopMusic()
    {
        _audioSource.Stop();
    }
    void Update()
    {
        if (!_audioSource.isPlaying)
        {
            currentSound = UnityEngine.Random.Range (1,sound.Length);
            PlayMusic();
        }
    }
}