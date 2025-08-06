using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;
using Slider = UnityEngine.UI.Slider;

public class HealthBarUI : MonoBehaviour
{
    private Entity entity;
    private Image background;
    private RectTransform rectTransform;
    private EntityStat myStat;
    private Slider slider;
    void Start()
    {
        entity = GetComponentInParent<Entity>();
        rectTransform = GetComponent<RectTransform>();
        myStat = GetComponentInParent<EntityStat>();
        slider = GetComponentInChildren<Slider>();
        background = GetComponentInChildren<Image>();
        myStat.onHealthUpdate += UpdateHealthBar;
        entity.onFlip += BarUI;
        UpdateHealthBar();
        if(GetComponentInParent<Player>()!=null)
            showHealthBar();
    }

    private void BarUI()
    {
            rectTransform.Rotate(0, 180, 0);
    }

    private void UpdateHealthBar()
    {
        slider.maxValue = myStat.maxHP.GetValue();
        slider.value = myStat.currentHP;
    }

    public void showHealthBar()
    {
      if(background.color != Color.clear) 
      {
            slider.fillRect.GetComponent<Image>().color = Color.clear;
            background.color = Color.clear; 
      }
      else
      {
          slider.fillRect.GetComponent<Image>().color = Color.red;
          background.color = Color.white; 
      }
    }
  
}
