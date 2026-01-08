using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI finalScoreText;
    [SerializeField] private Button mainmenuBtn;

    private void Start()
    {
        mainmenuBtn.onClick.AddListener(() => {
            GameManager.ResetStaticData();
            SceneLoader.LoadScene(SceneLoader.Scene.MainMenu);
        } );
        finalScoreText.text = "FINAL SCORE: " + GameManager.Instance.TotalScore;
    }
}
