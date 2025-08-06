using System;

[Serializable]
public class InventoryItem
{
   public ItemData itemData;
   public int itemStack;

   public InventoryItem(ItemData _itemData)
   {
      itemData = _itemData;
      itemStack = 1;
   }
   public void AddStack() => itemStack++;
   public void RemoveStack() => itemStack--;
}
