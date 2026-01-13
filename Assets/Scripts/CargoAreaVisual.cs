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
        cargoSprite.sprite = cargoArea.CargoSO.sprite;
        cargoArea.OnCargoDelivered += CargoArea_OnCargoDelivered;
    }

    private void CargoArea_OnCargoDelivered(object sender, System.EventArgs e)
    {
        gameObject.SetActive(false);
    }

    private void Update()
    {
        interactBar.fillAmount = cargoArea.GetProgressNormalized();
    }
}
