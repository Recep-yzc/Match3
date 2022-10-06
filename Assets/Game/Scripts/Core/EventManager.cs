using System.Collections.Generic;
using System.Linq;

namespace Game
{
    public delegate void Subscription(object[] args);

    [ScriptOrder(-9997)]
    public class EventManager
    {
        public static EventManager Instance;

        public void Init()
        {
            Instance = this;
        }

        #region Core

        Dictionary<string, List<Subscription>> events = new Dictionary<string, List<Subscription>>();

        public void Publish(string _eventName, params object[] args)
        {
            if (events.ContainsKey(_eventName))
            {
                var _events = events.Where(x => x.Key == _eventName).ToList();
                foreach (var _event in _events)
                {
                    var _callBacks = _event.Value.ToList();
                    foreach (var _callBack in _callBacks)
                    {
                        _callBack?.Invoke(args);
                    }
                }
            }
        }

        public void Subscribe(string _eventName, Subscription _event)
        {
            List<Subscription> callBacks;
            if (!events.ContainsKey(_eventName))
            {
                callBacks = new List<Subscription>();
                callBacks.Add(_event);

                events.Add(_eventName, callBacks);
            }
            else
            {
                callBacks = events[_eventName];
                callBacks.Add(_event);
            }
        }

        public void Unsubscribe(string _eventName, Subscription _event)
        {
            List<Subscription> callBacks = events[_eventName];

            if (callBacks.Count > 0)
            {
                callBacks.Remove(_event);
            }
            else
            {
                events.Remove(_eventName);
            }
        }

        #endregion
    }
}