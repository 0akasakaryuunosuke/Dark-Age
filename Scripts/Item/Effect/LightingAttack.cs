using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
[CreateAssetMenu(fileName = "LightingAttack",menuName = "Data/Effect/LightingAttack")]
public class LightingAttack : ItemEffect
{
   [SerializeField] private GameObject lightingPrefab;
   public override void ExecuteEffect(Transform _target)
   {

      GameObject newLighting = Instantiate(lightingPrefab,_target.position,quaternion.identity);
      Destroy(newLighting,1);
   }
}
