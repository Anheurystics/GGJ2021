using System.Collections;
using UnityEngine;

public class AudioManager : MonoSingleton<AudioManager>
{
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioSource bgmSource;
    [SerializeField] private AudioClip bgmMenu;
    [SerializeField] private AudioClip bgmGame;

    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void PlaySFX(AudioClip clip, float delay = 0)
    {
        StartCoroutine(_PlaySFX(clip, delay));
    }

    public void PlayBGMGame()
    {
        bgmSource.clip = bgmGame;
        bgmSource.Play();
    }

    public void PlayBGMMenu()
    {
        bgmSource.clip = bgmMenu;
        bgmSource.Play();
    }

    private IEnumerator _PlaySFX(AudioClip clip, float delay = 0)
    {
        yield return new WaitForSeconds(delay);
        sfxSource.PlayOneShot(clip);
    }
    
}
