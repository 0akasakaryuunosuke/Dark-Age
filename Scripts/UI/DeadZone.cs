using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Player>() != null)
        {
            other.GetComponent<PlayerStat>().Die();
        }
        else if (other.GetComponent<Enemy>() != null)
        {
            other.GetComponent<Enemy>().Die();
        }
        else 
            Destroy(other.gameObject);
    }
}
