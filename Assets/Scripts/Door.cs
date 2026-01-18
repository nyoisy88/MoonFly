using UnityEngine;

public class Door : MonoBehaviour
{
    private const string UNLOCK = "Unlock";
    [SerializeField] private CargoArea keyUnlockArea;

    private bool isUnlocked;

    private Animator animator;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        keyUnlockArea.OnDeliveryCompleted += KeyUnlockArea_OnInteractCompleted;
    }

    private void KeyUnlockArea_OnInteractCompleted(object sender, System.EventArgs e)
    {
        if (!isUnlocked)
        {
            animator.SetTrigger(UNLOCK);
            isUnlocked = true;
        }
    }
}
