using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

public class S1Mgr : MonoBehaviour
{
    public Button btnReset;
    //public Button btnPlay;

    public Button btnZoomIn;
    public Button btnZoomOut;
    public Button btnHome;

    public PointerButton btn360;
    public float rotateDeg = 40;

    public GameObject arSessionOrigin;

    public Button btnRule;
    private bool showRule = true;


    private ARTapToPlaceObject _arTapToPlaceObject;
    private ARPlaneManager _arPlaneManager;
    private bool isReset = false;


    private void Awake()
    {
        if (arSessionOrigin != null)
        {
            _arTapToPlaceObject = arSessionOrigin.GetComponent<ARTapToPlaceObject>();
            _arPlaneManager = arSessionOrigin.GetComponent<ARPlaneManager>();
        }
    }

    void Start()
    {
        btnReset.onClick.AddListener(OnBtnResetClick);
        //btnPlay.onClick.AddListener(OnBtnPlayClick);
        btnHome.onClick.AddListener(OnBtnHomeClick);

        btnRule.onClick.AddListener(OnBtnRuleClick);
        btnZoomIn.onClick.AddListener(OnZoomInClick);
        btnZoomOut.onClick.AddListener(OnZoomOutClick);
    }

    void Update()
    {
        if (isReset)
        {
            foreach (var plane in _arPlaneManager.trackables)
            {
                plane.gameObject.SetActive(false);
            }

            _arTapToPlaceObject.DestroySpawnedObject();
            isReset = false;
        }
        /*
        if (btn360.Pressed)
        {
            //Debug.Log(btn360.Pressed);

            float rotX = Input.GetAxis("Mouse X") * rotateDeg * Mathf.Deg2Rad;
            _arTapToPlaceObject.transform.Rotate(Vector3.up, -rotX);

            float rotY = Input.GetAxis("Mouse Y") * rotateDeg * Mathf.Deg2Rad;
            _arTapToPlaceObject.transform.Rotate(Vector3.right, rotY);
        }
        */
    }

    void OnBtnResetClick()
    {
        isReset = true;
    }

    void OnBtnPlayClick()
    {
        Debug.Log("OnBtnPlayClick");

        GameObject spawned = _arTapToPlaceObject.GetSpawnedObject();
        Debug.Log($"spawned==null? =>  {spawned == null}");

        if (spawned != null)
        {
            Animator animator = spawned.GetComponent<Animator>();
            Debug.Log($"animator==null? =>  {animator == null}");

            animator.Play("Take 001", 0, 0f);
        }
    }

    void OnBtnHomeClick()
    {
        GameDb.LoadingSceneAsync("S0");
    }


    void OnBtnRuleClick()
    {
        GameObject spawned = _arTapToPlaceObject.GetSpawnedObject();
        if (spawned != null)
        {
            for (int i = 0; i < spawned.transform.childCount; i++)
            {
                if (spawned.transform.GetChild(i).name.Equals("Measure_Line_Grp"))
                {
                    showRule = !showRule;
                    spawned.transform.GetChild(i).gameObject.SetActive(showRule);
                }
            }
        }
    }

    void OnZoomInClick()
    {
        GameObject spawned = _arTapToPlaceObject.GetSpawnedObject();
        spawned.transform.localScale *= 1.1f;

        Debug.Log($"Spawned localScale => {spawned.transform.localScale}");
    }

    void OnZoomOutClick()
    {
        GameObject spawned = _arTapToPlaceObject.GetSpawnedObject();
        spawned.transform.localScale /= 1.1f;
        Debug.Log($"Spawned localScale => {spawned.transform.localScale}");
    }
}