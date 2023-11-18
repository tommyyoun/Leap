using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySoundonStart : MonoBehaviour
{
    SoundManager sounds;

    // Start is called before the first frame update
    void Start()
    {
        sounds = GameObject.FindGameObjectWithTag("Audio").GetComponent<SoundManager>();
        sounds.PlayMusic(sounds.MenuSound);
    }
}

