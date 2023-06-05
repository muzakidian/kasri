using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class FloatValue : ScriptableObject, ISerializationCallbackReceiver {

    [Header("Value yang berjalan di game")]
    public float initialValue;
    [Header("Value default saat menjalankan game")]
    public float RuntimeValue;
    [HideInInspector]

    public void OnAfterDeserialize() { 
        initialValue = RuntimeValue; 
    }
    public void OnBeforeSerialize(){}
}