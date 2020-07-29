using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.UI;
//using UnityEngine.Experimental.XR;

using System;

//[RequireComponent(typeof(ARRaycastManager))]
//[RequireComponent(typeof(ARAnchorManager))]
//[RequireComponent(typeof(ARPlaneManager))]

public class TapToPlaceObject : MonoBehaviour
{

    [SerializeField]
    private Button startanchor;

    [SerializeField]
    private Button CreateCharacter;

    public GameObject placementIndicator;
    public GameObject character;

    private ARSessionOrigin arOrigin;
    private Pose PlacementPose;
    private ARRaycastManager arRaycastManager;
    private ARAnchorManager arAnchorManager;
    private ARPlaneManager arPlaneManager;

    private List<ARAnchor> Anchors = new List<ARAnchor>();
    private static List<ARRaycastHit> hits = new List<ARRaycastHit>(); 
    
    private bool placementPoseIsValid = false;
    private bool oneobject = false;
    public bool characterOn = true;
    public bool release = false;
    public bool reset = false;
    private bool startmain = false;
    private bool canchor = false;
    private bool oneanchor = false;
    private Vector3 anchorPosition;



    void Start()
    {
        arOrigin = FindObjectOfType<ARSessionOrigin>();
        arRaycastManager = FindObjectOfType<ARRaycastManager>();
        arPlaneManager = FindObjectOfType<ARPlaneManager>();
        arAnchorManager = FindObjectOfType<ARAnchorManager>();

        GameObject mobile = GameObject.Find("MobileSingleStickControl");
        //GameObject ghostbook = GameObject.Find("GhostBook");
        //GameObject castle = GameObject.Find("Castle");
        //GameObject ghostbook = GameObject.Find("GhostBook");
        GameObject MainMenu = mobile.transform.Find("MainMenu").gameObject;
        MainMenu.SetActive(false);
        // ReleaseBoat = MainMenu.transform.Find("ReleaseBoat").gameObject;
        // ReleaseBoat.SetActive(false);
        // DriveBoat = MainMenu.transform.Find("DriveBoat").gameObject;
        // DriveBoat.SetActive(false);
        startanchor.onClick.AddListener(CreateAnchor);
        CreateCharacter.onClick.AddListener(CreateCha);
        //mobile.transform.GetChild(1).gameObject.SetActive(false);
        //mobile.transform.GetChild(2).gameObject.SetActive(false);
    }

    void Update()
    {
        if(canchor){
            if(Input.touchCount == 0){
                return;
            }
            Touch touch = Input.GetTouch(0);
            if((!oneanchor)&& arRaycastManager.Raycast(touch.position, hits, TrackableType.PlaneWithinPolygon)){  
                //PlaceAnchor();
                arRaycastManager.Raycast(touch.position, hits, TrackableType.PlaneWithinPolygon);
                Pose hitPose = hits[0].pose;
                anchorPosition = hitPose.position;
                ARAnchor Anchor = arAnchorManager.AddAnchor(hitPose);
                if(Anchor == null){
                    string errorEntry = "There was an error creating a reference point\n";
                    Debug.Log(errorEntry);
                }else{
                    oneanchor = true;
                    Anchors.Add(Anchor);
                    setMainMenu();
                    canchor = false;
                }
            }

        }
         


        if(startmain){
            UpdatePlacementPose();
            UpdatePlacementIndicator();
            if (placementPoseIsValid && Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began){
                if(!oneobject){
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
            
      
    }

    private void setMainMenu(){
        GameObject mobile = GameObject.Find("MobileSingleStickControl");
        GameObject MainMenu = mobile.transform.Find("MainMenu").gameObject;
        MainMenu.SetActive(true);
        GameObject ReleaseBoat = MainMenu.transform.Find("ReleaseBoat").gameObject;
        ReleaseBoat.SetActive(false);
        GameObject DriveBoat = MainMenu.transform.Find("DriveBoat").gameObject;
        DriveBoat.SetActive(false);
        GameObject StartMenu = mobile.transform.Find("StartMenu").gameObject;
        StartMenu.SetActive(false);
    }

    private void CreateAnchor(){
        canchor = true;
    }

    private void CreateCha(){
        startmain = true;
        GameObject mobile = GameObject.Find("MobileSingleStickControl");
        GameObject MainMenu = mobile.transform.Find("MainMenu").gameObject;
        MainMenu.transform.Find("CreateCharacter").gameObject.SetActive(false);
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
        GameObject MainMenu = mobile.transform.Find("MainMenu").gameObject;
        MainMenu.transform.Find("ReleaseBoat").gameObject.SetActive(false);
        MainMenu.transform.Find("DriveBoat").gameObject.SetActive(false);

        //mobile.transform.GetChild(1).gameObject.SetActive(false);
        //mobile.transform.GetChild(2).gameObject.SetActive(false);
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
        Vector3 chaposition =new Vector3(PlacementPose.position.x,PlacementPose.position.y,anchorPosition.z);
        GameObject chaClone = (GameObject) Instantiate(character, chaposition, PlacementPose.rotation);
        chaClone.name = "character";
    }

    private void PlaceAnchor(){

        

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
        arRaycastManager.Raycast(screenCenter, hits, TrackableType.Planes);

        if((hits.Count > 0) && startmain){
            placementPoseIsValid = true;
        }
        //placementPoseIsValid = hits.Count > 0;
        if (placementPoseIsValid)
		{
            PlacementPose = hits[0].pose;
            var cameraForward = Camera.current.transform.forward;
            var cameraBearing = new Vector3(cameraForward.x, 0, cameraForward.z).normalized;
            PlacementPose.rotation = Quaternion.LookRotation(cameraBearing);
		}
	}
}