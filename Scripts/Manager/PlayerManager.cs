using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour,ISaveManager
{
   public static PlayerManager instance;
   public Player player;
   public long currency;
   private void Awake()
   {
      if(instance!= null)
         Destroy(instance.gameObject);
      else instance = this;
   }

   public bool canUseCurrency(int _price)
   {
      if (_price > currency)
      {
         Debug.Log("钱不够");
         return false;
      }
      currency -= _price;
      return true;
   }

   public string GetCurrency()
   {
      if(currency> 1_000_000.0)
      {
         double numberInMillion = currency / 1_000_000.0;
        return $"{numberInMillion:F2}M";
      }

      if (currency > 1_000.0)
      {
         double numberInMillion = currency / 1_000.0;
         return $"{numberInMillion:F2}K";
      }
      return currency.ToString();
   }

   public void SaveGame(ref GameData _gameData)
   {
      _gameData.currency = currency;
   }

   public void LoadGame(GameData _gameData)
   {
      currency = _gameData.currency;
   }
}
