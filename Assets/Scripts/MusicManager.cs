using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public AudioClip[] musicClips;
    private AudioClip currentClip;
    public AudioSource musicSource;

    private void Start()
    {
        currentClip = musicClips[Random.Range(0, musicClips.Length)];
        musicSource.clip = currentClip;
        musicSource.Play();
    }
}
