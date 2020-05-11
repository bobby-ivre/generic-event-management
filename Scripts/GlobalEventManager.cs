using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace LughNut.GEM
{
    public class GlobalEventManager : MonoBehaviour
    {
        public static Queue<string> recentMessages = new Queue<string>(10);

        private static Dictionary<string, ManagedEvent> allMessages_NoParams = new Dictionary<string, ManagedEvent>();
        private static Dictionary<string, ManagedEvent_A<bool>> allMessages_bool = new Dictionary<string, ManagedEvent_A<bool>>();
        private static Dictionary<string, ManagedEvent_A<float>> allMessages_float = new Dictionary<string, ManagedEvent_A<float>>();
        private static Dictionary<string, ManagedEvent_A<int>> allMessages_int = new Dictionary<string, ManagedEvent_A<int>>();
        private static Dictionary<string, ManagedEvent_A<Vector2>> allMessages_v2 = new Dictionary<string, ManagedEvent_A<Vector2>>();
        private static Dictionary<string, ManagedEvent_A<Vector3>> allMessages_v3 = new Dictionary<string, ManagedEvent_A<Vector3>>();
        private static Dictionary<string, ManagedEvent_A<dynamic>> allMessages_generic = new Dictionary<string, ManagedEvent_A<dynamic>>();

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
        public string[] keys_generic
        {
            get
            {
                string[] allkeys = new string[allMessages_generic.Keys.Count];
                allMessages_generic.Keys.CopyTo(allkeys, 0);
                return allkeys;
            }
        }
        public ManagedEvent GetEvent(string key)
        {
            if (allMessages_NoParams.ContainsKey(key))
                return allMessages_NoParams[key];
            else return null;
        }

        public ManagedEvent_A<dynamic> GetEventDynamic(string key)
        {
            return allMessages_generic[key];
        }

        public ManagedEvent_A<bool> GetEventBool(string key)
        {
            return allMessages_bool[key];
        }
        public static void Message(string messageName)
        {
            //if (messageInspectorIgnoreList.Contains(messageName) == false)
            //{
            //    recentMessages.Enqueue(messageName);
            //    if (recentMessages.Count > 10)
            //        recentMessages.Dequeue();
            //}
            if (allMessages_NoParams.ContainsKey(messageName))
                allMessages_NoParams[messageName].Invoke();
            else
                allMessages_NoParams.Add(messageName, new ManagedEvent());
        }

       public static  void Message<T>(string messageName, T param)
        {
            //if (messageInspectorIgnoreList.Contains(messageName) == false)
            //{
            //    recentMessages.Enqueue(messageName + ": " + param);
            //    if (recentMessages.Count > 10)
            //        recentMessages.Dequeue();
            //}
            if (param is bool)
            {
                if (allMessages_bool.ContainsKey(messageName))
                    allMessages_bool[messageName].Invoke((bool)(object)param);
                else
                    allMessages_bool.Add(messageName, new ManagedEvent_A<bool>());
            }
            else if (param is float)
            {
                if (allMessages_float.ContainsKey(messageName))
                    allMessages_float[messageName].Invoke((float)(object)param);
                else
                    allMessages_float.Add(messageName, new ManagedEvent_A<float>());
            }
            else if (param is int)
            {
                if (allMessages_int.ContainsKey(messageName))
                    allMessages_int[messageName].Invoke((int)(object)param);
                else
                    allMessages_int.Add(messageName, new ManagedEvent_A<int>());
            }
            else if (param is Vector2)
            {
                if (allMessages_v2.ContainsKey(messageName))
                    allMessages_v2[messageName].Invoke((Vector2)(object)param);
                else
                    allMessages_v2.Add(messageName, new ManagedEvent_A<Vector2>());
            }
            else if (param is Vector3)
            {
                if (allMessages_v3.ContainsKey(messageName))
                    allMessages_v3[messageName].Invoke((Vector3)(object)param);
                else
                    allMessages_v3.Add(messageName, new ManagedEvent_A<Vector3>());
            }
            else
            {
                if (allMessages_generic.ContainsKey(messageName))
                    allMessages_generic[messageName].Invoke(param);
                else
                    allMessages_generic.Add(messageName, new ManagedEvent_A<dynamic>());
            }
        }
        

        public static void Listen(string key, UnityAction listener)
        {
            if (allMessages_NoParams.ContainsKey(key))
                allMessages_NoParams[key].AddListener(listener);
            else
            {
                allMessages_NoParams.Add(key, new ManagedEvent());
                allMessages_NoParams[key].AddListener(listener);
            }

        }
        public static void Listen<T>(string key, UnityAction<T> listener)
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
            else
            {
                if (allMessages_generic.ContainsKey(key))
                    allMessages_generic[key].AddListener((UnityAction<dynamic>)(object)listener);
                else
                {
                    allMessages_generic.Add(key, new ManagedEvent_A<dynamic>());
                    allMessages_generic[key].AddListener((UnityAction<dynamic>)(object)listener);
                }
            }

        }
        
        public static void StopListening(string key, UnityAction listener)
        {
            if (allMessages_NoParams.ContainsKey(key))
            {
                allMessages_NoParams[key].RemoveListener(listener);
                if (allMessages_NoParams[key].ListenerCount == 0)
                    allMessages_NoParams.Remove(key);
            }

        }
        public static void StopListening<T>(string key, UnityAction<T> listener)
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
            else
            {
                if (allMessages_generic.ContainsKey(key))
                {
                    allMessages_generic[key].RemoveListener((UnityAction<dynamic>)(object)listener);
                    if (allMessages_generic[key].ListenerCount == 0)
                        allMessages_generic.Remove(key);
                }
            }

        }

        private void OnDestroy()
        {
            allMessages_NoParams.Clear();
            allMessages_bool.Clear();
            allMessages_generic.Clear();
        }
    }


