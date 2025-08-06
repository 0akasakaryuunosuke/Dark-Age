using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_InGame : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private Image dashCooldown;
    [SerializeField] private Image parryCooldown;
    [SerializeField] private Image swordSkillCooldown;
    [SerializeField] private Image blackHoleCooldown;
    [SerializeField] private Image crystalCooldown;
    [SerializeField] private Image flaskCooldown;
    [SerializeField] private TextMeshProUGUI currency;
    private SkillManager skills;
    private PlayerStat playerStat;

    private void Start()
    {
        skills = SkillManager.instance;
        playerStat = PlayerManager.instance.player.GetComponent<PlayerStat>();
        playerStat.onHealthUpdate += UpdateHealthBar;
    }

    private void Update()
    {
        currency.text = PlayerManager.instance.GetCurrency();
        if (Input.GetKeyDown(KeyCode.LeftShift)&&skills.dash.canUseDash)
            checkForCooldown(dashCooldown);
        if(Input.GetKeyDown(KeyCode.Alpha1)&&skills.parry.canParry)
            checkForCooldown(parryCooldown);
        if(Input.GetKeyDown(KeyCode.F)&&skills.sword.canThrow)
            checkForCooldown(swordSkillCooldown);
        if(Input.GetKeyDown(KeyCode.R)&&skills.blackHole.canUseBlackHole)
            checkForCooldown(blackHoleCooldown);
        if(Input.GetKeyDown(KeyCode.Q)&&skills.crystal.canCrystal)
            checkForCooldown(crystalCooldown);
        if(Input.GetKeyDown(KeyCode.C)&&Inventory.instance.GetEquipmentByType(EquipmentType.Flask))
            checkForCooldown(flaskCooldown);
        UpdateCooldown(dashCooldown,skills.dash.cooldown);
        UpdateCooldown(parryCooldown,skills.parry.cooldown);
        UpdateCooldown(swordSkillCooldown,skills.sword.cooldown);
        UpdateCooldown(blackHoleCooldown,skills.blackHole.cooldown);
        UpdateCooldown(crystalCooldown,skills.crystal.cooldown);
        if(Inventory.instance.GetEquipmentByType(EquipmentType.Flask))
          UpdateCooldown(flaskCooldown,Inventory.instance.GetEquipmentByType(EquipmentType.Flask).itemCooldown);
    }

   private void   UpdateCooldown(Image _image,float _cooldown)
   {
       if (_image.fillAmount > 0)
           _image.fillAmount -= 1 / _cooldown * Time.deltaTime;
   }

   private void checkForCooldown(Image _image)
   {
       if (_image.fillAmount <= 0)
           _image.fillAmount = 1;
   }
    private void UpdateHealthBar()
    {
        slider.maxValue = playerStat.maxHP.GetValue();
        slider.value = playerStat.currentHP;
    }
}
