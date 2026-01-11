using UnityEngine;
using UnityEngine.UI;

public class CargoAreaVisual : MonoBehaviour
{
    [SerializeField] private Image interactBar;
    [SerializeField] private SpriteRenderer cargoSprite;

    private CargoArea cargoArea;

    private void Awake()
    {
        cargoArea = GetComponent<CargoArea>();
    }

    private void Start()
    {
        cargoArea.OnInteractCompleted += CargoArea_OnInteractComplete;
        cargoSprite.sprite = cargoArea.CargoSO.sprite;
    }

    private void Update()
    {
        interactBar.fillAmount = cargoArea.GetProgressNormalized();
    }

    private void CargoArea_OnInteractComplete(object sender, System.EventArgs e)
    {
        gameObject.SetActive(false);
    }

}
