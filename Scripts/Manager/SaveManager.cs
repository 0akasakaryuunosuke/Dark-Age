using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Linq;

public class SaveManager : MonoBehaviour
{
    public static SaveManager instance;
    public GameData gameData;
    private List<ISaveManager> saveManagers;
    private FileDataHandler dataHandler;
    [SerializeField] private string fileName;
    [SerializeField] private bool encryptData;
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(instance.gameObject);
        }
        else
        {
            instance = this;
        }
    }

    private void Start()
    {
        dataHandler = new FileDataHandler(Application.persistentDataPath, fileName,encryptData);
        saveManagers = FindAllSaveManagers();
        Invoke("LoadGame",0.05f);
    }

    private void OnApplicationQuit()
    {
        SaveData();
    }

    public void LoadGame()
    {
        gameData = dataHandler.Load();
        if (gameData == null)
        {
            gameData = new GameData();
        }
        foreach (var saveManager in saveManagers)
        {
            saveManager.LoadGame( gameData);
        }
        
    }

    public void SaveData()
    {
        foreach (var saveManager in saveManagers)
        {
            saveManager.SaveGame(ref gameData);
        }
        dataHandler.Save(gameData);
        Debug.Log("保存了");
    }

    private  List<ISaveManager> FindAllSaveManagers()
    {
        IEnumerable<ISaveManager> foundSaveManagers = FindObjectsOfType<MonoBehaviour>().OfType<ISaveManager>();
        
        return new List<ISaveManager>(foundSaveManagers);
    }

    [ContextMenu("删除存档")]
    public void DeleteData()
    {
        dataHandler = new FileDataHandler(Application.persistentDataPath, fileName,encryptData);
        dataHandler.DeleteData();
    }

    public bool HasSavedData()
    {
        string fullPath = Path.Combine(Application.persistentDataPath, fileName);
        if (File.Exists(fullPath))
            return true;
        return false;
    }

    // public void SaveDeathInfo()
    // {
    //     foreach (var saveManager in saveManagers)
    //     {
    //         saveManager.SaveGame(ref gameData);
    //     }
    //     dataHandler.SaveLastDeath(gameData);
    // }
}
