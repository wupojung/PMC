using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AsyncLoaderMgr : MonoBehaviour
{
    //ready https://blog.csdn.net/qq_35037137/article/details/88826732?spm=1001.2101.3001.6650.3&utm_medium=distribute.pc_relevant.none-task-blog-2%7Edefault%7ECTRLIST%7ERate-3-88826732-blog-83096135.pc_relevant_aa2&depth_1-utm_source=distribute.pc_relevant.none-task-blog-2%7Edefault%7ECTRLIST%7ERate-3-88826732-blog-83096135.pc_relevant_aa2&utm_relevant_index=6

    public Slider loadingSlider;

    public TextMeshProUGUI loadingText;


    private float _loadingSpeed = 1;
    private float _targetValue;
    private AsyncOperation _asyncOperation;

    void Start()
    {
        loadingSlider.value = 0.0f;
        StartCoroutine(AsyncLoading());
    }

    void Update()
    {
        
        if (_asyncOperation == null)
        {
            return;
        }

        _targetValue = _asyncOperation.progress;
        Debug.Log(_targetValue);

        if (_asyncOperation.progress >= 0.9f)
        {
            //值最大为0.9

            _targetValue = 1.0f;
        }

        //为滑动条赋值

        if (_targetValue != loadingSlider.value)
        {
            loadingSlider.value = Mathf.Lerp(loadingSlider.value, _targetValue, Time.deltaTime * _loadingSpeed);

            if (Mathf.Abs(loadingSlider.value - _targetValue) < 0.01f)

            {
                loadingSlider.value = _targetValue;
            }
        }

        loadingText.text = ((int)(loadingSlider.value * 100)).ToString();

        if ((int)(loadingSlider.value * 100) == 100)
        {
            //允许异步加载完毕后自动切换场景
            _asyncOperation.allowSceneActivation = true;
        }
    }

    IEnumerator AsyncLoading()
    {
        /*
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(GameDb.NextLevelName);

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }*/
       
        //异步加载场景
        _asyncOperation = SceneManager.LoadSceneAsync(GameDb.NextLevelName);

        //阻止当加载完成自动切换
        _asyncOperation.allowSceneActivation = false;

        //读取完毕后返回，系统会自动进入C场景
        yield return _asyncOperation;
        
    }
}