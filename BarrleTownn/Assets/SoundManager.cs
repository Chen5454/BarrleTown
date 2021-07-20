using UnityEngine.Audio;
using UnityEngine;
using System;

public class SoundManager : MonoBehaviour
{
    public Sound[] sounds;
    public AudioSource audioSource;


    private void Awake()
    {
        foreach (Sound s in sounds)
        {
           
                audioSource = gameObject.AddComponent<AudioSource>();
                //s.source = gameObject.AddComponent<AudioSource>();
                s.source.clip = s.clip;
                s.source.playOnAwake = s.playOnstart;
                s.source.loop = s.loop;
                s.source.spatialBlend = s.spatialBlend;
                s.source.minDistance = s.minDistance;
                s.source.maxDistance = s.maxDistance;
                s.source.rolloffMode = s.audioMode;
            

      
        }
    }

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);

        if (s == null)
        {
            Debug.LogWarning("Sound" + name + "not Found!");
            return;
        }

        // s.source.PlayOneShot(s.clip);
        s.source.Play();
    }
}
