using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushroomAnimationTrigger : MonoBehaviour
{
    
    private Enemy_Mushroom enemy => GetComponentInParent<Enemy_Mushroom>();
    private void AnimationTrigger() => enemy.AnimationFinishTrigger();

    private void AttackTrigger()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(enemy.attackCheck.position, enemy.attackDistance);
        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Player>() != null)
            {
                PlayerStat target = hit.GetComponent<PlayerStat>();
                enemy.stat.DoDamage(target);
            }
        }
    }

    private void OpenCounterWindow() => enemy.OpenCounterAttackWindow();
    private void CloseCounterWindow() => enemy.CloseCounterAttackWindow();

    public void CreateSpore()
    {
        float xOffset = Random.Range(-0.5f, 0.5f);
        GameObject newSpore = 
            Instantiate(enemy.sporePrefab, transform.position + new Vector3(xOffset, 0.5f), Quaternion.identity);
        newSpore.GetComponent<SporeController>().SetUpSpore(enemy.lifeTime,enemy.chasingSpeed,enemy.popSpeed,enemy);
    }
}
