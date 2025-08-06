using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public enum SwordType
{
    Regular,
    Bounce,
    Pierce,
    Spin
}
public class SwordSkill : Skill
{
    public GameObject swordPrefab;
    [SerializeField] private Vector2 force;
    [SerializeField] private float swordGravity;
    private Vector2 finalDir;
    [SerializeField]private SwordType swordType;
    [SerializeField] private float freezeDuration;
    [Header("瞄准线参数")]
    [SerializeField] private int dotsNumber;
    [SerializeField] private float dotsGap;
    [SerializeField] private Transform dotsParent;
    [SerializeField] private GameObject dotPrefab;
    private GameObject[] dots;

    [Header("技能树")] 
    public bool canThrow;
    [SerializeField] private SkillTreeSlot swordThrowingSkill;
    [Header("剑刃弹跳参数")] 
    [SerializeField]private float bouncingGravity;
    [SerializeField]private int bouncingCounter;
    [SerializeField] private SkillTreeSlot swordBouncing;
    [Header("剑刃穿刺参数")] 
    [SerializeField] private int pierceAmount;
    [SerializeField] private float pierceGravity;
    [SerializeField] private SkillTreeSlot swordPiercing;
    [Header("剑刃旋转参数")] 
    [SerializeField] private float spinDuration;
    [SerializeField] private float maxTravelDistance;
    [SerializeField] private float hitCooldown;
    [SerializeField] private float spinGravity;
    [SerializeField] private SkillTreeSlot swordSpinning;
    public bool canBasicSwordSkill;
    [SerializeField] private SkillTreeSlot basicSwordSkill;
    public bool canNormalSwordSkill;
    [SerializeField] private SkillTreeSlot normalSwordSkill;
    #region 技能树
    protected override void LoadUnlocked()
    {
        UnlockSwordThrowing();
        UnlockSwordSpinning();
        UnlockSwordBouncing();
        UnlockSwordPiercing();
        UnlockBasicSwordSkill();
        UnlockNormalSwordSkill();
    }

    private void UnlockBasicSwordSkill()
    {
        if (basicSwordSkill.unlocked)
        {
            player.stat.strength.AddModifier(5);
            canBasicSwordSkill = true;
            Inventory.instance.UpdateSlotUI();
            basicSwordSkill.icon.color = Color.white;
        }
    }
    private void UnlockNormalSwordSkill()
    {
        if (normalSwordSkill.unlocked)
        {
            player.stat.strength.AddModifier(15);
            canNormalSwordSkill = true;
            Inventory.instance.UpdateSlotUI();
            normalSwordSkill.icon.color = Color.white;
        }
    }
    private void UnlockSwordThrowing()
    {
        if (swordThrowingSkill.unlocked)
        {
            swordType = SwordType.Regular;
            canThrow = true;
            swordThrowingSkill.icon.color = Color.white;
        }
    }

    private void UnlockSwordBouncing()
    {
        if (swordBouncing.unlocked)
        {
            swordType = SwordType.Bounce;
            swordBouncing.icon.color = Color.white;
        }
    }

    private void UnlockSwordPiercing()
    {
        if (swordPiercing.unlocked)
        {
            swordType = SwordType.Pierce;
            swordPiercing.icon.color = Color.white;
        }
    }
    private void UnlockSwordSpinning()
    {
        if (swordSpinning.unlocked)
        {
            swordType = SwordType.Spin;
            swordSpinning.icon.color = Color.white;
        }
    }

    #endregion
    public void CreateSword()
    {
        GameObject sword = Instantiate(swordPrefab, player.transform.position, transform.rotation);
        SwordSkillController controller = sword.GetComponent<SwordSkillController>();

        if (swordType == SwordType.Bounce)
        {
            controller.SetBouncing(true,bouncingCounter);
        }
        else if(swordType == SwordType.Pierce )
        {
            controller.SetPierce(true,pierceAmount);
        }
        else if (swordType == SwordType.Spin)
        {
            controller.SetSpin(true, maxTravelDistance, spinDuration, hitCooldown);
        }
        controller.SetUpSword(finalDir,swordGravity,player,freezeDuration);
        player.AssignSword(sword);
    }
    protected override void Start()
    {
        swordThrowingSkill.GetComponent<Button>().onClick.AddListener(UnlockSwordThrowing);
        swordBouncing.GetComponent<Button>().onClick.AddListener(UnlockSwordBouncing);
        swordPiercing.GetComponent<Button>().onClick.AddListener(UnlockSwordPiercing);
        swordSpinning.GetComponent<Button>().onClick.AddListener(UnlockSwordSpinning);
        basicSwordSkill.GetComponent<Button>().onClick.AddListener(UnlockBasicSwordSkill);
        normalSwordSkill.GetComponent<Button>().onClick.AddListener(UnlockNormalSwordSkill);
        base.Start();
        SetGravity();
        CreateDots();
    }

    private void SetGravity()
    {
        if (swordType == SwordType.Bounce)
        {
            swordGravity = bouncingGravity;
        }
        else if(swordType == SwordType.Pierce )
        {
            swordGravity = pierceGravity;
        }
        else if (swordType == SwordType.Spin)
        {
            swordGravity = spinGravity;
        }
    }


    protected override void Update()
    {
        base.Update();
        if (Input.GetKeyUp(KeyCode.F))
        {
            finalDir = new Vector2(Aim().x * force.x, Aim().y * force.y);
        }

        if (Input.GetKey(KeyCode.F))
        {
            for (int i = 0; i < dotsNumber; i++)
            {
                dots[i].transform.position = SetDotPosition(i*dotsGap);
            }
        }
    }

    #region 瞄准
    private void CreateDots()
    {
        dots = new GameObject[dotsNumber];
        for (int i = 0; i < dotsNumber; i++)
        {
            dots[i] = Instantiate(dotPrefab, player.transform.position, Quaternion.identity, dotsParent);
            dots[i].SetActive(false);
        }
    }

    public void SetDotsActive(bool _active)
    {
        for (int i = 0; i < dotsNumber; i++)
        {
            dots[i].SetActive(_active);
        }
    }

    private Vector2 SetDotPosition(float t)
    {
       
        Vector2 result = transform.position = player.transform.position + new Vector3(
            Aim().x * force.x*t,
            Aim().y * force.y * t +.5f * Physics2D.gravity.y * swordGravity * t * t
        );
                return result;
    }
    private Vector2 Aim()
    {
        Vector2 playerPosition = player.transform.position;
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        
        return (mousePosition - playerPosition).normalized;
    }
    #endregion

}
