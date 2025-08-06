using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class DashSkill : Skill
{
    public bool canUseDash;
    [SerializeField] private SkillTreeSlot dashSkill;
    public bool canCreateCloneEnd;
    [SerializeField] private SkillTreeSlot createCloneEndSkill;
    public bool canCreateCloneBegin;
    [SerializeField] private SkillTreeSlot createCloneBeginSkill;
    protected  override void Start()
    {
        base.Start();
        dashSkill.GetComponent<Button>().onClick.AddListener(()=>UnlockDashSkill());
        createCloneEndSkill.GetComponent<Button>().onClick.AddListener(()=>UnlockCreateCloneEndSkill());
        createCloneBeginSkill.GetComponent<Button>().onClick.AddListener(()=>UnlockCreateCloneBeginSkill());
    }

    public override void UseSkill()
    {
        base.UseSkill();
    }

    #region 技能树

    protected override void LoadUnlocked()
    {
        UnlockDashSkill();
        UnlockCreateCloneEndSkill();
        UnlockCreateCloneBeginSkill();
    }

    public void UnlockDashSkill()
    {
        if (dashSkill.unlocked)
        {
            canUseDash = true;
            dashSkill.icon.color=Color.white;
        }
    }

    public void UnlockCreateCloneEndSkill()
    {
        if (createCloneEndSkill.unlocked)
        {
            canCreateCloneEnd = true;
            createCloneEndSkill.icon.color = Color.white;
        }
    }

    public void UnlockCreateCloneBeginSkill()
    {
        if (createCloneBeginSkill.unlocked)
        {
            canCreateCloneBegin = true;
            createCloneBeginSkill.icon.color = Color.white;
        }
    }
    #endregion
}
