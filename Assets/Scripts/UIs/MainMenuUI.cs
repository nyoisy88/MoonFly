using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private Button startBtn;
    [SerializeField] private Button quitBtn;

    private void Awake()
    {
        startBtn.onClick.AddListener(() =>
        {
            SceneLoader.LoadScene(SceneLoader.Scene.GameScene);
        });
        quitBtn.onClick.AddListener(() =>
        {
            Application.Quit();
        } );
        Time.timeScale = 1.0f;
    }
}
