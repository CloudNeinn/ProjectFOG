using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SerializableDictionary<TKey, Tvalue> : Dictionary<TKey, Tvalue>, ISerializationCallbackReceiver
{
    [SerializeField] public List<TKey> keys = new List<TKey>();

    [SerializeField] public List<Tvalue> values = new List<Tvalue>();

    public void OnBeforeSerialize()
    {
        keys.Clear();
        values.Clear();
        foreach (KeyValuePair<TKey, Tvalue> pair in this)
        {
            keys.Add(pair.Key);
            values.Add(pair.Value);
        }
    }

    public void OnAfterDeserialize()
    {
        this.Clear();

        if(keys.Count != values.Count)
        {
            Debug.LogError("Amount of keys: " + keys.Count + " not equal to amout of values:" + values.Count);
        }

        for(int i = 0; i < keys.Count; i++)
        {
            this.Add(keys[i], values[i]);
        }
    }


}