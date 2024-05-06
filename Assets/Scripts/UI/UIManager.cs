using H2910.Common.Singleton;
using UnityEngine;

public class UIManager : ManualSingletonMono<UIManager>
{
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private Camera UICamera;
    [SerializeField] private GameObject blockRayCast;
    [SerializeField] private GameObject skipTutorial;

    public Camera CameraUI => UICamera;
    public void ResetCoolDownWisdomBtn()
    {
        
    }
}