using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class S0Mgr : MonoBehaviour
{

    public Button btnFun1;
    public Button btnFun2;
    public PointerButton btn360;

    
    void Start()
    {
        btnFun1.onClick.AddListener(OnBtnFun1Click);
        btnFun2.onClick.AddListener(OnBtnFun2Click);
    }

    void Update()
    {
        
    }
    
    void OnBtnFun1Click()
    {
        GameDb.LoadingSceneAsync("S1");
    }

    void OnBtnFun2Click()
    {
        GameDb.LoadingSceneAsync("S2");
        //GameDb.LoadingScene("S2");
    }
}
