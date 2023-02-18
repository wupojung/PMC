using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;


[RequireComponent(typeof(ARRaycastManager))]
public class ARTapToPlaceObject : MonoBehaviour
{

    public GameObject gameObjectToInstantiate;
    private GameObject spawnedObject;
    private ARRaycastManager _arRaycastManager;
    private Vector2 touchPosition;

    private static List<ARRaycastHit> hits = new List<ARRaycastHit>();
    
    private void Awake()
    {
        _arRaycastManager = GetComponent<ARRaycastManager>();
    }

    void Start()
    {
        
    }

    void Update()
    {

        if (!TryGetTouchPosition(out touchPosition))
        {
            return;
        }

        if (_arRaycastManager.Raycast(touchPosition, hits, TrackableType.PlaneWithinPolygon))
        {
            var hitPose = hits[0].pose;
            if (spawnedObject == null)
            {
                spawnedObject = Instantiate(gameObjectToInstantiate, hitPose.position, hitPose.rotation);
            }
            else
            {
                spawnedObject.transform.position = hitPose.position;
            }
        }

    }

    public void DestroySpawnedObject()
    {
        if (spawnedObject != null)
        {
            Destroy(spawnedObject.gameObject);
        }
        spawnedObject = null;
    }

    public GameObject GetSpawnedObject()
    {
        return spawnedObject;
    }

    bool TryGetTouchPosition(out Vector2 touchPostion)
    {
        if (Input.touchCount > 0)
        {
            touchPostion = Input.GetTouch(0).position;
            return true;
        }

        touchPostion = default;
        return false;
    }
    
}
