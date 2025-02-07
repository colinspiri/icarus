using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu(fileName = "New Sound Profile", menuName = "Scriptable Objects/Sound Profile")]
public class SoundProfile : ScriptableObject
{
    public AudioClip sfx;
    public AudioMixerGroup mixer;

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
        AudioManager.Instance.Play(sfx, volume, getPitch(), mixer);
    }
}
