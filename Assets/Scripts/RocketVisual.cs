using Signals;
using System;
using UnityEngine;

public class RocketVisual : MonoBehaviour
{
    [SerializeField] private ParticleSystem thrusterParticleSystemLeft;
    [SerializeField] private ParticleSystem thrusterParticleSystemMiddle;
    [SerializeField] private ParticleSystem thrusterParticleSystemRight;
    [SerializeField] private GameObject explosionVfx;

    bool applyingUpForce;
    bool applyingTurnForce;
    Rocket rocket;

    private void Awake()
    {
        rocket = GetComponent<Rocket>();
    }

    private void Start()
    {
        rocket.OnUpForce += Rocket_OnUpForce;
        rocket.OnTurnForce += Rocket_OnTurnForce;
        rocket.OnBeforeForce += Rocket_OnBeforeForce;
        //rocket.OnLanded += Rocket_OnLanded;
        SignalBus.Subcribe<RocketLandedSignal>(OnRocketLanded);

        DisableAllThruster();
    }

    private void OnRocketLanded(RocketLandedSignal e)
    {
        if (e.landingType == Rocket.LandingType.WrongLandingArea)
        {
            Instantiate(explosionVfx, transform.position, Quaternion.identity);
            gameObject.SetActive(false);
        }
    }

    private void Rocket_OnBeforeForce(object sender, EventArgs e)
    {
        if (!applyingUpForce && !applyingTurnForce)
        {
            return;
        }
        applyingTurnForce = false;
        applyingUpForce = false;
        DisableAllThruster();
    }

    private void Rocket_OnTurnForce(object sender, bool turnRight)
    {
        if (applyingTurnForce)
        {
            return;
        }
        applyingTurnForce = true;
        SetEnableThrusterParticleSystem(thrusterParticleSystemLeft, turnRight);
        SetEnableThrusterParticleSystem(thrusterParticleSystemRight, !turnRight);
    }

    private void Rocket_OnUpForce(object sender, EventArgs e)
    {
        if (applyingUpForce)
        {
            return;
        }
        applyingUpForce = true;
        SetEnableThrusterParticleSystem(thrusterParticleSystemRight, true);
        SetEnableThrusterParticleSystem(thrusterParticleSystemMiddle, true);
        SetEnableThrusterParticleSystem(thrusterParticleSystemLeft, true);

    }

    private void DisableAllThruster()
    {
        SetEnableThrusterParticleSystem(thrusterParticleSystemRight, false);
        SetEnableThrusterParticleSystem(thrusterParticleSystemMiddle, false);
        SetEnableThrusterParticleSystem(thrusterParticleSystemLeft, false);
    }

    private void SetEnableThrusterParticleSystem(ParticleSystem thrusterParticleSystem, bool enabled)
    {
        ParticleSystem.EmissionModule thrusterEmissionModule = thrusterParticleSystem.emission;
        thrusterEmissionModule.enabled = enabled;
    }

    private void OnDestroy()
    {
        SignalBus.Unsubcribe<RocketLandedSignal>(OnRocketLanded);
    }
}
