using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEngine;


public class FileDataHandler
{
   private string dataDirPath = "";
   private string dataFileName = "";
   private bool encryptData = false;
   private string codeWord = "zongshen";
   public FileDataHandler(string _dataDirPath, string _dataFileName,bool _encryptData)
   {
      dataDirPath = _dataDirPath;
      dataFileName = _dataFileName;
      encryptData = _encryptData;
   }

   public void Save(GameData _data)
   {
      string fullPath = Path.Combine(dataDirPath, dataFileName);
      try
      {
         Directory.CreateDirectory(Path.GetDirectoryName(fullPath));
         string dataToStore = JsonUtility.ToJson(_data, true);
         if (encryptData)
            dataToStore = EncryptDecrypt(dataToStore);
         using (FileStream stream = new FileStream(fullPath, FileMode.Create))
         {
            using (StreamWriter writer = new StreamWriter(stream))
            {
               writer.Write(dataToStore);
            }
         }
      }
      catch (Exception e)
      {
        Debug.LogError("ERROR:"+e);
         throw;
      }
   }

   public void SaveLastDeath(GameData _data)
   {
      string fullPath = Path.Combine(dataDirPath, "last death data.zongshen");
      try
      {
         Directory.CreateDirectory(Path.GetDirectoryName(fullPath));
         string dataToStore = JsonUtility.ToJson(_data, true);
         if (encryptData)
            dataToStore = EncryptDecrypt(dataToStore);
         using (FileStream stream = new FileStream(fullPath, FileMode.Create))
         {
            using (StreamWriter writer = new StreamWriter(stream))
            {
               writer.Write(dataToStore);
            }
         }
      }
      catch (Exception e)
      {
         Debug.LogError("ERROR:"+e);
         throw;
      }
   }

   public GameData LoadLastDeath()
   {
      string fullPath = Path.Combine(dataDirPath, "last death data.zongshen");
      GameData loadData = null;
      if (File.Exists(fullPath))
      {
         try
         {
            string dataToLoad = "";
            using (FileStream stream = new FileStream(fullPath,FileMode.Open))
            {
               using (StreamReader reader =new StreamReader(stream))
               {
                  dataToLoad = reader.ReadToEnd();
               }
            }
            if (encryptData)
               dataToLoad = EncryptDecrypt(dataToLoad);
            loadData = JsonUtility.FromJson<GameData>(dataToLoad);
         }
         catch (Exception e)
         {
            Debug.Log("ERROR-----------------------:"+e);
            throw;
         }
      }
      return loadData;
   }
   
   public GameData Load()
   {
      string fullPath = Path.Combine(dataDirPath, dataFileName);
      GameData loadData = null;
      if (File.Exists(fullPath))
      {
         try
         {
            string dataToLoad = "";
            using (FileStream stream = new FileStream(fullPath,FileMode.Open))
            {
               using (StreamReader reader =new StreamReader(stream))
               {
                  dataToLoad = reader.ReadToEnd();
               }
            }
            if (encryptData)
               dataToLoad = EncryptDecrypt(dataToLoad);
            loadData = JsonUtility.FromJson<GameData>(dataToLoad);
         }
         catch (Exception e)
         {
            Debug.Log("ERROR-----------------------:"+e);
            throw;
         }
      }
      return loadData;
   }

   public void DeleteData()
   {
      string fullPath = Path.Combine(dataDirPath, dataFileName);
      if (File.Exists(fullPath))
         File.Delete(fullPath);
   }

   private string EncryptDecrypt(string _code)
   {
      string modifiedData = "";
      for (int i = 0; i < _code.Length; i++)
      {
         modifiedData += (char)(_code[i] ^ codeWord[i % codeWord.Length]);
      }

      return modifiedData;
   }
}
