using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ItemTooltips : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI itemType;
    [SerializeField] private TextMeshProUGUI itemDescription;
  

    public void UpdateTooltips(ItemData _itemData)
    {
        itemName.text = _itemData.itemName;
        itemType.text = _itemData.itemType.ToString();
        if(_itemData.itemType==ItemType.Material)
            itemDescription.text = _itemData.GetDescription().ToString();
        else
        {
            ItemData_Equipment equipment = _itemData as ItemData_Equipment;
            itemDescription.text = equipment.GetDescription().ToString();
        }
        
    }
}
