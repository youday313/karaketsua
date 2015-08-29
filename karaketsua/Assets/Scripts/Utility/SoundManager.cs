using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

#region ::音量クラス::
[Serializable]
public class SoundVolume
{
    public float BGM = 1.0f;
    public float Voice = 1.0f;
    public float SE = 1.0f;
    public bool Mute = false;

    public void Init()
    {
        BGM = 1.0f;
        Voice = 1.0f;
        SE = 1.0f;
        Mute = false;
    }
}
#endregion

// 音管理クラス
public class SoundManager : DontDestroySingleton<SoundManager>
{

    // 音量
    public SoundVolume volume = new SoundVolume();

    //Audio種類
    public enum AudioKind { BGM, SE, Voice };
    // === AudioSource ===
    // BGM
    private AudioSource BGMsource;
    //同時再生数は4
    // SE
    private AudioSource[] SEsources = new AudioSource[4];
    // 音声
    private AudioSource[] VoiceSources = new AudioSource[4];

    // === AudioClip ===
    // BGM
    public AudioClip[] BGM;
    // SE
    public AudioClip[] SE;
    // 音声
    public AudioClip[] Voice;

    //名前からindexを取得する
    private Dictionary<string, int> BGMNameDictionary = new Dictionary<string, int>();
    private Dictionary<string, int> SENameDictionary = new Dictionary<string, int>();
    private Dictionary<string, int> VoiceNameDictionary = new Dictionary<string, int>();

    //設定
    public bool is2DSound = false;


    void Awake()
    {
        base.onAwake();

        // 全てのAudioSourceコンポーネントを追加する

        // BGM AudioSource
        BGMsource = gameObject.AddComponent<AudioSource>();
        // BGMはループを有効にする
        BGMsource.loop = true;

        // SE AudioSource
        for (int i = 0; i < SEsources.Length; i++)
        {
            SEsources[i] = gameObject.AddComponent<AudioSource>();

        }

        // 音声 AudioSource
        for (int i = 0; i < VoiceSources.Length; i++)
        {
            VoiceSources[i] = gameObject.AddComponent<AudioSource>();
        }
    }

    void Start()
    {
        #region ::ファイル名を辞書に登録::
        //BGM
        foreach (var i in BGM.Select((bgm, index) => new { bgm, index }))
        {
            BGMNameDictionary.Add(i.bgm.name, i.index);
        }
        //SE
        foreach (var i in SE.Select((bgm, index) => new { bgm, index }))
        {
            SENameDictionary.Add(i.bgm.name, i.index);
        }
        //Voice
        foreach (var i in Voice.Select((bgm, index) => new { bgm, index }))
        {
            VoiceNameDictionary.Add(i.bgm.name, i.index);
        }
        #endregion

        //debug
        Set2DSound();
        SetVolume();

    }

    void Update()
    {

    }
    public void SetVolume()
    {
        // ミュート設定
        BGMsource.mute = volume.Mute;
        foreach (AudioSource source in SEsources)
        {
            source.mute = volume.Mute;
        }
        foreach (AudioSource source in VoiceSources)
        {
            source.mute = volume.Mute;
        }

        // ボリューム設定
        BGMsource.volume = volume.BGM;
        foreach (AudioSource source in SEsources)
        {
            source.volume = volume.SE;
        }
        foreach (AudioSource source in VoiceSources)
        {
            source.volume = volume.Voice;
        }
    }

    //ファイル名から再生
    public void PlaySoundFromName(string audioName,AudioKind source)
    {
        switch (source)
        {
            case AudioKind.BGM:
                PlayBGM(BGMNameDictionary[audioName]);
                break;
            case AudioKind.SE:
                PlaySE(SENameDictionary[audioName]);
                break;
            case AudioKind.Voice:
                PlayVoice(VoiceNameDictionary[audioName]);
                break;
            default:
                Debug.Log("Cannot find " + audioName);
                break;
        }
    }


    // ***** BGM再生 *****
    // BGM再生
    public void PlayBGM(int index)
    {
        if (0 > index || BGM.Length <= index)
        {
            return;
        }
        // 同じBGMの場合は何もしない
        if (BGMsource.clip == BGM[index])
        {
            return;
        }
        BGMsource.Stop();
        BGMsource.clip = BGM[index];
        BGMsource.Play();
    }

    // BGM停止
    public void StopBGM()
    {
        BGMsource.Stop();
        BGMsource.clip = null;
    }


    // ***** SE再生 *****
    // SE再生
    public void PlaySE(int index)
    {
        if (0 > index || SE.Length <= index)
        {
            return;
        }

        // 再生中で無いAudioSouceで鳴らす
        foreach (AudioSource source in SEsources)
        {
            if (false == source.isPlaying)
            {
                source.clip = SE[index];
                source.Play();
                return;
            }
        }
    }

    // SE停止
    public void StopSE()
    {
        // 全てのSE用のAudioSouceを停止する
        foreach (AudioSource source in SEsources)
        {
            source.Stop();
            source.clip = null;
        }
    }


    // ***** 音声再生 *****
    // 音声再生
    public void PlayVoice(int index)
    {
        if (0 > index || Voice.Length <= index)
        {
            return;
        }
        // 再生中で無いAudioSouceで鳴らす
        foreach (AudioSource source in VoiceSources)
        {
            if (false == source.isPlaying)
            {
                source.clip = Voice[index];
                source.Play();
                return;
            }
        }
    }

    // 音声停止
    public void StopVoice()
    {
        // 全ての音声用のAudioSouceを停止する
        foreach (AudioSource source in VoiceSources)
        {
            source.Stop();
            source.clip = null;
        }
    }

    //2Dモード
    void Set2DSound()
    {
        if (is2DSound == true)
        {
            BGMsource.spatialBlend = 0;
            foreach (var source in SEsources)
            {
                source.spatialBlend = 0;
            }
            foreach (var source in VoiceSources)
            {
                source.spatialBlend = 0;
            }
        }
    }


}