using UnityEngine;

public class TouchUI : MonoBehaviour
{
    private void Awake()
    {
        bool showTouchUI = Application.isMobilePlatform; 
        //    && !Application.isEditor;

        gameObject.SetActive(showTouchUI);
    }
}
