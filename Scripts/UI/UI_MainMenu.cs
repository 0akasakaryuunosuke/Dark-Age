using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI_MainMenu : MonoBehaviour
{
   private string sceneName = "Main Scene";
  [SerializeField] private Button continueButton;
  [SerializeField] private UI_BlackScreen blackScreen;
   private void Start()
   {
      if (!SaveManager.instance.HasSavedData())
         continueButton.gameObject.SetActive(false);
      AudioManager.instance.PlayBGM(2);
      
   }

   public void ContinueGame()
   {
      StartCoroutine(FadeOutFor(1.2f));
   }

   public void NewGame()
   {
      SaveManager.instance.DeleteData();
      StartCoroutine(FadeOutFor(1.2f));
   }

   public void ExitGame()
   {
      Application.Quit();
   }

   private IEnumerator FadeOutFor(float _duration)
   {
      blackScreen.FadeOut();
      yield return new WaitForSeconds(_duration);
      SceneManager.LoadScene(sceneName);
   }
}
