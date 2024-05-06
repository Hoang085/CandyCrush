using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using H2910.Common.Singleton;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

    public class LoadingScreen : ManualSingletonMono<LoadingScreen>
    {
        [SerializeField] Canvas canvas;
        [SerializeField] Image progress;
        [SerializeField] private float minTimeLoad = 2f;

        public bool Loading { set; private get; }

        public void LoadScene(string sceneName, Func<bool> launchCondition = null)
        {
            StartCoroutine(LoadSceneCoroutine(sceneName, launchCondition));
        }

        IEnumerator LoadSceneCoroutine(string sceneName, Func<bool> launchConditon)
        {
            if (Loading) yield break;
            Loading = true;
            canvas.gameObject.SetActive(true);

            var ao = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
            ao.allowSceneActivation = false;

            var t = 0f;

            while (t < minTimeLoad || ao.progress < 0.9f)
            {
                t += Time.unscaledDeltaTime;
                progress.fillAmount = Math.Min(t / minTimeLoad, ao.progress / 0.9f);

                yield return null;
            }

            if (launchConditon != null)
                yield return new WaitUntil(launchConditon);

            ao.allowSceneActivation = true;
        }

        public void OnLoadingFinished()
        {
            Loading = false;
            canvas.gameObject.SetActive(false);
        }

    }

