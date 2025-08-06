using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThunderStrikeController : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    private int thunderDamage;
    private Transform target;
    private Animator anim;
    private bool trigger;
    void Start()
    {
        anim = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position,target.position,moveSpeed*Time.deltaTime);
        transform.right = transform.position - target.position;
        if (Vector2.Distance(transform.position, target.position) < 1&&!trigger)
        {
            trigger = true;
            anim.SetTrigger("Hit");
        }
        if (trigger)
        {
            anim.transform.localRotation=Quaternion.identity;
            transform.localRotation = Quaternion.identity;
            transform.localScale = new Vector3(3, 3);
            anim.transform.localPosition = new Vector3(0, .5f);
            Invoke("HitTarget", .2f);
        }
    }

    public void SetThunderStrike(Transform _target,int _thunderDamage)
    {
        target = _target;
        thunderDamage = _thunderDamage;
    }

    private void HitTarget()
    {
        target.GetComponent<EntityStat>().TakeDamage(thunderDamage);
        Destroy(gameObject,.6f);
    }
}
