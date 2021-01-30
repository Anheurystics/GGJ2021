using System.Collections;
using UnityEngine;

public class AudioManager : MonoSingleton<AudioManager>
{
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioSource bgmSource;

    public void PlaySFX(AudioClip clip, float delay = 0)
    {
        StartCoroutine(_PlaySFX(clip, delay));
    }

    private IEnumerator _PlaySFX(AudioClip clip, float delay = 0)
    {
        yield return new WaitForSeconds(delay);
        sfxSource.PlayOneShot(clip);
    }
    
}
