using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SwordSkillController : MonoBehaviour
{
    
    private Rigidbody2D rb;
    private CircleCollider2D cc;
    private Animator anim;
    private Player player;
    private bool canRotate = true;
    private float freezeDuration;
    [Header("剑刃弹跳")]
    [SerializeField]private float bouncingSpeed;
    private bool isBouncing ;
    private  int bouncingCounter;
    private int enemyIndex;
    private  List<Transform> enemyTarget;
    [Header("剑刃穿刺")]
    private int pierceAmount;
    private bool isPiercing;
    [Header("剑刃旋转参数")] 
    private bool isSpinning;
    private float spinDuration;
    private float spinTimer;
    private float maxTravelDistance;
    private float hitCooldown;
    private float hitTimer;
    private float spinGravity;
    private bool wasStopped;
    [Header("回收剑刃")]
    private bool isReturning;
    [SerializeField] private float returnSpeed;

    private void DestroyMe()
    {
        Destroy(gameObject);
    }
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        cc = GetComponent<CircleCollider2D>();
        anim = GetComponentInChildren<Animator>();
    }
    public void SetUpSword(Vector2 _dir, float _gravity, Player _player, float _freezeDuration)
    {
        player = _player;
        rb.velocity = _dir;
        rb.gravityScale = _gravity;
        freezeDuration = _freezeDuration;
        anim.SetBool("Rotate",true);
        Invoke("DestroyMe",6f);
    }

    public void Update()
    {
        if(canRotate)
        {
            transform.right = rb.velocity;
        }
        if (isReturning)
        {
            transform.position = Vector2.MoveTowards(transform.position, 
                player.transform.position,
                returnSpeed * Time.deltaTime);
            if (Vector2.Distance(transform.position, player.transform.position) < 1)
            {
                player.CatchTheSword();
                anim.SetBool("Rotate",false);
               
                if(transform.position.x>player.transform.position.x&&player.facingDir==-1)player.Flip();
                else if(transform.position.x<player.transform.position.x&&player.facingDir==1)player.Flip();
                player.rb.velocity = new Vector2(-player.facingDir*player.swordCatchForce, player.rb.velocity.y);
            }
        }
        Bouncing();
        Spin();
    }

    private void Spin()
    {
        if (isSpinning)
        {
            if (Vector2.Distance(transform.position, player.transform.position) > maxTravelDistance && !wasStopped)
            {
                StopAndSpin();
            }

            if (wasStopped)
            {
                spinTimer -= Time.deltaTime;
                hitTimer -= Time.deltaTime;
                if (hitTimer < 0)
                {
                    hitTimer = hitCooldown;
                    Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 1f);
                    foreach (var hit in colliders)
                    {
                        //hit.GetComponent<Enemy>()?.DamageEffect();
                        if(hit.GetComponent<Enemy>()!=null)
                           DoDamageLogic(hit.GetComponent<Enemy>());
                    }
                }
                if (spinTimer < 0)
                {
                    isSpinning = false;
                    isReturning = true;
                }
            }
        }
    }

    private void StopAndSpin()
    {
        wasStopped = true;
        rb.constraints = RigidbodyConstraints2D.FreezePosition;
    }

    public void SetBouncing(bool _isBouncing,int _bouncingCounter)
    {
        isBouncing = _isBouncing;
        bouncingCounter = _bouncingCounter;
        enemyTarget = new List<Transform>();
    }
    private void Bouncing()
    {
        if (isBouncing&&enemyTarget.Count>0)
        {
            transform.position = Vector2.MoveTowards(transform.position, 
                enemyTarget[enemyIndex].position,
                bouncingSpeed * Time.deltaTime);
            if (Vector2.Distance(transform.position, enemyTarget[enemyIndex].position) < .2f)
            {
                enemyTarget[enemyIndex].GetComponent<Enemy>().DamageEffect();
                DoDamageLogic(enemyTarget[enemyIndex].GetComponent<Enemy>());
                enemyTarget[enemyIndex].GetComponent<Enemy>().StartCoroutine("FreezeTimeFor", freezeDuration);
                if (--bouncingCounter == 0)
                {
                    isBouncing = false;
                    isReturning = true;
                }
                if (++enemyIndex >= enemyTarget.Count)
                {
                    enemyIndex = 0;
                }
            }
        }
    }

    public void ReturnSword()
    {
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        transform.parent = null;
        isReturning = true;
        anim.SetBool("Rotate",true);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isReturning) return;

        if (other.GetComponent<Enemy>() != null)
            DoDamageLogic(other.GetComponent<Enemy>());
         // other.GetComponent<Enemy>()?.StartCoroutine("FreezeTimeFor", freezeDuration);
        if (isPiercing&&pierceAmount>=0&&other.GetComponent<Enemy>()!=null)
        {
            pierceAmount--;
            return;
        }
        if (isSpinning)
        {
           StopAndSpin();
            return;
        }
        canRotate = false;
        cc.enabled = false;
        rb.isKinematic = true;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        EnemyCheck(other);
        if (isBouncing&&enemyTarget.Count>0) return;
        transform.parent = other.transform;
        anim.SetBool("Rotate",false);
    }

    private void EnemyCheck(Collider2D other)
    {
        if (other.GetComponent<Enemy>() != null)
        {
            if (isBouncing && enemyTarget.Count <= 0)
            {
                Collider2D[] colliders = Physics2D.OverlapCircleAll(other.transform.position, 10f);
                foreach (var hit in colliders)
                {
                    if (hit.GetComponent<Enemy>() != null)
                    {
                        enemyTarget.Add(hit.transform);
                    }
                }
            }
        }
    }

    public void SetPierce(bool _isPiercing,int _pierceAmount)
    {
        isPiercing = _isPiercing;
        pierceAmount = _pierceAmount;
    }

    public void SetSpin(bool _isSpinning, float _maxTravelDistance, float _spinDuration, float _hitCooldown)
    {
        isSpinning = _isSpinning;
        maxTravelDistance = _maxTravelDistance;
        spinDuration = _spinDuration;
        hitCooldown = _hitCooldown;
        wasStopped = false;
        spinTimer = spinDuration;
    }

    private void DoDamageLogic(Enemy _enemy)
    {
        player.stat.DoDamage(_enemy.GetComponent<EnemyStat>());
        Inventory.instance.GetEquipmentByType(EquipmentType.Amulet)?.CallEffects(_enemy.transform);
    }
}
