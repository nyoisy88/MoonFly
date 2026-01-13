using UnityEngine;

public class Door : MonoBehaviour
{
    private const string UNLOCK = "Unlock";
    [SerializeField] private CargoArea keyUnlockArea;

    private Animator animator;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        keyUnlockArea.OnCargoDelivered += KeyUnlockArea_OnInteractCompleted;
    }

    private void KeyUnlockArea_OnInteractCompleted(object sender, System.EventArgs e)
    {
        animator.SetTrigger(UNLOCK);
    }
}
