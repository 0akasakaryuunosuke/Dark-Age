using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BlackholeHotkeyController : MonoBehaviour
{
   private KeyCode myHotkey;
   private TextMeshProUGUI myText;
   private BlackholeSkillController skillController;
   private Transform myEnemy;
   private SpriteRenderer sr;
   public void SetHotkey(KeyCode _newHotkey, BlackholeSkillController _skillController, Transform _enemy)
   {
      myText = GetComponentInChildren<TextMeshProUGUI>();
      sr = GetComponent<SpriteRenderer>();
      skillController = _skillController;
      myEnemy = _enemy;
      myHotkey = _newHotkey;
      myText.text = myHotkey.ToString();
   }

   private void Update()
   {
      if (Input.GetKeyDown(myHotkey))
      {
         skillController.AddEnemyToList(myEnemy);
         sr.color = Color.clear;
         myText.color = Color.clear;
      }
   }
}
