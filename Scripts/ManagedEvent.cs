using System.Collections.Generic;
using UnityEngine.Events;

namespace LughNut.GEM
{
    //No Parameters
    public class ManagedEvent : UnityEvent
    {
        private int _listenerCount;
        private List<object> objects = new List<object>();
        private List<string> methodNames = new List<string>();

        public int ListenerCount { get { return _listenerCount; } }
        public object[] TargetObjects { get { return objects.ToArray(); } }
        public string[] TargetMethods { get { return methodNames.ToArray(); } }


        new public void AddListener(UnityAction call)
        {
            base.AddListener(call);
            objects.Add(call.Target);
            methodNames.Add(call.Method.Name);
            _listenerCount++;
        }

        new public void RemoveListener(UnityAction call)
        {
            base.RemoveListener(call);
            objects.Remove(call.Target);
            methodNames.Remove(call.Method.Name);
            _listenerCount--;
        }
    }

    //Single Parameter
    public class ManagedEvent_A<T> : UnityEvent<T>
    {
        private int _listenerCount;
        private List<object> objects = new List<object>();
        private List<string> methodNames = new List<string>();

        public int ListenerCount { get { return _listenerCount; } }
        public object[] TargetObjects { get { return objects.ToArray(); } }
        public string[] TargetMethods { get { return methodNames.ToArray(); } }


        new public void AddListener(UnityAction<T> call)
        {
            base.AddListener(call);
            objects.Add(call.Target);
            methodNames.Add(call.Method.Name);
            _listenerCount++;
        }

        new public void RemoveListener(UnityAction<T> call)
        {
            base.RemoveListener(call);
            objects.Remove(call.Target);
            methodNames.Remove(call.Method.Name);
            _listenerCount--;
        }
    }
}