#if UNITY_EDITOR
    [CustomEditor(typeof(GlobalEventManager))]
    public class EventManagerInspector : Editor
    {
        private GlobalEventManager m_eventManager;
        private void Awake()
        {
            m_eventManager = (GlobalEventManager)target;
        }
        public override void OnInspectorGUI()
        {
            Repaint();
            base.OnInspectorGUI();
            string[] recentMessages = GlobalEventManager.recentMessages.ToArray();
            if (recentMessages.Length != 0)
            {
                string recentMessagesLabel = recentMessages[recentMessages.Length - 1];
                for (int i = recentMessages.Length - 2; i > 0; i--)
                {
                    recentMessagesLabel += "\n" + recentMessages[i];
                }
                EditorGUILayout.TextArea(recentMessagesLabel);
            }
            string[] keys = m_eventManager.keys_noParams;
            for (int i = 0; i < keys.Length; i++)
            {
                EditorGUILayout.LabelField(keys[i], EditorStyles.boldLabel);
                ManagedEvent thisEvent = m_eventManager.GetEvent(keys[i]);
                if (thisEvent != null)
                {
                    for (int j = 0; j < thisEvent.TargetObjects.Length; j++)
                    {
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.ObjectField((Object)thisEvent.TargetObjects[j], typeof(Object), true);
                        EditorGUILayout.LabelField(thisEvent.TargetMethods[j]);
                        EditorGUILayout.EndHorizontal();
                    }
                }
            }
            keys = m_eventManager.keys_generic;
            for (int i = 0; i < keys.Length; i++)
            {
                EditorGUILayout.LabelField(keys[i], EditorStyles.boldLabel);
                ManagedEvent_A<dynamic> thisEvent = m_eventManager.GetEventDynamic(keys[i]);
                if (thisEvent != null)
                {
                    for (int j = 0; j < thisEvent.TargetObjects.Length; j++)
                    {
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.ObjectField((Component)thisEvent.TargetObjects[j], typeof(Object), true);
                        EditorGUILayout.LabelField(thisEvent.TargetMethods[j]);
                        EditorGUILayout.EndHorizontal();
                    }
                }
            }
        }
    }

#endif
}

