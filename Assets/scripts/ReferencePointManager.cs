using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARRaycastManager))]
[RequireComponent(typeof(ARAnchorManager))]
[RequireComponent(typeof(ARPlaneManager))]
public class ReferencePointManager : MonoBehaviour
{
    //[SerializeField]
    //private Text debugLog;

    //[SerializeField]
    //private Text referencePointCount;

    //[SerializeField]
    //private Button toggleButton;

    //[SerializeField]
    //private Button clearReferencePointsButton;

    private ARRaycastManager arRaycastManager;

    private ARAnchorManager ARAnchorManager;

    private ARPlaneManager arPlaneManager;

    private List<ARAnchor> Anchors = new List<ARAnchor>();

    private static List<ARRaycastHit> hits = new List<ARRaycastHit>(); 

    private bool oneanchor = false;
    
    void Awake() 
    {
        arRaycastManager = GetComponent<ARRaycastManager>();
        ARAnchorManager = GetComponent<ARAnchorManager>();
        arPlaneManager = GetComponent<ARPlaneManager>();

        //toggleButton.onClick.AddListener(TogglePlaneDetection);
        //clearReferencePointsButton.onClick.AddListener(ClearReferencePoints);

        //debugLog.gameObject.SetActive(false);
    }

    void Update()
    {
        if(Input.touchCount == 0)
            return;

        Touch touch = Input.GetTouch(0);

        if(touch.phase != TouchPhase.Began)
            return;

        if(arRaycastManager.Raycast(touch.position, hits, TrackableType.PlaneWithinPolygon) && (!oneanchor))
        {  
            Pose hitPose = hits[0].pose;
            ARAnchor Anchor = ARAnchorManager.AddAnchor(hitPose);
            oneanchor = true;

            if(Anchor == null)
            {
                //debugLog.gameObject.SetActive(true);
                string errorEntry = "There was an error creating a reference point\n";
                Debug.Log(errorEntry);
                //debugLog.text += errorEntry; 
            }
            else 
            {
                //debugLog.gameObject.SetActive(true);
                //string errorEntry = "hitPose.position";
                //Debug.Log(hitPose.position);
                //debugLog.text = (hitPose.position.ToString());
                Anchors.Add(Anchor);
                //referencePointCount.text = $"Reference Point Count: {referencePoints.Count}";
            }
        }
    }

    // private void TogglePlaneDetection()
    // {
    //     arPlaneManager.enabled = !arPlaneManager.enabled;

    //     foreach(ARPlane plane in arPlaneManager.trackables)
    //     {   
    //         plane.gameObject.SetActive(arPlaneManager.enabled);
    //     }
        
    //     toggleButton.GetComponentInChildren<Text>().text = arPlaneManager.enabled ? 
    //         "Disable Plane Detection" : "Enable Plane Detection";
    // }

    // private void ClearReferencePoints()
    // {
    //     foreach(ARReferencePoint referencePoint in referencePoints)
    //     {
    //         arReferencePointManager.RemoveReferencePoint(referencePoint);
    //     }
    //     referencePoints.Clear();
    //     referencePointCount.text = $"Reference Point Count: {referencePoints.Count}";
    // }
}