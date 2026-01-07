using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatsUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI statsText;
    [SerializeField] private Image fuelProgressBarImage;
    [SerializeField] private Image upArrow;
    [SerializeField] private Image downArrow;
    [SerializeField] private Image leftArrow;
    [SerializeField] private Image rightArrow;

    private void Start()
    {
        upArrow.gameObject.SetActive(false);
        downArrow.gameObject.SetActive(false);
        leftArrow.gameObject.SetActive(false);
        rightArrow.gameObject.SetActive(false);
    }
    private void Update()
    {
        UpdateStats();

    }

    private void UpdateStats()
    {
        statsText.text = $@"{GameManager.Instance.CurrentLevel}
                            {GameManager.Instance.CurrentScore}
                            {Mathf.Floor(GameManager.Instance.CurrentTime)}
                            {Mathf.Floor(Mathf.Abs(Rocket.Instance.Speed.x * 5))}
                            {Mathf.Floor(Mathf.Abs(Rocket.Instance.Speed.y * 5))}";
        fuelProgressBarImage.fillAmount = Rocket.Instance.GetFuelAmountNormalized();

        bool goUp = Rocket.Instance.Speed.y > 0.05f;
        upArrow.gameObject.SetActive(goUp);
        downArrow.gameObject.SetActive(!goUp);
        bool turnRight = Rocket.Instance.Speed.x > 0.05f;
        rightArrow.gameObject.SetActive(turnRight);
        leftArrow.gameObject.SetActive(!turnRight);
    }
}
