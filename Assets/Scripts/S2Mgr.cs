using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class S2Mgr : MonoBehaviour
{
    public Transform transTarget;
    public Button btnZoomIn;
    public Button btnZoomOut;


    public PointerButton btn360;
    public float rotateDeg = 40;


    public VideoHandler videoHandler;
    public SidePanelHandler sidePanelHandler;

    public Button btnHome;


    public Sprite imgBack;
    public Sprite imgHome;
    public Sprite imgSkip;
    private Image imgButton;

    private Vector3 _scale = new Vector3(1, 1, 1);

    private Touch oldTouch1, oldTouch2;


    private void Awake()
    {
        imgButton = btnHome.gameObject.transform.GetComponent<Image>();
    }

    void Start()
    {
        btnZoomIn.onClick.AddListener(OnBtnZoomInClick);
        btnZoomOut.onClick.AddListener(OnBtnZoomOutClick);
        //btnSidePanel.onClick.AddListener(OnBtnOpenSideMenuClick);
        //_isSidePanelExpand = false;

        btnHome.onClick.AddListener(OnBtnHomeClick);

        sidePanelHandler.PlayAnimationEvent += OnPlayAnimation;

      
    }

    private void OnEnable()
    {
        videoHandler.gameObject.SetActive(true);
        imgButton.sprite = imgSkip;
    }

    void Update()
    {
        /*
        if (btn360.Pressed)
        {
            //Debug.Log(btn360.Pressed);

            float rotX = Input.GetAxis("Mouse X") * rotateDeg * Mathf.Deg2Rad;
            transTarget.Rotate(Vector3.up, -rotX);

            float rotY = Input.GetAxis("Mouse Y") * rotateDeg * Mathf.Deg2Rad;
            transTarget.Rotate(Vector3.right, rotY);
        }
        */
        //Debug.Log(Input.touchCount);


        ProcessOneTouch();
        ProcessTwoTouch();
    }

    public float ClickDuration = 2;

    //public UnityEvent OnLongClick;
    private float totalDownTime = 0;
    private bool clicking = false;
    private bool isLong = false;

    void ProcessOneTouch()
    {
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);
            Vector2 pos = touch.deltaPosition;
            //Debug.Log($"touch one {pos}");

            if (!clicking)
            {
                //first time clicing 
                totalDownTime = 0;
                clicking = true;
            }

            if (clicking && Input.GetMouseButton(0))
            {
                totalDownTime += Time.deltaTime;

                if (totalDownTime >= ClickDuration)
                {
                    Debug.Log("Long click");
                    //clicking = false;
                    isLong = true;
                    //OnLongClick.Invoke();
                }
            }

            if (isLong)
            {
                transTarget.Translate(Vector3.down * pos.x, Space.World);
                transTarget.Translate(Vector3.right * pos.y, Space.World);
            }
            else
            {
                transTarget.Rotate(Vector3.down * pos.x, Space.World);
                transTarget.Rotate(Vector3.right * pos.y, Space.World);
            }
        }
        else
        {
            clicking = false;
            isLong = false;
        }
    }

    void ProcessTwoTouch()
    {
        //https://blog.csdn.net/A_Pro_Cat/article/details/105388755
        if (Input.touchCount == 2)
        {
            //
            Touch newTouch1 = Input.GetTouch(0);
            Touch newTouch2 = Input.GetTouch(1);

            if (newTouch2.phase == TouchPhase.Began)
            {
                oldTouch1 = newTouch1;
                oldTouch2 = newTouch2;
                return;
            }

            float oldDistance = Vector2.Distance(oldTouch1.position, oldTouch2.position);
            float newDistance = Vector2.Distance(newTouch1.position, newTouch2.position);

            float offset = newDistance - oldDistance;


            if (offset > 0)
            {
                OnBtnZoomInClick();
            }

            if (offset < 0)
            {
                OnBtnZoomOutClick();
            }

            /*
            float scaleFactor = offset / 100;
            Vector3 scale = transTarget.localScale * scaleFactor;
            Debug.Log($"{newDistance}-{oldDistance} ={offset}    {scale}");
            if (scale.x > 0.3f && scale.y > 0.3f && scale.z > 0.3f)
            {
                //  transTarget.localScale = scale;
            }
            */

            // write back 
            oldTouch1 = newTouch1;
            oldTouch2 = newTouch2;
        }
    }

    void OnBtnZoomInClick()
    {
        _scale *= 1.1f;
        transTarget.localScale = _scale;
    }

    void OnBtnZoomOutClick()
    {
        _scale *= 0.9f;
        transTarget.localScale = _scale;
    }

    private void OnBtnHomeClick()
    {
        if (videoHandler.IsPlaying)
        {
            imgButton.sprite = imgHome;
            videoHandler.Stop();
        }
        else
        {
            SceneManager.LoadScene("S0");
        }
    }

    public void OnPlayAnimation(int index)
    {
        Debug.Log($"Get index={index} from S2Mgr");
        imgButton.sprite = imgBack;
        videoHandler.Play(index);
    }


    /*
    void OnBtnOpenSideMenuClick()
    {
        Vector3 newPos = new Vector3(-1200, 0, 0); //界外
        if (!_isSidePanelExpand)
        {
            newPos = new Vector3(-680, 0, 0);
        }

        transSidePanel.localPosition = newPos;
        _isSidePanelExpand = !_isSidePanelExpand;
        
        videoHandler.Play();
    }
    */
}