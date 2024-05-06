using System;
using System.Collections.Generic;
using H2910.Common.Singleton;
using H2910.Defines;

public class NoticeManager : ManualSingletonMono<NoticeManager>
{
      private bool _isInit = false;
      public static Action<Notice, int> OntriggerNotice;

      private Dictionary<Notice, List<Action<Notice, int>>> _eventControllers =
            new Dictionary<Notice, List<Action<Notice, int>>>();

      private Dictionary<Notice, int> _cachedValue = new Dictionary<Notice, int>();
      private long _time = 0;

      public void Reset()
      {
            _isInit = false;
      }

      public void Init()
      {
            if(_isInit)
                  return;
            _isInit = true;
            //
      }
      public void RegisterNotice(Notice notice, Action<Notice, int> action)
      {
          if(_eventControllers.ContainsKey(notice))
                _eventControllers.Add(notice, new List<Action<Notice, int>>());
          if (!_eventControllers[notice].Contains(action))
          {
                _eventControllers[notice].Add(action);
                if (_cachedValue.ContainsKey(notice))
                      action(notice, _cachedValue[notice]);
          }
      }

      public void UnRegisterNotice(Notice notice, Action<Notice, int> action)
      {
            if(!_eventControllers.ContainsKey(notice))
                  return;
            if(!_eventControllers[notice].Contains(action))
                  return;
            _eventControllers[notice].Remove(action);
      }

      public bool IsNoticeEnable(Notice notice)
      {
            return (_cachedValue.ContainsKey(notice) && _cachedValue[notice] > 0);
      }

      public void TriggerEvent(Notice notice, bool value)
      {
            TriggerEvent(notice, value ? false : true);
      }

      public void TriggerEvent(Notice notice, int value)
      {
            if(!_isInit)
                  return;
            if (!_eventControllers.ContainsKey(notice))
            {
                  _eventControllers.Add(notice, new List<Action<Notice, int>>());
            }

            if (_cachedValue.ContainsKey(notice))
            {
                  int oldValue = _cachedValue[notice];
                  _cachedValue[notice] = value;
            }
            else
            {
                  _cachedValue.Add(notice,value);
            }

            foreach (var action in _eventControllers[notice])
            {
                  action?.Invoke(notice, value);
            }

            OntriggerNotice?.Invoke(notice, value);
      }

      public string GetNoticeText(Notice notice, NoticeIconType iconType)
      {
            switch (iconType)
            {
                  case NoticeIconType.Warn:
                        return "!";
            }

            return "";
      }

      #region Setup Notice

      private void SetupNoticeDailyReward()
      {
            TriggerEvent(Notice.News, true);
            return;
      }
      #endregion

      public void HideNoticeDailyReward()
      {
            TriggerEvent(Notice.News,false);
      }
}
