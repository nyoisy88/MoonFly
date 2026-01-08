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
        Rocket.Instance.OnLanded += Rocket_HandleLandingOnce;
        Hide();
    }

    private void Rocket_HandleLandingOnce(object sender, Rocket.OnLandedEventArgs e)
    {
        Rocket.Instance.OnLanded -= Rocket_HandleLandingOnce;

        Show();
        if (e.landingType == Rocket.LandingType.Success)
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

        statsText.text = $@"{e.landingAngle}
                                {e.landingSpeed}
                                x{e.scoreMultiplier}
                                {e.score + GameManager.Instance.Score}";
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
}
