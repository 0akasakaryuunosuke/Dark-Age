using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BlackHoleSkill : Skill
{
    [SerializeField] private GameObject blackHolePrefab;
    [SerializeField] private float maxSize;
    [SerializeField] private float increaseSpeed;
    [SerializeField] private float shrinkSpeed;
    [SerializeField] private float cloneAttackCooldown;
    [SerializeField] private float blackHoleDuration;
    public bool canUseBlackHole;
    [SerializeField] private SkillTreeSlot blackHoleSkillTreeSlot;
    [SerializeField] private SkillTreeSlot blackHoleSkillPlusTreeSlot;
    public BlackholeSkillController controller { get; private set; }

    protected override void Start()
    {
        base.Start();
        blackHoleSkillTreeSlot.GetComponent<Button>().onClick.AddListener(UnlockBlackHole);
        blackHoleSkillPlusTreeSlot.GetComponent<Button>().onClick.AddListener(UnlockBlackHolePlus);
    }

    protected override void LoadUnlocked()
    {
        UnlockBlackHole();
        UnlockBlackHolePlus();
    }

    private void UnlockBlackHole()
    {
        if (blackHoleSkillTreeSlot.unlocked)
        {
            canUseBlackHole = true;
            blackHoleSkillTreeSlot.icon.color = Color.white;
        }
    }
    private void UnlockBlackHolePlus()
    {
        if (blackHoleSkillPlusTreeSlot.unlocked)
        {
            cooldown = 30;
            blackHoleSkillPlusTreeSlot.icon.color = Color.white;
        }
    }
    public override void UseSkill()
    {
        base.UseSkill();
        GameObject blackHole = Instantiate(blackHolePrefab, player.transform.position,Quaternion.identity);
        controller = blackHole.GetComponent<BlackholeSkillController>();
        controller.SetBlackHole(maxSize,increaseSpeed,shrinkSpeed,cloneAttackCooldown,blackHoleDuration);
    }

    public bool canExit()
    {
        if (controller == null) return false;
        return controller.canExit;
    }
}
