using System.Collections;
using UnityEngine;

namespace NikoArchipelago.Stuff;

[RequireComponent(typeof(ParticleSystem))]
public class StayOnScreen : MonoBehaviour
{
    private ParticleSystem _particleSystem;
    private ParticleSystem.Particle[] _particles;
    public float snowRadius = 50f;
    public Camera playerCamera;

    void Start()
    {
        _particleSystem = GetComponent<ParticleSystem>();
        _particles = new ParticleSystem.Particle[_particleSystem.main.maxParticles];
        playerCamera = transform.parent.GetComponent<Camera>();
    }

    void LateUpdate()
    {
        if (!_particleSystem.isPlaying) return;

        int activeParticles = _particleSystem.GetParticles(_particles);
        Vector3 cameraPosition = playerCamera.transform.position;

        for (int i = 0; i < activeParticles; i++)
        {
            if (Vector3.Distance(_particles[i].position, cameraPosition) > snowRadius)
            {
                _particles[i].position = cameraPosition + Random.insideUnitSphere * snowRadius;
                _particles[i].remainingLifetime = _particleSystem.main.startLifetime.constant;
            }
        }

        _particleSystem.SetParticles(_particles, activeParticles);
    }
}
