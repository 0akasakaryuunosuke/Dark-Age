using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class CraftList : MonoBehaviour,IPointerDownHandler
{
   [SerializeField] private Transform craftSlotsParent;
   [SerializeField] private GameObject craftSlotPrefab;
   [SerializeField] private List<ItemData_Equipment> craftList;

   [SerializeField] private Transform craftListParent;
   private void Start()
   {
      if(this==craftListParent.GetChild(0).GetComponent<CraftList>())
      {
         craftListParent.GetChild(0).GetComponent<CraftList>().SetupSlots();
         if (craftList.Count > 0)
            GetComponentInParent<UI>().craftUI.GetComponentInChildren<CraftWindow>().SetUpCraftWindow(craftList[0]);
      }
   }

  

   private void SetupSlots()
   {
      for (int i = 0; i < craftSlotsParent.childCount; i++)
      {
         Destroy(craftSlotsParent.GetChild(i).gameObject);
      }
      for (int i = 0; i < craftList.Count; i++)
      {
         GameObject newSlot= Instantiate(craftSlotPrefab, craftSlotsParent);
         newSlot.GetComponent<CraftSlot>().SetUpCraftSlot(craftList[i]);
      }
   }

   public void OnPointerDown(PointerEventData eventData)
   {
      SetupSlots();
   }
}
