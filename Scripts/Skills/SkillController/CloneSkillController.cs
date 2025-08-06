using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


public class CloneSkillController : MonoBehaviour
{
    private float cooldownTimer;
    public float fadingSpeed;
    private SpriteRenderer sr;
    private Animator anim;
    private Transform closestEnemy;
    [SerializeField] private Transform attackCheck;
    private float attackRadius = 1.13f;
    private bool canDuplicate;
    private int chanceToDuplicate;
    private int facingDir = 1;
    private Player player;
    private void Awake()
    {
        sr=GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        cooldownTimer -= Time.deltaTime;
        if (cooldownTimer < 0)
        {
            sr.color = new Color(1, 1, 1,sr.color.a- Time.deltaTime * fadingSpeed);
            if (sr.color.a < 0)
            {
                Destroy(gameObject);
            }
        }
    }

    public void SetClone(float _duration, Vector2 _position, Transform _closestEnemy, bool _canDuplicate, int _chanceToDuplicate,Player _player)
    {
        player = _player;
        transform.position = _position;
        cooldownTimer = _duration;
        anim.SetInteger("AttackCount",Random.Range(1,3));
        closestEnemy = _closestEnemy;
        canDuplicate = _canDuplicate;
        chanceToDuplicate = _chanceToDuplicate;
        FaceClosestTarget();
    }

    public void AnimationTrigger()
    {
        cooldownTimer = -1f;
    }
    private void AttackTrigger()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(attackCheck.position, attackRadius);
        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                player.stat.DoDamage(hit.GetComponent<Enemy>().stat);
                Inventory.instance.GetEquipmentByType(EquipmentType.Amulet)?.CallEffects(hit.transform);
              //  hit.GetComponent<Enemy>().DamageEffect();
                if (canDuplicate)
                {
                    if (Random.Range(0, 100) < chanceToDuplicate)
                    {
                        SkillManager.instance.clone.CreateCloneWithOffset(hit.transform,new Vector3(1*facingDir,0));
                    }
                }
            }
        }
    }

    private void FaceClosestTarget()
    {
        if(closestEnemy!=null)
        {
            if (closestEnemy.transform.position.x < transform.position.x)
            {
                facingDir = -1;
                transform.Rotate(0,180,0);
            }
        }
    }
}
