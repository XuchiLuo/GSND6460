using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using UnityEngine.AI;
[CustomEditor(typeof(BaseEventSO<>))]

public class BaseEventSOEdittor<T> : Editor
{
    private BaseEventSO<T> baseEventSO;
    private void OnEnable()
    {
        if (baseEventSO == null)
        {
            baseEventSO = target as BaseEventSO<T>;
        }
    }
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        EditorGUILayout.LabelField("Event Listeners" + GetListeners().Count);
        foreach (var listener in GetListeners())
        {
            EditorGUILayout.LabelField(listener.ToString());
        }
        
    }
    private List<MonoBehaviour> GetListeners()
    {
        List<MonoBehaviour> listeners = new ();
        if (baseEventSO == null || baseEventSO.onEventRaised == null)
        {
            return listeners;
        }
        
        var subscribers = baseEventSO.onEventRaised.GetInvocationList();
        foreach (var subscriber in subscribers)
        {
            var obj = subscriber.Target as MonoBehaviour;
            if (!listeners.Contains(obj))
            {
                listeners.Add(obj);
            }
        }
        return listeners;
    }
    
}
