using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleController : MonoBehaviour
{
    private ParticleSystem particles;
    private Rigidbody2D parentRigidbody;
    public bool isParticleActive = false;

    public void SetParticles(Transform gameParticles)
    {
        particles = gameParticles.GetComponent<ParticleSystem>();
    }
    public void SetParticles(ParticleSystem gameParticles)
    {
        particles = gameParticles;
    }
    public void SetDynamicParticles(Transform gameParticles, GameObject parentObject)
    {
        particles = gameParticles.GetComponent<ParticleSystem>();
        parentRigidbody = parentObject.GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        if (particles != null)
        {
            if (isParticleActive && !particles.isPlaying)
            {
                particles.Play();
            }
            if (!isParticleActive && particles.isPlaying)
            {
                particles.Stop();
            }
        }
    }
}

public class DynamicParticleController : MonoBehaviour
{
    private ParticleSystem particleSys;
    private Rigidbody2D parentRigidbody;
    public bool isParticleActive = false;

    public void SetDynamicParticles(Transform gameParticles, GameObject parentObject)
    {
        particleSys = gameParticles.GetComponent<ParticleSystem>();
        parentRigidbody = parentObject.GetComponent<Rigidbody2D>();
    }

    private void LateUpdate()
    {
        if(isParticleActive)
        {
            ParticleSystem.Particle[] particles = new ParticleSystem.Particle[particleSys.main.maxParticles];
            int numParticlesAlive = particleSys.GetParticles(particles);

            for (int i = 0; i < numParticlesAlive; i++)
            {
                particles[i].velocity = parentRigidbody.velocity;
            }

            particleSys.SetParticles(particles, numParticlesAlive);
        }
    }
}

