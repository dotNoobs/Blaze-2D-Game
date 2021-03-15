using UnityEngine;

[System.Serializable]
public class Sound
{
    public enum AudioTypes { soundEffect, music }
    public AudioTypes audioType;

    [HideInInspector] public AudioSource source;
    public SoundEffectType clipName;
    public AudioClip audioClip;
    [SerializeField] bool isLoop;
    [SerializeField] bool playOnAwake;
    public SoundEffectType ClipName => clipName;
    public AudioClip AudioClip => audioClip;
    public bool IsLoop => isLoop;
    public bool PlayOnAwake => playOnAwake;
    public float Volume => volume;

    [Range(0, 1)]
    [SerializeField] float volume = 0.5f;
}
