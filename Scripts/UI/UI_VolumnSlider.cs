using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class UI_VolumnSlider : MonoBehaviour
{
  [SerializeField] private AudioMixer audioMixer;
  [SerializeField] private float multiplier;
  public Slider slider;
  public string parameter;

  public void SetValue(float _value)
  {
    audioMixer.SetFloat(parameter, Mathf.Log10(_value) * multiplier);
  }
  
  
}
