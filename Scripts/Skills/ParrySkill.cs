using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ParrySkill : Skill
{
   public bool canParry;
   [SerializeField] private SkillTreeSlot parrySkill;
   public bool canRestored;
   [SerializeField] private SkillTreeSlot parryRestored;
   [Range(0, 1f)] [SerializeField] private float restoredPercentage;
   public bool canCreateClone;
   [SerializeField] private SkillTreeSlot parryCreateClone;
   protected override void Start()
   {
      base.Start();
      parrySkill.GetComponent<Button>().onClick.AddListener(UnlockParrySkill);
      parryRestored.GetComponent<Button>().onClick.AddListener(UnlockParryRestored);
      parryCreateClone.GetComponent<Button>().onClick.AddListener(UnlockParryCreateClone);
   }

   public override void UseSkill()
   {
      base.UseSkill();
      if(canRestored)
      {
         PlayerManager.instance.player.GetComponent<PlayerStat>().IncreaseHPByPercentage(restoredPercentage);
      }
   }

   #region 技能树

   protected override void LoadUnlocked()
   {
      UnlockParrySkill();
      UnlockParryRestored();
      UnlockParryCreateClone();
   }

   private void UnlockParrySkill()
   {
      if (parrySkill.unlocked)
      {
         canParry = true;
         parrySkill.icon.color = Color.white;
      }
   }

   private void UnlockParryRestored()
   {
      if (parryRestored.unlocked)
      {
         canRestored = true;
         parryRestored.icon.color = Color.white;
      }
   }

   private void UnlockParryCreateClone()
   {
      if (parryCreateClone.unlocked)
      {
         canCreateClone = true;
         parryCreateClone.icon.color = Color.white;
      }
   }

   #endregion
}
