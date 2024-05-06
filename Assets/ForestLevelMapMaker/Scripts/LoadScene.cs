using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace H2910.Level
{
	public class LoadScene : MonoBehaviour
	{
		public void Load(int number)
        {
            if (SceneLoader.Instance) SceneLoader.Instance.LoadScene(number);
        }
	}
}
