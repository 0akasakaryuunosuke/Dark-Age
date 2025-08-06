using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloneSkill : Skill
{
   public GameObject clonePrefab;
   [SerializeField] private bool canCreateOnDashStart;
   [SerializeField] private bool canCreateOnDashOver;
   [SerializeField] private bool canCreateCounterClone;
   [SerializeField] private bool canDuplicate;
   [SerializeField] private int chanceToDuplicate;
   public void CreateClone(float _duration,Vector2 _position)
   {
      GameObject newClone = Instantiate(clonePrefab);
      newClone.GetComponent<CloneSkillController>().SetClone(_duration, _position,FindClosest(newClone.transform),canDuplicate,chanceToDuplicate,player);
   }

   public void CreateCloneWithOffset(Transform _transform, Vector3 _offset)
   {
      GameObject newClone = Instantiate(clonePrefab,_transform.position+_offset,Quaternion.identity);
      newClone.GetComponent<CloneSkillController>().SetClone(0, _transform.position+_offset,FindClosest(newClone.transform),canDuplicate,chanceToDuplicate,player);
   }
   public void CreateOnDashStart()
   {
      if (canCreateOnDashStart) CreateClone(0,player.transform.position);
   }

   public void CreateOnDashOver()
   {
      if(canCreateOnDashOver)CreateClone(0,player.transform.position);
   }

   public void CreateCounterClone(Transform _enemyTransform)
   {
      if (canCreateCounterClone)
         StartCoroutine(CreateCloneWithDelay(_enemyTransform, new Vector3(player.facingDir * 1, 0)));
   }
   private IEnumerator CreateCloneWithDelay(Transform _transform, Vector3 _offset)
   {
      yield return new WaitForSeconds(0.4f);
      CreateClone(0,_transform.position+_offset);
   }
   
}
