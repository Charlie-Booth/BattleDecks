using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Audio;
// AudioManager Class Followed from a Tutorial By Brackeys: https://www.youtube.com/watch?v=6OT43pvUyfY (made on 31st May 2017)
public class AudioManager : MonoBehaviour
{
    public Sound[] sounds; // Creates an array of sound classes 

    
    // Start is called before the first frame update
    void Awake()
    {
        //sets the audio source values
        foreach( Sound sound in sounds)
        {
            sound.source = gameObject.AddComponent<AudioSource>();
            sound.source.clip = sound.clip;
            sound.source.volume = sound.volume;
            sound.source.pitch = sound.pitch;
            sound.source.loop = sound.loop;
        }
    }

    // Update is called once per frame
    public void Play(string name)// class that can be played to play a sound in the sound array
    {
       Sound s = Array.Find(sounds, sound => sound.name == name);
        if(s== null)
        {
            return;
        }
        s.source.Play();
    }
}
