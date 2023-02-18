using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class SidePanelHandler : MonoBehaviour
{
    public List<Button> btnList;
    public Button btnExpand;
    private bool _isExpanded;

    //private Vector3 _expandedPositon = new Vector3(-680, 0, 0);
    //private Vector3 _collapsedPositon = new Vector3(-1200, 0, 0);

    public delegate void PlayAnimation(int index);

    public PlayAnimation PlayAnimationEvent;


    public float duration = 1.0f;

    private int _currentIndex;
    private bool _isDirty = false;
    private float _sideWidth = 500;
    private string _sideName = "pnlSidePanelMenu";

    private void Awake()
    {
        DOTween.Init();
    }

    void Start()
    {
        //scan for animation button 
        for (int i = 0; i < btnList.Count; i++)
        {
            int tempIndex = i;
            btnList[i].onClick.AddListener(() => { OnBtnAnimationClick(tempIndex); });
        }
        
        
        //scan for target
        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            if (gameObject.transform.GetChild(i).name.Equals(_sideName))
            {
                RectTransform traget = (RectTransform)gameObject.transform.GetChild(i).transform;

                //btnExpand = traget.GetComponent<Button>();

                _sideWidth = traget.rect.width - 40;
                break;
            }
        }
        
        //binding and setting 
        btnExpand.onClick.AddListener(OnBtnExpandClick);
        _currentIndex = 0;

        // action  (reset ui)
        Collapse(); //摺疊
    }

    private void OnEnable()
    {
        Collapse(); //摺疊
    }


    void Update()
    {
        if (_isDirty)
        {
            for (int i = 0; i < 3; i++)
            {
                btnList[i].GetComponent<Image>().enabled = false;
            }

            btnList[_currentIndex].GetComponent<Image>().enabled = true;
            _isDirty = false;
        }
    }

    void OnBtnAnimationClick(int index)
    {
        _currentIndex = index;
        _isDirty = true;
        PlayAnimationEvent?.Invoke(_currentIndex);
        Collapse();
    }

    void OnBtnExpandClick()
    {
        float x = 0;
        if (!_isExpanded)
        {
            x = _sideWidth * -1;
        }

        transform.DOMoveX(x, duration, true);
        _isExpanded = !_isExpanded;
        //videoHandler.Play();
    }

    void Expand()
    {
        transform.DOMoveX(0, duration, true);
        _isExpanded = true;
    }

    void Collapse()
    {
        transform.DOMoveX(_sideWidth * -1, duration, true);
        _isExpanded = false;
    }
}