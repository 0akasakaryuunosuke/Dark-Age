using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
   public static SkillManager instance;
   public CloneSkill clone { get; private set; }
   public SwordSkill sword{ get; private set; }
   public BlackHoleSkill blackHole {get; private set; }
   public CrystalSkill crystal { get; private set; }
   public DashSkill dash { get; private set; }
   public ParrySkill parry { get; private set; }
   private void Awake()
   {
      if(instance!=null)
          Destroy(instance.gameObject);
      else
       instance = this;
   }

   private void Start()
   {
       clone = GetComponent<CloneSkill>();
       sword = GetComponent<SwordSkill>();
       blackHole = GetComponent<BlackHoleSkill>();
       crystal = GetComponent<CrystalSkill>();
       dash = GetComponent<DashSkill>();
       parry = GetComponent<ParrySkill>();
   }
   
}
