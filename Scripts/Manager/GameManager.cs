using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour,ISaveManager
{
   public static GameManager instance;
   [SerializeField] private CheckPoint[] checkPoints;
   public string lastCheckPointID;
   public bool canPause =true;
   [Header("DeadBody")] 
   private float bodyPositionX;
   private float bodyPositionY;
   public long lostCurrency;
   [SerializeField] private GameObject deadBodyPrefab;
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
      checkPoints = FindObjectsOfType<CheckPoint>();
   }

   public void BackToTitle()=> SceneManager.LoadScene("Main Menu");

   public void Restart()
   {
      Scene scene = SceneManager.GetActiveScene();
      SceneManager.LoadScene(scene.name);
   }

   public void SaveGame(ref GameData _gameData)
   {
      _gameData.bodyPositionX = bodyPositionX;
      _gameData.bodyPositionY = bodyPositionY;
      _gameData.lostCurrency = lostCurrency;
      
      _gameData.checkPoints.Clear();
      _gameData.lastCheckPointID = lastCheckPointID;
      foreach (var checkPoint in checkPoints)
      {
         _gameData.checkPoints.Add(checkPoint.id, checkPoint.active);
      }
   }

   public void LoadGame(GameData _gameData)
   {
      StartCoroutine(LoadWithDelay(_gameData));
   }

   private void LoadDeadBody(GameData _gameData)
   {
      bodyPositionX = _gameData.bodyPositionX;
      bodyPositionY = _gameData.bodyPositionY;
      lostCurrency = _gameData.lostCurrency;
      if(lostCurrency>0)
      {
         GameObject newDeadBody = Instantiate(deadBodyPrefab);
         newDeadBody.GetComponent<DeadBody>().SetUpDeadBody(bodyPositionX, bodyPositionY, lostCurrency);
      }
   }

   private IEnumerator LoadWithDelay(GameData _gameData)
   {
      yield return new WaitForSeconds(0.2f);
      LoadCheckPoint(_gameData);
      LoadDeadBody(_gameData);
   }

   private void LoadCheckPoint(GameData _gameData)
   {
      lastCheckPointID = _gameData.lastCheckPointID;
      foreach (var pair in _gameData.checkPoints)
      {
         foreach (var checkPoint in checkPoints)
         {
            if (checkPoint.id == pair.Key && pair.Value)
            {
               checkPoint.ActiveAnim();
            }
         }
      }
      foreach (var checkPoint in checkPoints)
      {
         if (checkPoint.id == lastCheckPointID)
         {  
            PlayerManager.instance.player.transform.position = checkPoint.transform.position;
         }
      }
   }

   public void SetDeadBody(float _x, float _y, long _currency)
   {
      bodyPositionX = _x;
      bodyPositionY = _y;
      lostCurrency = _currency;
   }

   public void PauseGame(bool _pause)=>Time.timeScale = _pause ? 0 : 1;

   public void QuitGame() => Application.Quit();

   public void CoroutineDelay(float _seconds) => StartCoroutine(DelayWith(_seconds));

   private   IEnumerator DelayWith(float _seconds)
   {
      yield return new WaitForSeconds(_seconds);
   }
}
