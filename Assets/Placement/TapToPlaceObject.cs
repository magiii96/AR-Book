using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
//using UnityEngine.Experimental.XR

using UnityEngine.XR.ARSubsystems;
using System;

public class TapToPlaceObject : MonoBehaviour
{
    public GameObject placementIndicator;
    public GameObject character;

    //private ARSessionOrigin arOrigin;
    private Pose PlacementPose;
    private ARRaycastManager aRRaycastManager;
    private bool placementPoseIsValid = false;
    private bool oneobject = false;
    public bool characterOn = true;
    public bool release = false;
    public bool reset = false;

    void Start()
    {
        //arOrigin = FindObjectOfType<ARSessionOrigin>();
        aRRaycastManager = FindObjectOfType<ARRaycastManager>();
        GameObject mobile = GameObject.Find("MobileSingleStickControl");
        //GameObject ghostbook = GameObject.Find("GhostBook");
        //GameObject castle = GameObject.Find("Castle");
        //GameObject ghostbook = GameObject.Find("GhostBook");
        mobile.transform.GetChild(1).gameObject.SetActive(false);
        mobile.transform.GetChild(2).gameObject.SetActive(false);
    }

    void Update()
    {
        UpdatePlacementPose();
        UpdatePlacementIndicator();
        if (placementPoseIsValid && Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            {   if(!oneobject){
                    PlaceObject();
                    oneobject = true;
                }
            
            }

        if(!characterOn){
            DeActiveCha();
        }else{
            if(release){
                ActiveCha();
            }
        }
        
        if(reset){
            ResetAll();
        }
        
        if(oneobject){
            placementIndicator.SetActive(false);
        }else{
            placementIndicator.SetActive(true);
        }
    }

    private void ActiveCha(){
        PlaceObject();
        oneobject = true;
        GameObject cha = GameObject.Find("character");
        GameObject shipObject = GameObject.Find("ship");
        if (shipObject != null){
            ShipController shipc = shipObject.GetComponent<ShipController>();
            shipc.enabled = false;
            shipObject.GetComponent<Rigidbody>().isKinematic = true;
        }
        if (cha != null){
            manController manc = cha.GetComponent<manController>();
            manc.enabled = true;
        }
        release = false;
    }

    private void ResetAll(){
        GameObject cha = GameObject.Find("character");
        GameObject shipObject = GameObject.Find("ship");
        if (cha != null){
            manController manc = cha.GetComponent<manController>();
            manc.enabled = true;
            Destroy(cha);
        }
        if (shipObject != null){
            ShipController shipc = shipObject.GetComponent<ShipController>();
            shipc.enabled = false;
            shipObject.GetComponent<Rigidbody>().isKinematic = true;
        }
        GameObject mobile = GameObject.Find("MobileSingleStickControl");
        mobile.transform.GetChild(1).gameObject.SetActive(false);
        mobile.transform.GetChild(2).gameObject.SetActive(false);
        oneobject = false;
        characterOn = true;
        release = false;
        reset = false;
    }


    private void DeActiveCha() {
        GameObject cha = GameObject.Find("character");
        GameObject shipObject = GameObject.Find("ship");
        if (cha != null){
            manController manc = cha.GetComponent<manController>();
            manc.enabled = false;
            Destroy(cha);
            }
        if (shipObject != null){
            ShipController shipc = shipObject.GetComponent<ShipController>();
            shipc.enabled = true;
            shipObject.GetComponent<Rigidbody>().isKinematic = false;
        }
        characterOn = true;
        
    }

    private void PlaceObject()
    {
        GameObject chaClone = (GameObject) Instantiate(character, PlacementPose.position, PlacementPose.rotation);
        chaClone.name = "character";
    }

    private void UpdatePlacementIndicator()
    {
        if (placementPoseIsValid)
        {
            placementIndicator.SetActive(true);
            placementIndicator.transform.SetPositionAndRotation(PlacementPose.position, PlacementPose.rotation);
        }
        else
		{
            placementIndicator.SetActive(false);
		}
	}

    private void UpdatePlacementPose()
	{
        var screenCenter = Camera.current.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));
        var hits = new List<ARRaycastHit>();
        aRRaycastManager.Raycast(screenCenter, hits, TrackableType.Planes);

        placementPoseIsValid = hits.Count > 0;
        if (placementPoseIsValid)
		{
            PlacementPose = hits[0].pose;
            var cameraForward = Camera.current.transform.forward;
            var cameraBearing = new Vector3(cameraForward.x, 0, cameraForward.z).normalized;
            PlacementPose.rotation = Quaternion.LookRotation(cameraBearing);
		}
	}
}