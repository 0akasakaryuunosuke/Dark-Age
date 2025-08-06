using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "IceAndFire",menuName = "Data/Effect/IceAndFire")]
public class IceAndFilreEffect : ItemEffect
{
    [SerializeField]private GameObject iceAndFirePrefab;
    
    public override void ExecuteEffect(Transform _target)
    {
        if(PlayerManager.instance.player.primaryAttackState.comboCounter==2)
        {
            Transform player = PlayerManager.instance.player.transform;
            GameObject newIceAndFire = Instantiate(iceAndFirePrefab, _target.position, player.rotation);
            newIceAndFire.GetComponent<IceAndFireController>().SetUp(PlayerManager.instance.player.facingDir);
            Destroy(newIceAndFire, 1.5f);
        }
    }
}
