
using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour,IPointerDownHandler,IPointerEnterHandler,IPointerExitHandler
{
    [SerializeField]protected TextMeshProUGUI amount;
    [SerializeField]protected Image image;
    public InventoryItem item;

    protected UI ui;
    // Start is called before the first frame update
   public virtual void Start()
    {
        ui = GetComponentInParent<UI>();
    }

    public void CleanUpSlot()
    {
        item = null;
        image.sprite = null;
        image.color=Color.clear;
        amount.text = "";
    }
    public void UpdateSlot(InventoryItem _item)
    {
        item = _item;
        if (item != null)
        {
            image.sprite = item.itemData.icon;
            amount.text = item.itemStack > 1 ? item.itemStack.ToString() : "";
            image.color = item.itemStack > 0 ? Color.white : Color.clear;
        }
    }

    public void UpdateItemOnly(InventoryItem _item)
    {
        item = _item;
        if (item != null)
        {
            image.sprite = item.itemData.icon;
            amount.text = item.itemStack > 1 ? item.itemStack.ToString() : "";
            image.color = Color.white;
        }
    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (item?.itemData.itemType == ItemType.Equipment)
            {
                Inventory.instance.EquipItem(item.itemData);
            }
        }
    }

    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        if (item==null||item.itemData.itemName=="")
            return;
        ui.itemTooltips.gameObject.SetActive(true);
        ui.itemTooltips.UpdateTooltips(item.itemData);
    }

    public virtual void OnPointerExit(PointerEventData eventData)=> ui.itemTooltips.gameObject.SetActive(false);
}
