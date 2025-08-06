
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Scripting;
using UnityEngine.UI;
[Preserve]
public class SkillTreeSlot : MonoBehaviour,ISaveManager,IPointerEnterHandler,IPointerExitHandler
{
   public bool unlocked;
   [SerializeField] private Color colorSkillLocked;
   [SerializeField] private SkillTreeSlot[] shouldBeUnlock;
   [SerializeField] private SkillTreeSlot[] shouldBeLock;
   public Image icon;
   [SerializeField] private string skillName;
   [TextArea] [SerializeField] private string skillDescription;
   [SerializeField] private int price;
   private UI ui;
   private void OnValidate()
   {
      gameObject.name = "Skill Tree -" + skillName;
   }

   private void Awake()
   {
      GetComponent<Button>().onClick.AddListener(()=>Unlock());
   }

   private void Start()
   {
      ui=GetComponentInParent<UI>();
      icon = GetComponent<Image>();
      if(!unlocked) icon.color = colorSkillLocked;
   }

   public void Unlock()
   {
      foreach (var slot in shouldBeUnlock)
      {
         if (!slot.unlocked)
         {
            Debug.Log("前置条件未解锁");
            return;
         }
      }
      foreach (var slot in shouldBeLock)
      {
         if (slot.unlocked)
         {
            Debug.Log("冲突条件已解锁");
            return;
         }
      }
      if (!PlayerManager.instance.canUseCurrency(price))
         return;
      AudioManager.instance.PlayerSoundEffect(12,null);
      unlocked = true;
      icon.color=Color.white;
   }
   
   public  void OnPointerEnter(PointerEventData eventData)
   {
      string newDescription = skillDescription + "\n" + "price:" + price;
      ui.skillTreeTooltips.showTooltips(skillName,newDescription);
   }

   public  void OnPointerExit(PointerEventData eventData)
   {
      ui.skillTreeTooltips.hideTooltips();
   }

   public void SaveGame(ref GameData _gameData)
   {
      if (_gameData.skillTree.TryGetValue(skillName, out bool value))
      {
         _gameData.skillTree.Remove(skillName);
         _gameData.skillTree.Add(skillName,unlocked);
      }
      else
      {
         _gameData.skillTree.Add(skillName, unlocked);
      }

      
   }

   public void LoadGame(GameData _gameData)
   {
      if (_gameData.skillTree.TryGetValue(skillName, out var value))
      {
         unlocked = value;
      }
   }
  
}
