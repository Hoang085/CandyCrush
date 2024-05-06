using H2910.Map;
using UnityEngine;
using UnityEngine.UI;

namespace H2910.Level
{
    public class LevelButton : MonoBehaviour
    {
        public GameObject LeftStar;
        public GameObject MiddleStar;
        public GameObject RightStar;
        public GameObject Lock;
        public Button button;
        public Text numberText;
        public bool Interactable { get; private set; }
        
        internal void SetActive(bool active, int activeStarsCount, bool isPassed)
        {
            if (LeftStar)  LeftStar.SetActive(activeStarsCount > 1 && isPassed);
            if (MiddleStar) MiddleStar.SetActive(activeStarsCount > 0 && isPassed);
            if (RightStar) RightStar.SetActive(activeStarsCount > 2 && isPassed);
            Interactable = active || isPassed;
            if(button)  button.interactable = Interactable;
            if (active)
            {
                MAPController.Instance.ActiveButton = this;
            }

            if(Lock) Lock.SetActive(!isPassed && !active);
        }
    }
}