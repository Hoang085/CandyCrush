using UnityEngine;
using TMPro;

namespace SuperScrollView
{

    public class FPSDisplay : MonoBehaviour
    {
        float deltaTime = 0.0f;
        private TextMeshProUGUI txtFps;

        void Awake()
        {
            txtFps = GetComponent<TextMeshProUGUI>();
        }

        void Update()
        {
            deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
        }

        void OnGUI()
        {
            float fps = 1.0f / deltaTime;
            txtFps.text = string.Format("   {0:0.} FPS", fps);
        }
    }
}
