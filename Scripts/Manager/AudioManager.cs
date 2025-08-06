using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    [SerializeField] private AudioSource[] sfx;
    [SerializeField] private AudioSource[] bgm;
    [SerializeField] private int limitSFxPlayingDistance;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(instance.gameObject);
        }
        else
        {
            instance = this;
        }
    }

    public void PlayerSoundEffect(int _index,Transform _source)
    {
        if (sfx[_index].isPlaying)
            return;

        if (_source!=null&&Vector2.Distance(PlayerManager.instance.player.transform.position, _source.position) <
            limitSFxPlayingDistance)
            return;
        
        if(_index<sfx.Length)
            sfx[_index].Play();
    }

    public void StopSoundEffect(int _index)
    {
        if(_index<sfx.Length)
            sfx[_index].Stop();
    }

    public void StopAllBGM()
    {
        for (int i = 0; i < bgm.Length; i++)
        {
            bgm[i].Stop();
        }
    }

    public void PlayBGM(int _index)
    {
        StopAllBGM();
        bgm[_index].Play();
        Debug.Log("播放bgm:"+bgm[_index].name);
       
    }



    public void PlayBattleBGM()
    {
        if (bgm[1].isPlaying)
            PlayBGM(1);
    }
}
