using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISaveManager
{
    void SaveGame(ref GameData _gameData);

  void LoadGame(GameData _gameData);
}
