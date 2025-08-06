using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_BlackScreen : MonoBehaviour
{
   private static readonly int Out = Animator.StringToHash("FadeOut");
   private static readonly int In = Animator.StringToHash("FadeIn");
   private Animator animator;

   private void Start()
   {
      animator = GetComponent<Animator>();
   }

   public void FadeIn() => animator.SetTrigger(In);
   public void FadeOut() => animator.SetTrigger(Out);
}
