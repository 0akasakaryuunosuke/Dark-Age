using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CraftWindow : MonoBehaviour
{
   [SerializeField] private Image craftIcon;
   [SerializeField] private TextMeshProUGUI craftName;
   [SerializeField] private TextMeshProUGUI craftDescription;
   [SerializeField] private Image[] materials;
   [SerializeField] private Button button;

   public void SetUpCraftWindow(ItemData_Equipment _equipment)
   {
      button.onClick.RemoveAllListeners();
      for (int i = 0; i < materials.Length; i++)
      {
         materials[i].color =Color.clear;
         materials[i].GetComponentInChildren<TextMeshProUGUI>().color= Color.clear;
      }

      craftIcon.sprite = _equipment.icon;
      craftName.text = _equipment.itemName;
      craftDescription.text = _equipment.GetDescription().ToString();

      for (int i = 0; i < _equipment.craftMaterials.Count; i++)
      {
         materials[i].sprite = _equipment.craftMaterials[i].itemData.icon;
         materials[i].color = Color.white;
         TextMeshProUGUI amount = materials[i].GetComponentInChildren<TextMeshProUGUI>();
         amount.color =Color.white;
         amount.text = _equipment.craftMaterials[i].itemStack.ToString();
      }
      button.onClick.AddListener(()=>Inventory.instance.canCraft(_equipment,_equipment.craftMaterials));
   }

}
