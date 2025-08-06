using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinKingAnimationTrigger : MonoBehaviour
{
    private Enemy_GoblinKing enemy => GetComponentInParent<Enemy_GoblinKing>();
    public void AnimationTrigger() => enemy.AnimationFinishTrigger();

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

    private void CreateBombs()
    {
        Vector2 playerPosition = PlayerManager.instance.player.transform.position;
        BombLogic(playerPosition,0);
        BombLogic(playerPosition,6);
        BombLogic(playerPosition,-6);
    }

    private void BombLogic(Vector2 playerPosition,float _xVelocityOffset)
    {
        GameObject newBomb = Instantiate(enemy.bombPrefab,
            enemy.transform.position + new Vector3(4 * enemy.facingDir, 1.7f), Quaternion.identity);
        float gravity = newBomb.GetComponent<Rigidbody2D>().gravityScale;
        float xVelocity = (playerPosition.x - newBomb.transform.position.x)/1;
        float yVelocity = (playerPosition.y - newBomb.transform.position.y +   gravity*1/2) / 1;
        newBomb.GetComponent<BombController>().SetUpBomb(xVelocity+_xVelocityOffset,yVelocity,enemy);
    }

    private void OpenCounterWindow() => enemy.OpenCounterAttackWindow();
    private void CloseCounterWindow() => enemy.CloseCounterAttackWindow();
}
