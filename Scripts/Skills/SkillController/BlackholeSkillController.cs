using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BlackholeSkillController : MonoBehaviour
{
     private bool canIncrease;
     private bool canShrink;
     private float maxSize;
     private float increaseSpeed;
     private float shrinkSpeed;
     private float blackHoleTimer;
    [SerializeField] private List<Transform> enemyList = new();
    [SerializeField] private List<GameObject> hotkeyList = new();
    [SerializeField] private List<KeyCode> keyCodeList;
    [SerializeField] private GameObject hotkeyPrefab;
    private float cloneAttackCooldown;
    private float cloneAttackTimer;
    private int enemyIndex = 0;
    private bool canAttack = false;
    private bool attackReleased = false;
    public bool canExit = false;
    public void SetBlackHole(float _maxSize, float _increaseSpeed, float _shrinkSpeed,float _cloneAttackCooldown,float _blackHoleDuration)
    {
        maxSize = _maxSize;
        increaseSpeed = _increaseSpeed;
        shrinkSpeed = _shrinkSpeed;
        cloneAttackCooldown = _cloneAttackCooldown;
        blackHoleTimer = _blackHoleDuration;
        canIncrease = true;
        canShrink = false;
    }

private void Update()
    {
        cloneAttackTimer -= Time.deltaTime;
        blackHoleTimer -= Time.deltaTime;
        if (blackHoleTimer < 0&&!attackReleased)
        {
           
            canAttack = true;
            DestroyHotkey();
        }
        BlackHoleSizeController();

        if (Input.GetKeyDown(KeyCode.R)&&!attackReleased)
        {
            canAttack = true;
            DestroyHotkey();
        }
        CloneAttack();
    }

    private void CloneAttack()
    {
        if (canAttack)
        {
            attackReleased = true;
            if(cloneAttackTimer<0&&enemyList.Count>0)
            {
                float xOffset = Random.Range(0, 100) > 50 ? 1 : -1;
                SkillManager.instance.clone.CreateClone(0, enemyList[enemyIndex++].position + new Vector3(xOffset, 0));
                cloneAttackTimer = cloneAttackCooldown;
            }
            if (enemyIndex >= enemyList.Count)
            {
                canAttack = false;
                canShrink = true;
                canExit = true;
            }
        }
    }

    private void BlackHoleSizeController()
    {
        if (canIncrease&&!canShrink)
        {
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(maxSize, maxSize), increaseSpeed*Time.deltaTime);
        }

        if (canShrink)
        {
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(-1, -1), shrinkSpeed *Time.deltaTime);
            if (transform.localScale.x < 0)
            {
                Destroy(gameObject);
            }
        }
    }

    private void DestroyHotkey()
    {
        if (hotkeyList.Count <= 0) return;
        for (int i = 0; i < hotkeyList.Count; i++)
        {
            Destroy(hotkeyList[i]);
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
            if (other.GetComponent<Enemy>() != null)
            {
                CreateEnemyHotkey(other);
            }
    }

    private void OnTriggerExit2D(Collider2D other) => other.GetComponent<Enemy>()?.FreezeTime(false);

    private void CreateEnemyHotkey(Collider2D other)
    {
        if (keyCodeList.Count < 0) return;
        other.GetComponent<Enemy>().FreezeTime(true);
        GameObject newHotkey = Instantiate(
            hotkeyPrefab, 
            other.transform.position + new Vector3(0, 2),
            Quaternion.identity);
        hotkeyList.Add(newHotkey);
        BlackholeHotkeyController hotkeyController = newHotkey.GetComponent<BlackholeHotkeyController>();
        KeyCode chosenKeyCode = keyCodeList[Random.Range(0, keyCodeList.Count)];
        keyCodeList.Remove(chosenKeyCode);
        hotkeyController.SetHotkey(chosenKeyCode,this,other.transform);
    }

    public void AddEnemyToList(Transform _enemy) => enemyList.Add(_enemy);

}
