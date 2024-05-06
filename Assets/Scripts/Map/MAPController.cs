using System;
using System.Collections;
using System.Collections.Generic;
using H2910.Common.Singleton;
using H2910.Level;
using UnityEngine;
using UnityEngine.UI;

namespace H2910.Map
{
    public class MAPController : ManualSingletonMono<MAPController>
    {
        private List<LevelButton> _mapLevelButtons;
        private LevelButton _activeButton;

        public List<LevelButton> MapLevelButtons
        {
            get { return _mapLevelButtons;}
            set { _mapLevelButtons = value; }
        }
        
        public LevelButton ActiveButton
        {
            get { return _activeButton; }
            set { _activeButton = value; }
        }

        [HideInInspector] 
        public Canvas parentCanvas;

        private MapMaker _mapMaker;
        private ScrollRect _scrollRect;
        private RectTransform _content;
        private int _biomesCount = 6;

        public static int CurrentLevel = 1;
        public static int TopPassedLevel = 30;

        [Header("If true, then the map will scroll to the Active Level Button", order = 1)]
        public bool scrollToActiveButton = true;

        private void Start()
        {
            if (_mapMaker == null) _mapMaker = GetComponent<MapMaker>();
            if(_mapMaker == null) return;
            if (_mapMaker.biomes == null)
            {
                return;
            }

            _content = GetComponent<RectTransform>();
            if(!_content) return;

            List<Biome> bList = new List<Biome>(_mapMaker.biomes);
            bList.RemoveAll((b) => { return b == null; });
            
            if(_mapMaker.mapType == MapType.Vertical) bList.Reverse();
            MapLevelButtons = new List<LevelButton>();
            foreach (var b in bList )
            {
                MapLevelButtons.AddRange(b.levelButtons);
            }

            TopPassedLevel = Mathf.Clamp(TopPassedLevel, 0, MapLevelButtons.Count - 1);
            for (int i = 0; i < MapLevelButtons.Count; i++)
            {
                int scene = i + 1;
                MapLevelButtons[i].button.onClick.AddListener(() =>
                {
                    CurrentLevel = scene;
                    //Load Level
                });
                SetButtonActive(scene, (CurrentLevel == scene || scene == TopPassedLevel + 1),
                    (TopPassedLevel >= scene));
                MapLevelButtons[i].numberText.text = (scene).ToString();
            }

            parentCanvas = GetComponent<Canvas>();
            _scrollRect = GetComponent<ScrollRect>();
            if (scrollToActiveButton) StartCoroutine(SetMapPositionToActiveButton());
        }

        IEnumerator SetMapPositionToActiveButton()
        {
            yield return new WaitForSeconds(0.1f);
            if (_scrollRect)
            {
                int bCount = _mapMaker.biomes.Count;
                if (_mapMaker.mapType == MapType.Vertical)
                {
                    float contentSizeY = _content.sizeDelta.y / (bCount) * (bCount - 1.0f);
                    float relPos = _content.InverseTransformPoint(ActiveButton.transform.position).y;
                    float vpos = (-contentSizeY / (bCount * 2.0f) + relPos) / contentSizeY;
                    
                    SimpleTween.Cancel(gameObject, false);
                    float start = _scrollRect.verticalNormalizedPosition;

                    SimpleTween.Value(gameObject, start, vpos, 0.25f).SetOnUpdate((float f) =>
                    {
                        _scrollRect.verticalNormalizedPosition = Mathf.Clamp01(f);
                    });
                }
                else
                {
                    float contentSizeX = _content.sizeDelta.x / (bCount) * (bCount - 1.0f);
                    float relPos = _content.InverseTransformPoint(ActiveButton.transform.position).x;
                    float hpos = (-contentSizeX / (bCount * 2.0f) + relPos) / contentSizeX;
                    
                    SimpleTween.Cancel(gameObject,false);
                    float start = _scrollRect.horizontalNormalizedPosition;
                    SimpleTween.Value(gameObject, start, hpos, 0.25f).SetOnUpdate((float f) =>
                    {
                        _scrollRect.horizontalNormalizedPosition = Mathf.Clamp01(f);
                    });
                }
            }
        }
        
        private void SetButtonActive(int sceneNumber, bool active, bool isPassed)
        {
            string saveKey = sceneNumber.ToString() + "_stars_";
            int activeStarsCount = (PlayerPrefs.HasKey(saveKey)) ? PlayerPrefs.GetInt(saveKey) : 0;
            MapLevelButtons[sceneNumber - 1].SetActive(active, activeStarsCount, isPassed);
        }

        private void SetControlActivity(bool activity)
        {
            for (int i = 0; i < MapLevelButtons.Count; i++)
            {
                if (!activity) MapLevelButtons[i].button.interactable = activity;
                else
                {
                    MapLevelButtons[i].button.interactable = MapLevelButtons[i].Interactable;
                }
            }
        }
    }
}