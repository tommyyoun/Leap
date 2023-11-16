using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySoundonStart : MonoBehaviour
{
    [SerializeField] private AudioClip clip; 

    // Start is called before the first frame update
    void Start()
    {
        SoundManager.Instance.PlaySound(clip);
    }

}
