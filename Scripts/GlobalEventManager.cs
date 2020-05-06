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
        private static Dictionary<string, ManagedEvent_A<int>> allMessages_int = new Dictionary<string, ManagedEvent_A<int>>();
        private static Dictionary<string, ManagedEvent_A<float>> allMessages_float = new Dictionary<string, ManagedEvent_A<float>>();

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
        public string[] keys_Int
        {
            get
            {
                string[] allKeys = new string[allMessages_int.Keys.Count];
                allMessages_int.Keys.CopyTo(allKeys, 0);
                return allKeys;
            }
        }

        public string[] keys_float
        {
            get
            {
                string[] allKeys = new string[allMessages_float.Keys.Count];
                allMessages_float.Keys.CopyTo(allKeys, 0);
                return allKeys;
            }
        }

        public ManagedEvent GetEvent(string key)
        {
            if (allMessages_NoParams.ContainsKey(key))
                return allMessages_NoParams[key];
            else return null;
        }
        public ManagedEvent_A<int> GetIntEvent(string key)
        {
            if (allMessages_int.ContainsKey(key))
                return allMessages_int[key];
            else return null;
        }
        public ManagedEvent_A<float> GetFloatEvent(string key)
        {
            if (allMessages_int.ContainsKey(key))
                return allMessages_float[key];
            else return null;
        }
        public static void Message(string messageName)
        {
            if (messageInspectorIgnoreList.Contains(messageName) == false)
            {
                recentMessages.Enqueue(messageName);
                if (recentMessages.Count > 10)
                    recentMessages.Dequeue();
            }
            if (allMessages_NoParams.ContainsKey(messageName))
                allMessages_NoParams[messageName].Invoke();
            else
                allMessages_NoParams.Add(messageName, new ManagedEvent());
        }

       
        public static void Message(string messageName, int param)
        {
            if (messageInspectorIgnoreList.Contains(messageName) == false)
            {
                recentMessages.Enqueue(messageName + ": " + param);
                if (recentMessages.Count > 10)
                    recentMessages.Dequeue();
            }
            if (allMessages_int.ContainsKey(messageName))
                allMessages_int[messageName].Invoke(param);
            else
                allMessages_int.Add(messageName, new ManagedEvent_A<int>());
        }

        public static void Message(string messageName, float param)
        {
            if (messageInspectorIgnoreList.Contains(messageName) == false)
            {
                recentMessages.Enqueue(messageName + ": " + param);
                if (recentMessages.Count > 10)
                    recentMessages.Dequeue();
            }
            if (allMessages_float.ContainsKey(messageName))
                allMessages_float[messageName].Invoke(param);
            else
                allMessages_float.Add(messageName, new ManagedEvent_A<float>());
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
        public static void Listen(string key, UnityAction<int> listener)
        {
            if (allMessages_int.ContainsKey(key))
                allMessages_int[key].AddListener(listener);
            else
            {
                allMessages_int.Add(key, new ManagedEvent_A<int>());
                allMessages_int[key].AddListener(listener);
            }

        }

        public static void Listen(string key, UnityAction<float> listener)
        {
            if (allMessages_float.ContainsKey(key))
                allMessages_float[key].AddListener(listener);
            else
            {
                allMessages_float.Add(key, new ManagedEvent_A<float>());
                allMessages_float[key].AddListener(listener);
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
        public static void StopListening(string key, UnityAction<int> listener)
        {
            if (allMessages_int.ContainsKey(key))
            {
                allMessages_int[key].RemoveListener(listener);
                if (allMessages_int[key].ListenerCount == 0)
                    allMessages_int.Remove(key);
            }

        }

        public static void StopListening(string key, UnityAction<float> listener)
        {
            if (allMessages_float.ContainsKey(key))
            {
                allMessages_float[key].RemoveListener(listener);
                if (allMessages_float[key].ListenerCount == 0)
                    allMessages_float.Remove(key);
            }

        }

        private void OnDestroy()
        {
            allMessages_NoParams.Clear();
            allMessages_int.Clear();
            allMessages_float.Clear();
            recentMessages.Clear();
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
                        EditorGUILayout.ObjectField((UnityEngine.Object)thisEvent.TargetObjects[j], typeof(UnityEngine.Object), true);
                        EditorGUILayout.LabelField(thisEvent.TargetMethods[j]);
                        EditorGUILayout.EndHorizontal();
                    }
                }
            }
            keys = m_eventManager.keys_Int;
            for (int i = 0; i < keys.Length; i++)
            {
                EditorGUILayout.LabelField(keys[i], EditorStyles.boldLabel);
                ManagedEvent_A<int> thisEvent = m_eventManager.GetIntEvent(keys[i]);
                if (thisEvent != null)
                {
                    for (int j = 0; j < thisEvent.TargetObjects.Length; j++)
                    {
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.ObjectField((UnityEngine.Component)thisEvent.TargetObjects[j], typeof(UnityEngine.Object), true);
                        EditorGUILayout.LabelField(thisEvent.TargetMethods[j]);
                        EditorGUILayout.EndHorizontal();
                    }
                }
            }
        }
    }

#endif
}

