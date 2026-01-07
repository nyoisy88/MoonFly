using System;
using UnityEngine;

public class RocketVisual : MonoBehaviour
{
    [SerializeField] private Rocket rocket;
    [SerializeField] private ParticleSystem thrusterParticleSystemLeft;
    [SerializeField] private ParticleSystem thrusterParticleSystemMiddle;
    [SerializeField] private ParticleSystem thrusterParticleSystemRight;
    [SerializeField] private GameObject explosionVfx;

    private void Start()
    {
        rocket.OnUpForce += Rocket_OnUpForce;
        rocket.OnTurnForce += Rocket_OnTurnForce;
        rocket.OnBeforeForce += Rocket_OnBeforeForce;
        rocket.OnLanded += Rocket_OnLanded;
    }

    private void Rocket_OnLanded(object sender, Rocket.OnLandedEventArgs e)
    {
        if (e.landingType == Rocket.LandingType.WrongLandingArea)
        {
            Instantiate(explosionVfx, transform.position, Quaternion.identity);
            gameObject.SetActive(false);
        }
    }

    private void Rocket_OnBeforeForce(object sender, EventArgs e)
    {
        SetEnableThrusterParticleSystem(thrusterParticleSystemRight, false);
        SetEnableThrusterParticleSystem(thrusterParticleSystemMiddle, false);
        SetEnableThrusterParticleSystem(thrusterParticleSystemLeft, false);
    }

    private void Rocket_OnTurnForce(object sender, bool turnRight)
    {
        SetEnableThrusterParticleSystem(thrusterParticleSystemLeft, turnRight);
        SetEnableThrusterParticleSystem(thrusterParticleSystemRight, !turnRight);
    }

    private void Rocket_OnUpForce(object sender, EventArgs e)
    {
        SetEnableThrusterParticleSystem(thrusterParticleSystemRight, true);
        SetEnableThrusterParticleSystem(thrusterParticleSystemMiddle, true);
        SetEnableThrusterParticleSystem(thrusterParticleSystemLeft, true);

    }

    private void SetEnableThrusterParticleSystem(ParticleSystem thrusterParticleSystem, bool enabled)
    {
        ParticleSystem.EmissionModule thrusterEmissionModule = thrusterParticleSystem.emission;
        thrusterEmissionModule.enabled = enabled;
    }
}
