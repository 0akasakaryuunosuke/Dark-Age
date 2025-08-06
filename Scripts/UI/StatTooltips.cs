using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StatTooltips : MonoBehaviour
{
   private int minHeight;
   [SerializeField] private TextMeshProUGUI description;

   public void showTooltips(StatSlot _stat)
   {
      gameObject.SetActive(true);
      description.text = _stat.description;
   
   }

   public void hideTooltips() => gameObject.SetActive(false);
}
