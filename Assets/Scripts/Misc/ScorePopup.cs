using TMPro;
using UnityEngine;

public class ScorePopup : MonoBehaviour
{
    [SerializeField] private TextMeshPro scoreText;

    public void SetText(string text)
    {
        scoreText.text = text;
    }

    private void Start()
    {
        Destroy(gameObject, 2f);
    }
}
