using UnityEngine;

public static class ParticleSystemExtension
{
    public static void EnableEmission(this ParticleSystem mb, bool state = true)
    {
        var main = mb.emission;
        main.enabled = state;
    }
}
