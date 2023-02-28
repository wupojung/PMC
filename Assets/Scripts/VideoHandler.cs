using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class VideoHandler : MonoBehaviour
{
    public List<VideoClip> videoClipList;

    private VideoPlayer _videoPlayer;

    public VideoClip BeginVideo;

    private void Awake()
    {
        _videoPlayer = GetComponent<VideoPlayer>();
    }

    private void OnEnable()
    {
        gameObject.SetActive(true);

        //重新校正至 1920 : 1080 的比例
        int height = Screen.width * 1080 / 1920;
        RectTransform rt =(gameObject.transform.GetChild(0).transform as RectTransform); 
        rt.sizeDelta= new Vector2(rt.sizeDelta.x,height);

        if (_videoPlayer.isPlaying)
        {
            _videoPlayer.Stop();
        }

        _videoPlayer.clip = BeginVideo;
        Debug.Log($"Name={_videoPlayer.clip.name}");
        _videoPlayer.Play();
    }


    private void Start()
    {
        _videoPlayer.loopPointReached += CheckOver;
    }

    void Update()
    {
    }


    void CheckOver(VideoPlayer vp)
    {
        Debug.Log("Video Stop Now!!");
        gameObject.SetActive(false);
    }


    public void Play(int index)
    {
        Debug.Log($"Get index={index} from VideoHandler");
        gameObject.SetActive(true);
        if (_videoPlayer.isPlaying)
        {
            _videoPlayer.Stop();
        }

        _videoPlayer.clip = videoClipList[index];
        Debug.Log($"Name={_videoPlayer.clip.name}");
        _videoPlayer.Play();
    }

    public bool IsPlaying => (_videoPlayer != null && _videoPlayer.isPlaying);

    public void Stop()
    {
        if (_videoPlayer != null && _videoPlayer.isPlaying)
        {
            _videoPlayer.Stop();
        }

        gameObject.SetActive(false);
    }
}