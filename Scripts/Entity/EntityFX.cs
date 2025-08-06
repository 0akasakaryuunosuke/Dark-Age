using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using TMPro;

public class EntityFX : MonoBehaviour
{
    private SpriteRenderer sr;
    private Player player;
    [Header("受伤效果")] 
    [SerializeField] private Material hitMaterial;
    private Material originalMaterial;

    [Header("疾病效果")]
    [SerializeField] private Color[] igniteColor;
    [SerializeField] private Color chillColor;
    [SerializeField] private Color[] shockColor;
    [SerializeField] private ParticleSystem igniteFX;
    [SerializeField] private ParticleSystem chillFX;
    [SerializeField] private ParticleSystem shockFX;

    [SerializeField] private ParticleSystem dustFX;
    [SerializeField] private GameObject criticalFXPrefab;

    [Header("残影效果")] 
    [SerializeField] private GameObject dashImagePrefab;
    [SerializeField] private int looseSpeed;

    [Header("镜头抖动")] 
    private CinemachineImpulseSource screenShake;
    [SerializeField] private Vector3 shakePower;
    [SerializeField] private float shakeMultiply;

    [SerializeField] private GameObject popUpTextPrefab;
    private void Start()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        screenShake = GetComponent<CinemachineImpulseSource>();
        player = PlayerManager.instance.player;
        originalMaterial = sr.material;
    }

    private IEnumerator FlashFX()
    {
        sr.material = hitMaterial;
        yield return new WaitForSeconds(0.2f);
        sr.material = originalMaterial;
    }

    private void RedColorBlink()
    {
        if(sr.color !=Color.white)
            sr.color =Color.white;
        else
        {
            sr.color = Color.red;
        }
    }

    private void CancelColorBlink()
    {
        CancelInvoke();
        sr.color = Color.white;
        igniteFX.Stop();
        chillFX.Stop();
        shockFX.Stop();
    }

    public void IgniteFor(float _seconds)
    {
        igniteFX.Play();
        InvokeRepeating("IgniteFX",0,.3f);
        Invoke("CancelColorBlink",_seconds);
    }

    public void ChillFor(float _seconds)
    {
        chillFX.Play();
        InvokeRepeating("ChillFX",0,.3f);
        Invoke("CancelColorBlink",_seconds);
    }

    public void ShockFor(float _seconds)
    {
        shockFX.Play();
        InvokeRepeating("ShockFX",0,.3f);
        Invoke("CancelColorBlink",_seconds);
    }
    
    private void IgniteFX()
    {
        if (sr.color != igniteColor[0])
            sr.color = igniteColor[0];
        else
            sr.color = igniteColor[1];
    }

    private void ChillFX() => sr.color = chillColor;

    private void ShockFX()
    {
        if (sr.color != shockColor[0])
            sr.color = shockColor[0];
        else
            sr.color = shockColor[1];
    }


    public void CreateCriticalFX(Transform _target,int _facingDir)
    {
        GameObject criticalFX = Instantiate(criticalFXPrefab, _target.position, Quaternion.identity);
        criticalFX.transform.localScale = new Vector3(_facingDir, 1, 1);
        Destroy(criticalFX,.7f);
    }

    public void PlayDustFX()
    {
        if (dustFX != null)
            dustFX.Play();
    }

    public void CreateDashImage(int _facingDir)
    {
        GameObject newImage = Instantiate(dashImagePrefab, transform.position, Quaternion.identity);
        newImage.GetComponent<DashImage>().SetUpImage(sr.sprite,looseSpeed,_facingDir);
    }

    public void ScreenShake(float _shakeMultiply)
    {
        screenShake.m_DefaultVelocity = new Vector3(shakePower.x * player.facingDir, shakePower.y)*_shakeMultiply;
        screenShake.GenerateImpulse();
    }

    public void CreatePopUpText(string _text)
    {
        GameObject text = Instantiate(popUpTextPrefab, transform.position+new Vector3(0,2,0), Quaternion.identity);
        text.GetComponent<TextMeshPro>().text = _text;
    }
}
