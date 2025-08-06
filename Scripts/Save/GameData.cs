using System;
using System.Collections.Generic;


[Serializable]
public class GameData
{
    public long currency;
    public SerializableDictionary<string, int> inventory;
    public List<string> equipmentID;
    public SerializableDictionary<string, bool> skillTree;
    public SerializableDictionary<string, bool> checkPoints;
    public SerializableDictionary<string, float> volumes;
    public string lastCheckPointID;

    public float bodyPositionX;
    public float bodyPositionY;
    public long lostCurrency;
    public GameData()
    {
        bodyPositionX = 0;
        bodyPositionY = 0;
        lostCurrency = 0;
        currency = 0;
        inventory = new SerializableDictionary<string, int>();
        equipmentID = new List<string>();
        skillTree = new SerializableDictionary<string, bool>();
        checkPoints = new SerializableDictionary<string, bool>();
        volumes = new SerializableDictionary<string, float>();
    }
}
