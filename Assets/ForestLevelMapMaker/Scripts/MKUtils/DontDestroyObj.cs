using UnityEngine;
using System.Collections;

/*
   11112019 - first
 */

namespace H2910.Level
{
    public class DontDestroyObj : MonoBehaviour
    {
        void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}