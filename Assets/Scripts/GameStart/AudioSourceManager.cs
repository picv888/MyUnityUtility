﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// AudioSource管理器，负责播放背景音乐、短小的音乐片段，用字典对音乐文件进行缓存
/// </summary>
public class AudioSourceManager : MonoBehaviour {
    private static AudioSourceManager instance;
    public static AudioSourceManager Instance {
        get {
            return instance;
        }
    }

    private void Awake() {
        instance = this;
        Init();
    }

    private AudioSource audioSource;
    private Dictionary<string, AudioClip> dict;
    private void Init() {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.loop = true;
        dict = new Dictionary<string, AudioClip>();
    }

    /// <summary>
    /// 从字典里获取音乐片段，如果找不到则从Resource中找，还找不到就返回null
    /// </summary>
    /// <returns>The audio clip.</returns>
    /// <param name="clipPath">Clip path.</param>
    public AudioClip GetAudioClip(string clipPath) {
        if (string.IsNullOrEmpty(clipPath)) {
            return null;
        }
        AudioClip clip = null;
        if (!dict.TryGetValue(clipPath, out clip)) {
            clip = Resources.Load<AudioClip>(clipPath);
            if (clip == null) {
                Debug.Log("找不到音乐文件: " + clipPath);
            }else{
                dict[clipPath] = clip;
            }
        }
        return clip;
    }

    /// <summary>
    /// 根据指定的路径（相对Resource）,播放音乐片段
    /// </summary>
    /// <returns>The play.</returns>
    /// <param name="clipPath">Clip path.</param>
    public void PlayOneShot(string clipPath) {
        AudioClip clip = GetAudioClip(clipPath);
        if (clip != null) {
            audioSource.PlayOneShot(clip);
        }
    }

    /// <summary>
    /// 根据指定的路径（相对Resource）,播放背景音乐
    /// </summary>
    /// <returns>The play.</returns>
    /// <param name="clipPath">Clip path.</param>
    public void PlayBGM(string clipPath){
        AudioClip clip = GetAudioClip(clipPath);
        if (clip != null) {
            if (audioSource.clip == clip && audioSource.isPlaying) {
                return;
            }
            audioSource.clip = clip;
            audioSource.Play();
        }
    }

    #region 测试
    //public void OnGUI() {
    //    if (GUILayout.Button("abc/Kardusen")) {
    //        AudioSourceManager.Instance.PlayBGM("abc/Kardusen");
    //    }

    //    if (GUILayout.Button("abc/SnowLand")) {
    //        AudioSourceManager.Instance.PlayBGM("abc/SnowLand");
    //    }

    //    if (GUILayout.Button("Player GunShot")) {
    //        AudioSourceManager.Instance.PlayOneShot("Player GunShot");
    //    }

    //    if (GUILayout.Button("查看字典")) {
    //        foreach (var item in dict) {
    //            Debug.Log(item.Key +" : "+item.Value);
    //        }
    //    }
    //}
    #endregion
}