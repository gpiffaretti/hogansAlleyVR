using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{
    [SerializeField]
    AudioSource effects2D;

    [SerializeField]
    AudioSource music;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Play2DSound(AudioClip clip)
    {
        effects2D.PlayOneShot(clip);
    }

    public void PlayMusic(AudioClip clip)
    {
        music.PlayOneShot(clip);
    }
}
