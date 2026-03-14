using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CollisionDetection : MonoBehaviour
{
    [Header("Events")]
    public UnityEvent onTriggerEnterEvent;

    [Header("Settings")]
    public string identifier = "";

    public static string lastTriggeredId { get; private set; }

    private void OnTriggerEnter(Collider other)
    {
        string otherName = other.gameObject.name.ToLower(); 
        Debug.Log("Collision detected: " + otherName + " == " + identifier+"_Drag");
        if (otherName == identifier+"_drag")
        {
          lastTriggeredId = identifier;
          onTriggerEnterEvent?.Invoke();
        }
    }
}
