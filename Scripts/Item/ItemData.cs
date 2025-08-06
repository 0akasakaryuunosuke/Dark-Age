
using System.Text;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public enum ItemType
{
    Material,
    Equipment
}

[CreateAssetMenu(fileName = "New Item Data",menuName = "Data/Item")]
public class ItemData : ScriptableObject
{
    public ItemType itemType;
    public string itemName;
    public Sprite icon ;
    [Range(0,100)]
    public float dropPossibility;
    public string detail;
    protected StringBuilder description;

    public string itemID;

    private void OnValidate()
    {
        #if UNITY_EDITOR
        string path = AssetDatabase.GetAssetPath(this);
        itemID = AssetDatabase.AssetPathToGUID(path);
        #endif
    }

    public virtual StringBuilder GetDescription()
    {
        description = new StringBuilder();
       return description.Append(detail+"\n");
    }
}
