using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StatSlot : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
{
    [SerializeField] private TextMeshProUGUI statValue;
    [SerializeField] private TextMeshProUGUI statNameGUI;
    [SerializeField] private StatType statType;
    [TextArea] public string description;
    private UI ui;
    private void OnValidate()
    {
        gameObject.name = "Stat -" + statType;
        if (statNameGUI)
            statNameGUI.text = statType.ToString();
        
    }

    void Start()
    {
        UpdateUI();
        ui = GetComponentInParent<UI>();
    }

    public void UpdateUI()
    {
        PlayerStat playerStat = PlayerManager.instance.player.GetComponent<PlayerStat>();
        statValue.text = playerStat.GetStat(statType).GetValue().ToString();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ui.StatTooltips.showTooltips(this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
       ui.StatTooltips.hideTooltips();
    }
}
