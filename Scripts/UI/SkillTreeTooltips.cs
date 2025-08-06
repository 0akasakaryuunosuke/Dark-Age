using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;


public class SkillTreeTooltips : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI skillName;
    [SerializeField] private TextMeshProUGUI skillDescription;
    private int minLength = 60;
    public void showTooltips(string _skillName, string _skillDescription)
    {
        gameObject.SetActive(true);
        skillName.text = _skillName;
        StringBuilder sb = new StringBuilder(_skillDescription);
      if(_skillDescription.Length<minLength)
      {
          for (int i = 0; i < (minLength - _skillDescription.Length)/10+1; i++)
          {
              sb.AppendLine();
          }
      }
      skillDescription.text = sb.ToString();
      Vector2 mousePosition = Input.mousePosition;
      float xOffset = 200, yOffset = -50;
      if (mousePosition.x + xOffset +180>1200)
          xOffset = -230;
      if (mousePosition.y + yOffset - 200 < -420)
          yOffset = 250;
      transform.position = new Vector2(mousePosition.x + xOffset, mousePosition.y + yOffset);
    }

    public void hideTooltips() => gameObject.SetActive(false);
}
