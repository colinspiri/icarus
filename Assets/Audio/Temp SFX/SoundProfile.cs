using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu(fileName = "New Sound Profile", menuName = "Scriptable Objects/Sound Profile")]
public class SoundProfile : ScriptableObject
{
    public AudioClip sfx;
    public AudioClip[] sfxList = null;
    public AudioMixerGroup mixer;
    public bool looping = false;

    [Range(0.0f, 1.0f)]
    public float volume = 1.0f;
    [Range(0.0f, 5.0f)]
    public float pitchMin = 1.0f;
    [Range(0.0f, 5.0f)]
    public float pitchMax = 1.0f;

    public float getPitch()
    {
        return Random.Range(pitchMin, pitchMax);
    }

    public void PlaySFX()
    {
        if (sfxList != null && sfxList.Length > 0)
        {
            AudioManager.Instance.Play(sfxList[Random.Range(0, sfxList.Length)], volume, getPitch(), mixer, looping);
        }
        else
        {
            AudioManager.Instance.Play(sfx, volume, getPitch(), mixer, looping);
        }
        
    }

    public void StopSFX()
    {
        AudioManager.Instance.StopSFX(sfx);
    }
}
