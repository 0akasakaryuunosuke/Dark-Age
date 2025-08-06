using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class UI : MonoBehaviour,ISaveManager
{
   public ItemTooltips itemTooltips;
   public StatTooltips StatTooltips;
   public SkillTreeTooltips skillTreeTooltips;
  
   public GameObject characterUI;
   public GameObject craftUI;
   public GameObject skillTreeUI;
   public GameObject optionsUI;
   public GameObject inGameUI;
   
   public UI_BlackScreen blackScreen;
   [SerializeField] private TextMeshProUGUI youDied;
   [SerializeField] private Button restartButton;
   [SerializeField] private Button backToTitle;
   [SerializeField] private UI_VolumnSlider[] volumes;
   [SerializeField] private TextMeshProUGUI congratulation;
   private void Awake()
   {
      StartCoroutine(LoadSkillTree());
   }

   private void Start()
   {
      Time.timeScale = 1;
      //SwitchOn(inGameUI);
      AudioManager.instance.PlayBGM(0);
      blackScreen.FadeIn();
   }

   private void Update()
   {
      if(Input.GetKeyDown(KeyCode.Escape)&&inGameUI.activeSelf)
         SwitchWithKey(optionsUI);
      else if(Input.GetKeyDown(KeyCode.Escape)&&!inGameUI.activeSelf)
         SwitchWithKey(inGameUI);
      if(Input.GetKeyDown(KeyCode.P))
         SwitchWithKey(characterUI);
      if(Input.GetKeyDown(KeyCode.K))
         SwitchWithKey(skillTreeUI);
      if(Input.GetKeyDown(KeyCode.O))
         SwitchWithKey(craftUI);
   }

   public void SwitchOn(GameObject _menu)
   {
      for (int i = 0; i < transform.childCount; i++)
      {
         if(transform.GetChild(i).GetComponent<UI_BlackScreen>() ==null)
            transform.GetChild(i).gameObject.SetActive(false);
      }

      if (_menu != null)
         _menu.SetActive(true);

      if (GameManager.instance != null&&GameManager.instance.canPause)
      {
         if(_menu==inGameUI)
            GameManager.instance.PauseGame(false);
         else 
            GameManager.instance.PauseGame(true);
      }
   }

   public void SwitchWithKey(GameObject _menu)
   {
      if (_menu != null && _menu.activeSelf)
      {
            
         _menu.SetActive(false);
         if(_menu!=inGameUI)
            SwitchOn(inGameUI);
         return;
      }
      SwitchOn(_menu);
   }

   public void ShowDeathScreen()
   {
      GameManager.instance.canPause = false;
      SwitchOn(null);
      blackScreen.FadeOut();
      AudioManager.instance.PlayerSoundEffect(4,null);
      StartCoroutine(EndScreenWithDelay());
   }

   public void ShowClearScreen()
   {
      GameManager.instance.canPause = false;
      SwitchOn(null);
      blackScreen.FadeOut();
      AudioManager.instance.PlayerSoundEffect(4,null);
      StartCoroutine(ClearGameWithDelay());
   }

   private IEnumerator EndScreenWithDelay()
   {
      yield return new WaitForSeconds(1f);
      youDied.gameObject.SetActive(true);
   }

   private IEnumerator ClearGameWithDelay()
   {
      yield return new WaitForSeconds(1f);
      AudioManager.instance.PlayerSoundEffect(15,null);
      congratulation.gameObject.SetActive(true);
   }
   public void SaveGame(ref GameData _gameData)
   {
      _gameData.volumes.Clear();
      foreach (var item in volumes)
      {
         _gameData.volumes.Add(item.parameter,item.slider.value);
      }
   }

   public void LoadGame(GameData _gameData)
   {
      //StartCoroutine(LoadWithDelay(_gameData));
      foreach (var pair in _gameData.volumes)
      {
         foreach (var item in volumes)
         {
            if (item.parameter == pair.Key)
            {
               item.slider.value = pair.Value;
            }
         }
      }
   }
   
   private IEnumerator LoadWithDelay(GameData _gameData)
   {
      yield return new WaitForSeconds(0.2f);
      foreach (var pair in _gameData.volumes)
      {
         foreach (var item in volumes)
         {
            if (item.parameter == pair.Key)
            {
               item.slider.value = pair.Value;
            }
         }
      }
   }

   private IEnumerator LoadSkillTree()
   {
      SwitchOn(skillTreeUI);
      var canvasGroup =skillTreeUI.GetComponent<CanvasGroup>();
      canvasGroup.alpha = 0f;
      canvasGroup.interactable = false;
      canvasGroup.blocksRaycasts = false;
      inGameUI.SetActive(true);
      yield return new WaitForSeconds(0.5f);
      
      canvasGroup.alpha = 1f;
      canvasGroup.interactable = true;
      canvasGroup.blocksRaycasts = true;
      SwitchOn(inGameUI);
   }
}
