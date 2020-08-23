using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
 
//import the libraries below
using UnityEngine.UI;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.XR.ARFoundation;

public class TrackedObjectInfoManager : MonoBehaviour 
{

    // [SerializeField]
    // [Tooltip("The camera")]
    // Camera m_WorldSpaceCanvasCamera;

    [SerializeField]
    private GameObject arVideo;

    // public Camera WorldSpaceCanvasCamera
    // {
    //     get{ return m_WorldSpaceCanvasCamera;}
    //     set{ m_WorldSpaceCanvasCamera = value;}
    // }

	private ARTrackedObjectManager m_TrackedObjectManager;

	void Awake()
    {
        m_TrackedObjectManager = GetComponent<ARTrackedObjectManager>();
        
        
    }

    void OnEnable()
    {
        m_TrackedObjectManager.trackedObjectsChanged += OnTrackedObjectChanged;
    }

    void OnDisable()
    {
        m_TrackedObjectManager.trackedObjectsChanged -= OnTrackedObjectChanged;
    }

    void OnTrackedObjectChanged(ARTrackedObjectsChangedEventArgs eventArgs)
    {
        foreach (var trackedObject in eventArgs.added)
        {
            // Give the initial image a reasonable default scale
            trackedObject.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            GameObject newARObject = Instantiate(arVideo, Vector3.zero, Quaternion.identity);
            if(trackedObject.name == "video"){
              
                newARObject.SetActive(true);
            }else{
                newARObject.SetActive(true);
            }
        }
    }
}
