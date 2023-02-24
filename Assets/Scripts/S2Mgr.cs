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

    public Button btnReset;
    public Button btnRule;
    private bool showRule = true;

    public GameObject ruleObj;

    public float rotateDeg = 40;

    public VideoHandler videoHandler;
    public SidePanelHandler sidePanelHandler;

    public Button btnHome;

    public Sprite imgBack;
    public Sprite imgHome;
    public Sprite imgSkip;
    private Image _imgButton;

    public FixedJoystick hJoystick;

    public FixedJoystick joystick;

    private Vector3 _scale = new Vector3(1, 1, 1);

    private Touch oldTouch1, oldTouch2;

    private bool isDirty = true;

    public Vector3 defaultPosition = new Vector3(-0.5f, -1f, -5f);
    public Vector3 defaultRotation = new Vector3(0, 20, 0);
    public Vector3 defaultScale = new Vector3(1, 1, 1);


    private void Awake()
    {
        _imgButton = btnHome.gameObject.transform.GetComponent<Image>();
        btnRule.onClick.AddListener(OnBtnRuleClick);
        btnReset.onClick.AddListener(OnBtnResetClick);
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
        _imgButton.sprite = imgSkip;
        
        OnBtnResetClick();
    }
    void Start()
    {
      
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

        if (isDirty)
        {
            ruleObj.SetActive(showRule);
            isDirty = false;
        }


        ProcessJoystick();
        ProcessOneTouch();
        ProcessTwoTouch();
    }

    void ProcessJoystick()
    {
        if (joystick.Direction != Vector2.zero)
        {
            Debug.Log($"joystick:{joystick.Direction}");
            Vector3 position = new Vector3(joystick.Direction.x, 0, joystick.Direction.y);
            transTarget.Translate(position * Time.deltaTime); //位移方法
        }

        if (hJoystick.Direction != Vector2.zero)
        {
            Debug.Log($"H joystick:{hJoystick.Direction}");
            Vector3 position = new Vector3(0, hJoystick.Direction.y, 0);
            transTarget.Translate(position * Time.deltaTime); //位移方法
        }
    }

    void ProcessOneTouch()
    {
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);
            Vector2 pos = touch.deltaPosition;
            //Debug.Log($"touch one {pos}");

            transTarget.Rotate(Vector3.down * pos.x, Space.World);
            transTarget.Rotate(Vector3.right * pos.y, Space.World);
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

    void OnBtnResetClick()
    {
        transTarget.localPosition = defaultPosition;
        transTarget.localEulerAngles = defaultRotation;
        transTarget.localScale = defaultScale;
    }

    void OnBtnRuleClick()
    {
        showRule = !showRule;
        isDirty = true;
    }

    void OnBtnZoomInClick()
    {
        _scale *= 1.1f;
        Debug.Log($"Scale :{_scale}");
        transTarget.localScale = _scale;
    }

    void OnBtnZoomOutClick()
    {
        _scale *= 0.9f;
        Debug.Log($"Scale :{_scale}");
        transTarget.localScale = _scale;
    }

    private void OnBtnHomeClick()
    {
        if (videoHandler.IsPlaying)
        {
            _imgButton.sprite = imgHome;
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
        _imgButton.sprite = imgBack;
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