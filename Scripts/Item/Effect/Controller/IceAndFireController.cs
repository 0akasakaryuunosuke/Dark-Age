using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceAndFireController : LightingAttackController
{
     private Rigidbody2D rb;
    [SerializeField] private Vector2 velocity;

    public void SetUp(int facingDir)
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2(velocity.x*facingDir,0);
    }
    
 
}
