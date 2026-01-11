using UnityEngine;

[CreateAssetMenu(fileName = "CargoSO", menuName = "Scriptable Objects/CargoSO")]
public class CargoSO : ScriptableObject
{
    public string cargoName;
    public Transform cargoPrefab;
    public Sprite sprite;
}
