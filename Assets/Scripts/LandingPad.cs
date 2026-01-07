using TMPro;
using UnityEngine;

public class LandingPad : MonoBehaviour
{
    [SerializeField] private int scoreMultiplier;
    [SerializeField] private TextMeshPro scoreMultiplierLabel;

    public int ScoreMultiplier => scoreMultiplier;

    private void Awake()
    {
        scoreMultiplierLabel.text = $"x{scoreMultiplier}";
    }
}
