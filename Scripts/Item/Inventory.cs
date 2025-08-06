using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

public class Inventory : MonoBehaviour,ISaveManager
{
    public static Inventory instance;

    public List<ItemData> startingEquipment;

    public Transform statSlotsParent;
    private StatSlot[] statSlots;
    
    public List<InventoryItem> inventoryItems;
    private Dictionary<ItemData, InventoryItem> inventoryDictionary;
    public Transform inventorySlotsParent;
    private ItemSlot[] inventorySlots;

    public List<InventoryItem> stashItems;
    private Dictionary<ItemData, InventoryItem> stashDictionary;
    public Transform stashSlotsParent;
    private ItemSlot[] stashSlots;

    public List<InventoryItem> equipmentItems;
    private  Dictionary<ItemData_Equipment, InventoryItem> equipmentDictionary;
    public Transform equipmentSlotsParent;
    private EquipSlot[] equipSlots;
    private float flaskCooldown;
    private float armonCooldown;

   [Header("Data base")] 
    public List<InventoryItem> loadedItems;
    public List<ItemData_Equipment> loadEquipment;
    public List<ItemData> dataBase;
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        inventoryItems = new List<InventoryItem>();
        inventoryDictionary = new Dictionary<ItemData, InventoryItem>();
        inventorySlots = inventorySlotsParent.GetComponentsInChildren<ItemSlot>();

        stashItems = new List<InventoryItem>();
        stashDictionary = new Dictionary<ItemData, InventoryItem>();
        stashSlots = stashSlotsParent.GetComponentsInChildren<ItemSlot>();

        equipmentItems = new List<InventoryItem>();
        equipmentDictionary = new Dictionary<ItemData_Equipment, InventoryItem>();
        equipSlots = equipmentSlotsParent.GetComponentsInChildren<EquipSlot>();

        statSlots = statSlotsParent.GetComponentsInChildren<StatSlot>();

