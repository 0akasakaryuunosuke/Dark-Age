
using System.Collections.Generic;
using UnityEngine;

public class ItemDropController : MonoBehaviour
{
    [SerializeField] private int dropAmount;
    [SerializeField] protected GameObject dropPrefab;
  //  [SerializeField] protected ItemData itemData;
    [SerializeField] private ItemData[] possibleDrop;
    protected List<ItemData> dropList;

    public virtual void GenerateDrop()
    {
        dropList = new List<ItemData>();
        foreach (var t in possibleDrop)
        {
            if(Random.Range(0,100)<=t.dropPossibility)
                dropList.Add(t);
        }

        for (int i = 0; i < dropAmount; i++)
        {
            ItemData itemToDrop = dropList[Random.Range(0, dropList.Count - 1)];
            DropItem(itemToDrop);
            dropList.Remove(itemToDrop);
        }
    }

    // ReSharper disable Unity.PerformanceAnalysis
    protected void DropItem(ItemData _itemData)
    { 
        Vector2 dropVelocity = new Vector2(Random.Range(-10, 10), Random.Range(8, 15));
        ItemObject drop =  Instantiate(dropPrefab, transform.position, Quaternion.identity).GetComponent<ItemObject>();
        drop.GetComponent<ItemObject>().SetUpItem(_itemData,dropVelocity);
    }
}
    