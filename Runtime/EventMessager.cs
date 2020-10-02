// --------------------------------------------------------------------------------------------------------------------
// Creation Date:	02/10/20
// Author:				Bobby Greaney
// Description:		An event messager that uses ManagedEvents to keep track of all events
// --------------------------------------------------------------------------------------------------------------------

#if UNITY_EDITOR
using UnityEditor;
#endif

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace LughNut.GEM
{

	///<summary>
	/// An event messager that uses ManagedEvents to keep track of all events
	///</summary>
	public class EventMessager : MonoBehaviour
	{
        public string messagerName;
		protected Dictionary<string, ManagedEvent> allMessages_NoParams = new Dictionary<string, ManagedEvent>();
		protected Dictionary<string, ManagedEvent_A<bool>> allMessages_bool = new Dictionary<string, ManagedEvent_A<bool>>();
		protected Dictionary<string, ManagedEvent_A<float>> allMessages_float = new Dictionary<string, ManagedEvent_A<float>>();
		protected Dictionary<string, ManagedEvent_A<int>> allMessages_int = new Dictionary<string, ManagedEvent_A<int>>();
		protected Dictionary<string, ManagedEvent_A<Vector2>> allMessages_v2 = new Dictionary<string, ManagedEvent_A<Vector2>>();
		protected Dictionary<string, ManagedEvent_A<Vector3>> allMessages_v3 = new Dictionary<string, ManagedEvent_A<Vector3>>();


#if UNITY_EDITOR
        public static Queue<string> recentMessages = new Queue<string>(10);
        private static List<string> messageInspectorIgnoreList = new List<string>{
            "PerFrameUpdate",
            "MinuteUpdate",
            "HourUpdate"
        };

        public string[] keys_noParams
        {
            get
            {
                string[] allKeys = new string[allMessages_NoParams.Keys.Count];
                allMessages_NoParams.Keys.CopyTo(allKeys, 0);
                return allKeys;
            }
        }
        public string[] keys_bool
        {
            get
            {
                string[] allkeys = new string[allMessages_bool.Keys.Count];
                allMessages_bool.Keys.CopyTo(allkeys, 0);
                return allkeys;
            }
        }
        public string[] keys_float
        {
            get
            {
                string[] allkeys = new string[allMessages_float.Keys.Count];
                allMessages_float.Keys.CopyTo(allkeys, 0);
                return allkeys;
            }
        }
        public string[] keys_int
        {
            get
            {
                string[] allkeys = new string[allMessages_int.Keys.Count];
                allMessages_int.Keys.CopyTo(allkeys, 0);
                return allkeys;
            }
        }
        public string[] keys_Vector2
        {
            get
            {
                string[] allkeys = new string[allMessages_v2.Keys.Count];
                allMessages_v2.Keys.CopyTo(allkeys, 0);
                return allkeys;
            }
        }
        public string[] keys_Vector3
        {
            get
            {
                string[] allkeys = new string[allMessages_v3.Keys.Count];
                allMessages_v3.Keys.CopyTo(allkeys, 0);
                return allkeys;
            }
        }

        public ManagedEvent GetEvent(string key)
        {
            if (allMessages_NoParams.ContainsKey(key))
                return allMessages_NoParams[key];
            else return null;
        }


        public ManagedEvent_A<bool> GetEventBool(string key)
        {
            if (allMessages_bool.ContainsKey(key))
                return allMessages_bool[key];
            return null;
        }
        public ManagedEvent_A<float> GetEventFloat(string key)
        {
            if (allMessages_float.ContainsKey(key))
                return allMessages_float[key];
            return null;
        }
        public ManagedEvent_A<int> GetEventInt(string key)
        {
            if (allMessages_int.ContainsKey(key))
                return allMessages_int[key];
            return null;
        }
        public ManagedEvent_A<Vector2> GetEventV2(string key)
        {
            if (allMessages_v2.ContainsKey(key))
                return allMessages_v2[key];
            return null;
        }
        public ManagedEvent_A<Vector3> GetEventV3(string key)
        {
            if (allMessages_v3.ContainsKey(key))
                return allMessages_v3[key];
            return null;
        }


#endif
        public void Message(string messageName)
        {
#if UNITY_EDITOR
            if (messageInspectorIgnoreList.Contains(messageName) == false)
            {
                recentMessages.Enqueue(messageName);
                if (recentMessages.Count > 10)
                    recentMessages.Dequeue();
            }
#endif
            if (allMessages_NoParams.ContainsKey(messageName))
                allMessages_NoParams[messageName].Invoke();
            else
                allMessages_NoParams.Add(messageName, new ManagedEvent());
        }

        public void Message<T>(string messageName, T param)
        {
#if UNITY_EDITOR
            if (messageInspectorIgnoreList.Contains(messageName) == false)
            {
                recentMessages.Enqueue(messageName + ": " + param);
                if (recentMessages.Count > 10)
                    recentMessages.Dequeue();
            }
#endif
            if (param is bool)
            {
                if (allMessages_bool.ContainsKey(messageName))
                    allMessages_bool[messageName].Invoke((bool)(object)param);
                else
                {
                    allMessages_bool.Add(messageName, new ManagedEvent_A<bool>());
                    allMessages_bool[messageName].Invoke((bool)(object)param);
                }
            }
            else if (param is float)
            {
                if (allMessages_float.ContainsKey(messageName))
                    allMessages_float[messageName].Invoke((float)(object)param);
                else
                {
                    allMessages_float.Add(messageName, new ManagedEvent_A<float>());
                    allMessages_float[messageName].Invoke((float)(object)param);
                }
            }
            else if (param is int)
            {
                if (allMessages_int.ContainsKey(messageName))
                    allMessages_int[messageName].Invoke((int)(object)param);
                else
                {
                    allMessages_int.Add(messageName, new ManagedEvent_A<int>());
                    allMessages_int[messageName].Invoke((int)(object)param);
                }
            }
            else if (param is Vector2)
            {
                if (allMessages_v2.ContainsKey(messageName))
                    allMessages_v2[messageName].Invoke((Vector2)(object)param);
                else
                {
                    allMessages_v2.Add(messageName, new ManagedEvent_A<Vector2>());
                    allMessages_v2[messageName].Invoke((Vector2)(object)param);
                }
            }
            else if (param is Vector3)
            {
                if (allMessages_v3.ContainsKey(messageName))
                    allMessages_v3[messageName].Invoke((Vector3)(object)param);
                else
                {
                    allMessages_v3.Add(messageName, new ManagedEvent_A<Vector3>());
                    allMessages_v3[messageName].Invoke((Vector3)(object)param);
                }
            }
        }


        public void Listen(string key, UnityAction listener)
        {
            if (allMessages_NoParams.ContainsKey(key))
                allMessages_NoParams[key].AddListener(listener);
            else
            {
                allMessages_NoParams.Add(key, new ManagedEvent());
                allMessages_NoParams[key].AddListener(listener);
            }

        }
        public void Listen<T>(string key, UnityAction<T> listener)
        {
            if (listener is UnityAction<bool>)
            {
                if (allMessages_bool.ContainsKey(key))
                    allMessages_bool[key].AddListener((UnityAction<bool>)(object)listener);
                else
                {
                    allMessages_bool.Add(key, new ManagedEvent_A<bool>());
                    allMessages_bool[key].AddListener((UnityAction<bool>)(object)listener);
                }
            }
            else if (listener is UnityAction<float>)
            {
                if (allMessages_float.ContainsKey(key))
                    allMessages_float[key].AddListener((UnityAction<float>)(object)listener);
                else
                {
                    allMessages_float.Add(key, new ManagedEvent_A<float>());
                    allMessages_float[key].AddListener((UnityAction<float>)(object)listener);
                }
            }
            else if (listener is UnityAction<int>)
            {
                if (allMessages_int.ContainsKey(key))
                    allMessages_int[key].AddListener((UnityAction<int>)(object)listener);
                else
                {
                    allMessages_int.Add(key, new ManagedEvent_A<int>());
                    allMessages_int[key].AddListener((UnityAction<int>)(object)listener);
                }
            }
            else if (listener is UnityAction<Vector2>)
            {
                if (allMessages_v2.ContainsKey(key))
                    allMessages_v2[key].AddListener((UnityAction<Vector2>)(object)listener);
                else
                {
                    allMessages_v2.Add(key, new ManagedEvent_A<Vector2>());
                    allMessages_v2[key].AddListener((UnityAction<Vector2>)(object)listener);
                }
            }
            else if (listener is UnityAction<Vector3>)
            {
                if (allMessages_v3.ContainsKey(key))
                    allMessages_v3[key].AddListener((UnityAction<Vector3>)(object)listener);
                else
                {
                    allMessages_v3.Add(key, new ManagedEvent_A<Vector3>());
                    allMessages_v3[key].AddListener((UnityAction<Vector3>)(object)listener);
                }
            }

        }

        public void StopListening(string key, UnityAction listener)
        {
            if (allMessages_NoParams.ContainsKey(key))
            {
                allMessages_NoParams[key].RemoveListener(listener);
                if (allMessages_NoParams[key].ListenerCount == 0)
                    allMessages_NoParams.Remove(key);
            }

        }
        public void StopListening<T>(string key, UnityAction<T> listener)
        {
            if (listener is UnityAction<bool>)
            {
                if (allMessages_bool.ContainsKey(key))
                {
                    allMessages_bool[key].RemoveListener((UnityAction<bool>)(object)listener);
                    if (allMessages_bool[key].ListenerCount == 0)
                        allMessages_bool.Remove(key);
                }
            }
            else if (listener is UnityAction<float>)
            {
                if (allMessages_float.ContainsKey(key))
                {
                    allMessages_float[key].RemoveListener((UnityAction<float>)(object)listener);
                    if (allMessages_float[key].ListenerCount == 0)
                        allMessages_float.Remove(key);
                }
            }
            else if (listener is UnityAction<int>)
            {
                if (allMessages_int.ContainsKey(key))
                {
                    allMessages_int[key].RemoveListener((UnityAction<int>)(object)listener);
                    if (allMessages_int[key].ListenerCount == 0)
                        allMessages_int.Remove(key);
                }
            }
            else if (listener is UnityAction<Vector2>)
            {
                if (allMessages_v2.ContainsKey(key))
                {
                    allMessages_v2[key].RemoveListener((UnityAction<Vector2>)(object)listener);
                    if (allMessages_v2[key].ListenerCount == 0)
                        allMessages_v2.Remove(key);
                }
            }
            else if (listener is UnityAction<Vector3>)
            {
                if (allMessages_v3.ContainsKey(key))
                {
                    allMessages_v3[key].RemoveListener((UnityAction<Vector3>)(object)listener);
                    if (allMessages_v3[key].ListenerCount == 0)
                        allMessages_v3.Remove(key);
                }
            }

        }

        private void OnDestroy()
        {
#if UNITY_EDITOR
            recentMessages.Clear();
#endif
            allMessages_NoParams.Clear();
            allMessages_bool.Clear();
            allMessages_float.Clear();
            allMessages_int.Clear();
            allMessages_v2.Clear();
            allMessages_v3.Clear();
        }
    }
}
