
using System;
using TMPro;
using UnityEngine;

public class PopUpText : MonoBehaviour
{
   private TextMeshPro textMeshPro;
   [SerializeField] private float speed;
   [SerializeField] private float fadeSpeed;

   private void Start()
   {
      textMeshPro = GetComponent<TextMeshPro>();
   }

   private void Update()
   {
      transform.position = Vector3.MoveTowards(transform.position, transform.position+new Vector3(0,1), speed * Time.deltaTime);

      var color = textMeshPro.color;
      color.a -= fadeSpeed * Time.deltaTime;
      textMeshPro.color = color;
      if (textMeshPro.color.a <= 0)
         Destroy(gameObject);
   }
}
