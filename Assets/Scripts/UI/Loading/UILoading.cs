using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UILoading : MonoBehaviour
{
    [SerializeField] private GameObject circleFx;

    public void ShowLoading(bool isShow, LoadingTypeFX typeFX)
    {
        switch (typeFX)
        {
            case LoadingTypeFX.Circle:
                gameObject.SetActive(isShow);
                break;
        }
    }
}

public enum LoadingTypeFX
{
    Circle,
}