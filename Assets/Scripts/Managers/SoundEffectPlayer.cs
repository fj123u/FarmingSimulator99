using UnityEngine;
public class SoundEffectPlayer : MonoBehaviour
{
    [Range(0f, 1f)]
    public float effectsVolume = 0.8f;
    private AudioSource audioSource;
    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }
    public void PlaySound(AudioClip clip)
    {
        if (clip != null && audioSource != null)
        {
            audioSource.PlayOneShot(clip, effectsVolume);
        }
    }
}