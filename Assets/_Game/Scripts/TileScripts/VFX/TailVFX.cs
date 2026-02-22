using UnityEngine;

public class TailVFX : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private ParticleSystem _tailParticles;
    [SerializeField] private bool _includeTrails = true;

    public void SyncColor(Color color)
    {
        if (_tailParticles == null) return;

        Color opaqueColor = color;
        opaqueColor.a = 1f;

        Gradient gradient = new Gradient();
        gradient.SetKeys(
            new GradientColorKey[] { new GradientColorKey(opaqueColor, 0.0f), new GradientColorKey(opaqueColor, 1.0f) },
            new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 0.0f), new GradientAlphaKey(0.0f, 1.0f) }
        );

        var systems = _tailParticles.GetComponentsInChildren<ParticleSystem>();
        foreach (var ps in systems)
        {
            ApplyToSystem(ps, opaqueColor, gradient);
        }
    }

    private void ApplyToSystem(ParticleSystem ps, Color color, Gradient gradient)
    {
        var main = ps.main;
        main.startColor = color;

        var colorModule = ps.colorOverLifetime;
        if (colorModule.enabled)
        {
            colorModule.color = gradient;
        }

        if (_includeTrails)
        {
            var trails = ps.trails;
            if (trails.enabled)
            {
                trails.colorOverLifetime = gradient;
            }
        }
    }
}
