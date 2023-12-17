using UnityEngine;

public class DualSoundPlayer : MonoBehaviour
{
    public AudioSource[] audioSources;

    void Start()
    {
        audioSources = GetComponents<AudioSource>();
        PlaySounds();
    }

    void PlaySounds()
    {
        foreach (var source in audioSources)
        {
            if (source.clip != null)
            {
                source.Play();
            }
        }
    }
}