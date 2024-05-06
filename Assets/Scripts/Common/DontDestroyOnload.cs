using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace H2910.Common
{
    public class DontDestroyOnload : MonoBehaviour
    {
        private static Dictionary<string, GameObject> _cache = new Dictionary<string, GameObject>();

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
            if (_cache.ContainsKey(name))
            {
                Destroy(this.gameObject);
            }
            else
            {
                _cache[name] = gameObject;
            }
        }

        internal static void DestroyAllDontDestroyOnloadGameObject()
        {
            foreach (var item in _cache)
            {
                Destroy(item.Value);
            }
            _cache.Clear();
        }
    }
}