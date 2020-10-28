// --------------------------------------------------------------------------------------------------------------------
// Creation Date:	02/10/20
// Author:				bgreaney
// Description:		Inspector for EventMessagers
// --------------------------------------------------------------------------------------------------------------------

#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace LughNut.GEM
{
    [CustomEditor(typeof(EventMessager), true)]
    public class EventManagerInspector : Editor
    {
        private EventMessager m_eventManager;
        private void Awake()
        {
            m_eventManager = (EventMessager)target;
        }
        public override void OnInspectorGUI()
        {
            Repaint();
            base.OnInspectorGUI();
            string[] recentMessages = m_eventManager.recentMessages.ToArray();
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
            DrawEvent<bool>(m_eventManager.keys_bool, typeof(bool));
            DrawEvent<float>(m_eventManager.keys_float, typeof(float));
            DrawEvent<int>(m_eventManager.keys_int, typeof(int));
            DrawEvent<Vector2>(m_eventManager.keys_Vector2, typeof(Vector2));
            DrawEvent<Vector3>(m_eventManager.keys_Vector3, typeof(Vector3));

        }

        void DrawEvent<T>(string[] keys, System.Type type)
        {
            if (keys.Length > 0)
                EditorGUILayout.LabelField("Events: " + type.ToString(), EditorStyles.boldLabel);
            for (int i = 0; i < keys.Length; i++)
            {
                ManagedEvent_A<T> thisEvent = null;
                if (type == (typeof(bool)))
                    thisEvent = (ManagedEvent_A<T>)(object)m_eventManager.GetEventBool(keys[i]);
                else if (type == (typeof(float)))
                    thisEvent = (ManagedEvent_A<T>)(object)m_eventManager.GetEventFloat(keys[i]);
                else if (type == (typeof(int)))
                    thisEvent = (ManagedEvent_A<T>)(object)m_eventManager.GetEventInt(keys[i]);
                else if (type == (typeof(Vector2)))
                    thisEvent = (ManagedEvent_A<T>)(object)m_eventManager.GetEventV2(keys[i]);
                else if (type == (typeof(Vector3)))
                    thisEvent = (ManagedEvent_A<T>)(object)m_eventManager.GetEventV3(keys[i]);

                if (thisEvent != null)
                {
                    EditorGUILayout.LabelField(keys[i] + ": " + thisEvent.lastValue, EditorStyles.miniBoldLabel);
                    if (thisEvent.TargetObjects.Length > 0)
                    {
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("Object:");
                        EditorGUILayout.LabelField("Method:");
                        EditorGUILayout.EndHorizontal();
                    }
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
}
#endif
