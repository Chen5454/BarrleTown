using UnityEngine.Audio;
using UnityEngine;
using System;
using System.Collections.Generic;

public class SoundManager : MonoBehaviour
{

   public static SoundManager instance;

    public Sound[] sounds;

    Dictionary<string, Sound> soundDictionary;
    

    //public AudioSource audioSource;

    /*
     * All Char have AudioSource
     * we want to recive CLip and then to announce to all that that specific char have to play this sound
     * need tp have Rpc function that get soundmanager funcation to play sound, 
    //Function that 

    */
    private void Awake()
    {
        instance = this;
        

        soundDictionary = new Dictionary<string, Sound>();

        foreach (Sound s in sounds)
        {

            soundDictionary.Add(s.name, s);






           //     audioSource = gameObject.AddComponent<AudioSource>();
           //     //s.source = gameObject.AddComponent<AudioSource>();
           //     s.source.clip = s.clip;
           //     s.source.playOnAwake = s.playOnstart;
           //     s.source.loop = s.loop;
           //     s.source.spatialBlend = s.spatialBlend;
           //     s.source.minDistance = s.minDistance;
           //     s.source.maxDistance = s.maxDistance;
           //     s.source.rolloffMode = s.audioMode;
            

      
        }
    }

    public Sound GetSound(string sound)
    {

        if (soundDictionary.ContainsKey(sound))
        {
            return soundDictionary[sound];
        }

        return null;
    }
}
