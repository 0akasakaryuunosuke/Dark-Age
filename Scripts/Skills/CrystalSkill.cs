using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CrystalSkill : Skill
{
 
   [SerializeField] private GameObject crystalPrefab;
   [SerializeField] private float crystalDuration;
   private GameObject crystal;
   [Header("水晶")]
   public bool canCrystal;
   [SerializeField] private SkillTreeSlot crystalSkill;
   [Header("克隆攻击")] 
   [SerializeField] private bool canCreateClone;
   [SerializeField] private SkillTreeSlot crystalClone;
   [Header("水晶爆炸")]
   [SerializeField] private bool canExplode;
   [SerializeField] private SkillTreeSlot crystalExplode;
   [Header("水晶移动")]
   [SerializeField] private bool canMove;
   [SerializeField] private float moveSpeed;
   [SerializeField] private SkillTreeSlot crystalMoving;
   [Header("多重水晶")]
   [SerializeField] private bool canMulti;
   [SerializeField] private int crystalAmount;
   [SerializeField] private float multiCrystalWindow;
   [SerializeField] private List<GameObject> crystalList;
   [SerializeField] private SkillTreeSlot multiCrystal;
   protected override void Start()
   {
      crystalSkill.GetComponent<Button>().onClick.AddListener(UnlockCrystal);
      crystalClone.GetComponent<Button>().onClick.AddListener(UnlockCrystalClone);
      crystalExplode.GetComponent<Button>().onClick.AddListener(UnlockCrystalExplode);
      crystalMoving.GetComponent<Button>().onClick.AddListener(UnlockCrystalMoving);
      multiCrystal.GetComponent<Button>().onClick.AddListener(UnlockMultiCrystal);
      base.Start();
   }

   public override void UseSkill()
   {
      base.UseSkill();
      if (!canCrystal) return;
      if(CanUseMulti())return;
      if (crystal == null)
      {
         crystal = Instantiate(crystalPrefab, player.transform.position, Quaternion.identity);
         CrystalSkillController controller = crystal.GetComponent<CrystalSkillController>();
         controller.SetCrystal(crystalDuration,canExplode,canMove,moveSpeed,FindClosest(crystal.transform),player);
      }
      else
      {
         if(canMove)return;
         Vector2 playerPosition = player.transform.position;
         player.transform.position = crystal.transform.position;
         crystal.transform.position = playerPosition;

         if (canCreateClone)
         {
            SkillManager.instance.clone.CreateClone(0,crystal.transform.position);
            DestroySelf();
         }
         else
         {
            crystal.GetComponent<CrystalSkillController>()?.CrystalRelease();
         } 
      }
   }
   private void DestroySelf() => Destroy(crystal);

   private bool CanUseMulti()
   {
      if (canMulti)
      {
         if (cooldownTimer < 0)
         {
            if (crystalList.Count == crystalAmount)
            {
               Invoke("MultiWindow",multiCrystalWindow);
            }
            GameObject lastCrystalInList = crystalList[crystalList.Count - 1];
            GameObject newCrystal = Instantiate(lastCrystalInList, player.transform.position, Quaternion.identity);
            newCrystal.GetComponent<CrystalSkillController>().SetCrystal(crystalDuration,canExplode,canMove,moveSpeed,FindClosest(player.transform),player);
            crystalList.Remove(lastCrystalInList);
            if(crystalList.Count<=0)
            {
               cooldownTimer = cooldown;
               RefillCrystal();
            }
         }
         return true;
      }

      return false;
   }

   private void RefillCrystal()
   {
      int amountToFill = crystalAmount - crystalList.Count;
      for (int i = 0; i < amountToFill; i++)
      {
         crystalList.Add(crystalPrefab);
      }
   }

   private void MultiWindow()
   {
      if (crystalList.Count == crystalAmount) return;
      RefillCrystal();
      cooldownTimer = cooldown;
   }

   #region 技能树

   protected override void LoadUnlocked()
   {
      UnlockCrystal();
      UnlockCrystalExplode();
      UnlockCrystalClone();
      UnlockCrystalMoving();
      UnlockMultiCrystal();
   }

   private void UnlockCrystal()
   {
      if (crystalSkill.unlocked)
      {
         canCrystal = true;
         crystalSkill.icon.color =Color.white;
      }
   }

   private void UnlockCrystalClone()
   {
      if (crystalClone.unlocked)
      {
         canCreateClone = true;
         crystalClone.icon.color = Color.white;
      }
   }

   private void UnlockCrystalExplode()
   {
      if (crystalExplode.unlocked)
      {
         canExplode = true;
         crystalExplode.icon.color = Color.white;
      }
   }

   private void UnlockCrystalMoving()
   {
      if (crystalMoving.unlocked)
      {
         canMove = true;
         crystalMoving.icon.color = Color.white;
      }
   }

   private void UnlockMultiCrystal()
   {
      if (multiCrystal.unlocked)
      {
         canMulti = true;
         multiCrystal.icon.color = Color.white;
      }
   }

   #endregion
}