        flaskCooldown = 0;
        armonCooldown = 0;
        //ApplyStartingEquipment();
        StartCoroutine(LoadWithDelay(null));
    }

    
    private void ApplyStartingEquipment()
    {
        if (loadEquipment.Count > 0)
        {
            foreach (var v in loadEquipment)
            {
               EquipItem(v);
            }
        }
        if (loadedItems.Count > 0)
        {
            foreach (var v in loadedItems)
            {
                for (int i = 0; i < v.itemStack; i++)
                {
                    AddItem(v.itemData);
                }
            }
            return;
        }
        
        foreach (var v in startingEquipment)
        {
            AddItem(v);
        }
    }

    public  void UpdateSlotUI()
    {
        for (int i = 0; i < equipSlots.Length; i++)
        {
            equipSlots[i].CleanUpSlot();
        }
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            inventorySlots[i].CleanUpSlot();
        }
        for (int i = 0; i < stashSlots.Length; i++)
        {
            stashSlots[i].CleanUpSlot();
        }
        
        
        for (int i = 0; i < equipSlots.Length; i++)
        {
            foreach (var equipment in equipmentDictionary)
            {
                if(equipment.Key.equipmentType==equipSlots[i].slotType)
                { 
                    equipSlots[i].UpdateSlot(equipment.Value);
                }
            }
        }
        for (int i = 0; i < inventoryItems.Count; i++)
        {
            inventorySlots[i].UpdateSlot(inventoryItems[i]);
        }

        for (int i = 0; i < stashItems.Count; i++)
        {
            stashSlots[i].UpdateSlot(stashItems[i]);
        }

        foreach (var t in statSlots)
        {
            t.UpdateUI();
        }
    }

    public void EquipItem(ItemData _newItem)
    {

        ItemData_Equipment newItemDataEquipment = _newItem as ItemData_Equipment;
        ItemData_Equipment oldItemDataEquipment = null;
        foreach (var equipment in equipmentDictionary)
        {
            if (equipment.Key.equipmentType == newItemDataEquipment.equipmentType)
                oldItemDataEquipment = equipment.Key;
        }

        if (oldItemDataEquipment != null)
        {
            AddItem(oldItemDataEquipment);
            UnequipItem(oldItemDataEquipment);
            oldItemDataEquipment.RemoveModifiers();
        }
        InventoryItem newItem = new InventoryItem(_newItem);
        equipmentItems.Add(newItem);
        equipmentDictionary.Add(newItemDataEquipment, newItem);
        newItemDataEquipment.AddToModifiers();
        RemoveItem(_newItem);
        PlayerManager.instance.player.GetComponent<PlayerStat>().onHealthUpdate();
    }

    public void UnequipItem(ItemData_Equipment _itemDataEquipment)
    {
        if (_itemDataEquipment == null) return;
        if (equipmentDictionary.TryGetValue(_itemDataEquipment, out InventoryItem item))
        {
            equipmentItems.Remove(item);
            equipmentDictionary.Remove(_itemDataEquipment);
            _itemDataEquipment.RemoveModifiers();
        }
    }
    public void AddItem(ItemData _itemData)
    {
        CanPickUp(_itemData.itemType);
        if (_itemData.itemType == ItemType.Material)
        {
            AddToInventory(_itemData);    
        }
        else if (_itemData.itemType == ItemType.Equipment)
        {
            AddToStash(_itemData);
        }
        UpdateSlotUI();
    }

    private void AddToInventory(ItemData _itemData)
    {
        if (inventoryDictionary.TryGetValue(_itemData, out InventoryItem value))
        {
            value.AddStack();
        }
        else
        {
            value = new InventoryItem(_itemData);
            inventoryDictionary.Add(_itemData,value);
            inventoryItems.Add(value);
        }
    }

    private void AddToStash(ItemData _itemData)
    {
        if (stashDictionary.TryGetValue(_itemData, out InventoryItem value))
        {
            value.AddStack();
        }
        else
        {
            value = new InventoryItem(_itemData);
            stashDictionary.Add(_itemData,value);
            stashItems.Add(value);
        }
    }
    public void RemoveItem(ItemData _itemData)
    {
        if (inventoryDictionary.TryGetValue(_itemData, out InventoryItem value))
        {
            if (value.itemStack <= 1)
            {
                inventoryDictionary.Remove(_itemData);
                inventoryItems.Remove(value);
            }
            else
            {
                value.RemoveStack();
            }
        }
        
        else if (stashDictionary.TryGetValue(_itemData, out InventoryItem valueA))
        {
            if (valueA.itemStack <= 1)
            {
                stashDictionary.Remove(_itemData);
                stashItems.Remove(valueA);
            }
            else
            {
                valueA.RemoveStack();
            }
        }
        UpdateSlotUI();
    }
    // Update is called once per frame
    void Update()
    {
        flaskCooldown -= Time.deltaTime;
        armonCooldown -= Time.deltaTime;
        
        if (Input.GetKeyDown(KeyCode.L))
        {
            ItemData data = inventoryItems[0].itemData;
            RemoveItem(data);
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            UseFlask();
        }
    }

    public bool canCraft(ItemData_Equipment itemToCraft, List<InventoryItem> itemRequired)
    {
        List<InventoryItem> itemToUse = new List<InventoryItem>();
        for (int i = 0; i < itemRequired.Count; i++)
        {
            if (inventoryDictionary.TryGetValue(itemRequired[i].itemData, out InventoryItem itemInInventory))
            {
                if(itemInInventory.itemStack < itemRequired[i].itemStack)
                {
                    Debug.Log("材料不足");
                    return false;
                }
                else
                {
                    itemToUse.Add(itemRequired[i]);
                }
                
            }
            else
            {
                Debug.Log("材料不足");
                return false;
            }
        }

        for (int i = 0; i < itemRequired.Count; i++)
        {
            for (int j = 0; j < itemToUse[i].itemStack; j++)
            {
                RemoveItem(itemToUse[i].itemData);     
            }
        }
        AudioManager.instance.PlayerSoundEffect(9,null);
        AddItem(itemToCraft);
        return true;
    }

    public ItemData_Equipment GetEquipmentByType(EquipmentType _type)
    {
        foreach (var equipment in equipmentDictionary)
        {
            if (equipment.Key.equipmentType == _type)
                return equipment.Key;
        }

        return null;
    }

    private void UseFlask()
    {
            if (flaskCooldown < 0)
            {
               ItemData_Equipment flask= GetEquipmentByType(EquipmentType.Flask);
               if (flask)
               {
                   flaskCooldown = flask.itemCooldown;
                   flask.CallEffects(transform);
               }
            }
    }

    public bool CanUseArmon()
    {
        if (armonCooldown < 0)
        {
            Debug.Log("使用盔甲效果");
            armonCooldown = GetEquipmentByType(EquipmentType.Armon).itemCooldown;
            return true;
        }
        return false;
    }

    public bool CanPickUp(ItemType _itemType)
    {
        switch (_itemType)
        {
            case ItemType.Material when inventoryItems.Count >= inventorySlots.Length:
            case ItemType.Equipment when stashItems.Count >= stashSlots.Length:
                PlayerManager.instance.player.fx.CreatePopUpText("can not pick up,please clean up your inventory");
                return false;
            default:
                return true;
        }
    }

    public void SaveGame(ref GameData _gameData)
    {
        _gameData.inventory.Clear();
        _gameData.equipmentID.Clear();
        foreach (var item in inventoryDictionary)
        {
            _gameData.inventory.Add(item.Key.itemID,item.Value.itemStack);
        }
        foreach (var item in stashDictionary)
        {
            _gameData.inventory.Add(item.Key.itemID,item.Value.itemStack);
        }
        foreach (var item in equipmentDictionary)
        {
            _gameData.equipmentID.Add(item.Key.itemID);
        }
    }

    public void LoadGame(GameData _gameData)
    {
       // StartCoroutine(LoadWithDelay(_gameData));
       loadedItems = new List<InventoryItem>();
       foreach (var pair in _gameData.inventory)
       {
           foreach (var item in dataBase)
           {
               if (item != null && item.itemID == pair.Key)
               {
                   InventoryItem storedItem = new InventoryItem(item);
                   storedItem.itemStack = pair.Value;
                   loadedItems.Add(storedItem);
                   break;
               }
           }
       }
       foreach (var id in _gameData.equipmentID)
       {
           foreach (var item in dataBase)
           {
               if (item != null && item.itemID == id)
               {
                   loadEquipment.Add(item as ItemData_Equipment);
                   break;
               }
           }
       }
    }

    private IEnumerator LoadWithDelay(GameData _gameData)
    {
        yield return new WaitForSeconds(0.1f);
        ApplyStartingEquipment();
    }
    
    #if UNITY_EDITOR
    [ContextMenu("初始化道具信息")]
    public void InitDataBase() => dataBase =new List<ItemData>(GetItemDataBase()) ;
    
    private List<ItemData> GetItemDataBase()
    {
       List<ItemData> itemDataBase = new List<ItemData>();
        string[]  assetNames = AssetDatabase.FindAssets("",new[] { "Assets/Scripts/Item/Data/Items" });
        foreach (var assetName in assetNames)
        {
            var path = AssetDatabase.GUIDToAssetPath(assetName);
            var itemData = AssetDatabase.LoadAssetAtPath<ItemData>(path);
            itemDataBase.Add(itemData);
        }

        return itemDataBase;
    }
    #endif
}
