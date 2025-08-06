using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CheckPoint : MonoBehaviour
{
    private static readonly int Active = Animator.StringToHash("Active");
    private Animator anim;
    private bool inside ;
    public bool active ;
    public string id;
    [SerializeField] private Image tips;
   private void Start()
   {
       anim = GetComponent<Animator>();
       anim.SetBool(Active,active);
       if(tips.gameObject.activeSelf)
        tips.gameObject.SetActive(false);
   }

   private void OnTriggerEnter2D(Collider2D other)
   {
       if (other.GetComponent<Player>() != null)
       {
           tips.gameObject.SetActive(true);
           inside = true;
       }
   }

   private void OnTriggerExit2D(Collider2D other)
   {
       if (other.GetComponent<Player>() != null)
       {
           tips.gameObject.SetActive(false);
           inside = false;
       }
   }
    [ContextMenu("生成id")]
   private void GenerateID()
   {
       id = Guid.NewGuid().ToString();
   }
   private void Update()
   {
       if (inside && (Input.GetKeyDown(KeyCode.Return)||Input.GetKeyDown(KeyCode.KeypadEnter)))
       {
           SaveGameInCheckPoint();
           PlayerStat stat = PlayerManager.instance.player.GetComponent<PlayerStat>();
           stat.currentHP = stat.maxHP.GetValue();
           stat.onHealthUpdate();
       }
   }

   private void SaveGameInCheckPoint()
   {
       anim.SetBool(Active, true);
       active = true;
       GameManager.instance.lastCheckPointID= id;
       SaveManager.instance.SaveData();
   }

   public void ActiveAnim()
   {
       active = true;
       anim.SetBool(Active,active);
   }
}
