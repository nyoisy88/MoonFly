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
        Rocket.Instance.OnBulletHit += Rocket_OnBulletHit;
        Hide();
    }

    private void Rocket_OnBulletHit(object sender, EventArgs e)
    {
        Show();
            landedResultText.text = "<color=#ffa500>NOT TOO SLOW?</color>";
            nextBtnText.text = "RETRY";
            OnNextButtonClick = GameManager.Instance.Retry;
    }

    private void Rocket_HandleLandingOnce(RocketLandedSignal signal)
    {
        //Rocket.Instance.OnLanded -= Rocket_HandleLandingOnce;
        SignalBus.Unsubcribe<RocketLandedSignal>(Rocket_HandleLandingOnce);

        Show();
        if (signal.landingType == Rocket.LandingType.Success)
        {
            landedResultText.text = "SUCCESS!!";
            nextBtnText.text = "NEXT LEVEL";
            OnNextButtonClick = GameManager.Instance.NextLevel;
        }
        else
        {
            landedResultText.text = "<color=#FF0000>CRASHED!!</color>";
            nextBtnText.text = "RETRY";
            OnNextButtonClick = GameManager.Instance.Retry;
        }

        statsText.text = $@"{signal.landingAngle}
                                {signal.landingSpeed}
                                x{signal.scoreMultiplier}
                                {signal.score + GameManager.Instance.Score}";
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
    }
}
