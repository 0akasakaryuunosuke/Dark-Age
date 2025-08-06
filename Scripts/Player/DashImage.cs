using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashImage : MonoBehaviour
{
   private SpriteRenderer sr;
   private int colorLooseSpeed;

   public void SetUpImage(Sprite _sr, int _colorLooseSpeed,int _facingDir)
   {
       sr = GetComponent<SpriteRenderer>();
       sr.sprite = _sr;
      colorLooseSpeed = _colorLooseSpeed;
      transform.localScale = new Vector3(_facingDir, 1, 1);
   }

   private void Update()
   {
       var color = sr.color;
       color.a -= colorLooseSpeed * Time.deltaTime;
       sr.color = color;
       if(color.a<=0)
           Destroy(gameObject);
   }
}
