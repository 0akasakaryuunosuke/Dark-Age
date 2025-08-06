using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Stat
{
    public List<int> modifiers ;
    [SerializeField] private int baseValue;

  
    public int GetValue()
    {
        int finalValue=baseValue;
        foreach (var value in modifiers)
        {
            finalValue += value;
        }
        return finalValue;
    }

    public void AddModifier(int _modifier) => modifiers.Add(_modifier);
    public void RemoveModifier(int _modifer) => modifiers.Remove(_modifer);

    public void SetDefaultValue(int _defaultValue)=>  baseValue = _defaultValue;
    
}
