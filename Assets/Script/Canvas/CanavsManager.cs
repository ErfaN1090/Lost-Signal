using UnityEngine;

public class CanvasManager : MonoBehaviour
{
    public static CanvasManager instance;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    public GameObject WinScreen;

    public void ShowWinScreen()
    {
        WinScreen.SetActive(true);
    }
}
