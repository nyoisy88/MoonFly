using Signals;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LandedUI : MonoBehaviour
{
    [SerializeField] private GameObject container;
    [SerializeField] private TextMeshProUGUI landedResultText;
    [SerializeField] private TextMeshProUGUI statsText;
    [SerializeField] private Button nextBtn;
    [SerializeField] private TextMeshProUGUI nextBtnText;

    private Action OnNextButtonClick;

    private void Start()
    {
        nextBtn.onClick.AddListener(() =>
        {
            OnNextButtonClick();
        });
        //Rocket.Instance.OnLanded += Rocket_HandleLandingOnce;
        SignalBus.Subcribe<RocketLandedSignal>(Rocket_HandleLandingOnce);
        SignalBus.Subcribe<RocketDestroyedSignal>(Rocket_OnRocketDestroyed);
        Hide();
    }

    private void Rocket_OnRocketDestroyed(RocketDestroyedSignal signal)
    {

        Show();
        landedResultText.text = "<color=#FF0000>EXPLOSION!</color>";
        nextBtnText.text = "RETRY";
        OnNextButtonClick = GameManager.Instance.Retry;
    }

    private void Rocket_HandleLandingOnce(RocketLandedSignal signal)
    {
        SignalBus.Unsubcribe<RocketLandedSignal>(Rocket_HandleLandingOnce);
        SignalBus.Unsubcribe<RocketDestroyedSignal>(Rocket_OnRocketDestroyed);

        Show();
        bool success = true;
        if (signal.landingType == Rocket.LandingType.Success)
        {
            landedResultText.text = "SUCCESS!!";
            nextBtnText.text = "NEXT LEVEL";
            OnNextButtonClick = GameManager.Instance.NextLevel;
        }
        else
        {
            landedResultText.text = "<color=#ffa500>CRASHED!!</color>";
            nextBtnText.text = "RETRY";
            OnNextButtonClick = GameManager.Instance.Retry;
            success = false;
        }

        statsText.text = $@"{signal.landingAngle}
                                {signal.landingSpeed}
                                x{(success ? signal.scoreMultiplier : 0)}
                                {signal.score + (success ? GameManager.Instance.Score : 0)}";
    }

    private void Show()
    {
        container.SetActive(true);
        nextBtn.Select();
    }

    private void Hide()
    {
        container.SetActive(false);
    }

    private void OnDestroy()
    {
        SignalBus.Unsubcribe<RocketLandedSignal>(Rocket_HandleLandingOnce);
        SignalBus.Unsubcribe<RocketDestroyedSignal>(Rocket_OnRocketDestroyed);
    }
}
