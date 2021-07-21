using UnityEngine.Audio;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu (fileName ="Sound" , menuName = "ScriptableObject/Sound")]
public class Sound:ScriptableObject
{


    public string name;
    public AudioClip clip;
    //[HideInInspector]
    //public AudioSource source;
    public bool loop;
    public bool playOnstart;
    [Range(0f,1f)]
    public int spatialBlend;
    public int minDistance;
    public int maxDistance;
    public AudioRolloffMode audioMode;
}
