using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private AudioSource sfxAudio;

    public static AudioManager singleton;

    private void Awake()
    {
        singleton = this;
    }

    public void Play2DOneShot(AudioClip clip)
    {
        if (clip == null)
            return;

        sfxAudio.PlayOneShot(clip);
    }

    public void PlaySourceOneShot(AudioSource source, AudioClip clip)
    {
        if (clip == null)
            return;

        source.PlayOneShot(clip);
    }
}
