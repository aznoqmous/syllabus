using System.Collections.Generic;
using UnityEngine;

public static class AudioSourceExtension
{
    public static void PlayAtRandomPitch(this AudioSource audioSource, float minPitch, float maxPitch)
    {
        audioSource.pitch = Random.Range(minPitch, maxPitch);
        audioSource.Play();
    }
    public static void PlayAtRandomPitch(this AudioSource audioSource, float randomPitchMaxVariation)
    {
        audioSource.pitch = Random.Range(1f - randomPitchMaxVariation, 1f + randomPitchMaxVariation);
        audioSource.Play();
    }
}