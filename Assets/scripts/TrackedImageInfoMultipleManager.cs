using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;
using UnityEngine.XR.ARFoundation;

[RequireComponent(typeof(ARTrackedImageManager))]
public class TrackedImageInfoMultipleManager : MonoBehaviour
{
   
    private ARSessionOrigin arOrigin;
    

    [SerializeField]
    private Text imageTrackedText;

    [SerializeField]
    private Button finishButton;

    [SerializeField]
    private Toggle m_Toggle;

    [SerializeField]
    private GameObject[] arObjectsToPlace;

  

    //[SerializeField]
    private Vector3 taxiposition = new Vector3(0.0f,0.0f,0.0f);
    private Vector3 WorldPosition = new Vector3(1000.0f,0,0); 
    private Vector3 GhostPosition = new Vector3(1000.0f,0,0); 

    //occlusion part
    private Vector3 occlusionPosition = new Vector3(1000.0f,0,0);
    //
    private Vector3 cameraPosition;
    //adjust character 
    private Vector3 rotationEulerAngles;
    private Quaternion currentRotation;

    private GameObject mobile;
    private GameObject StartMenu;
    private GameObject MainMenu;
    private GameObject Joystick;
    private GameObject BoatControl;
    private GameObject runOcclusion;
    private GameObject ARcamera;
 

    private float minBridge = 100;
    private float maxBridge = 0;
    private float count = 1000;
    private bool show = true;
    private bool isCalibration = false; 
    private bool isOcclusion = true; 


    private ARTrackedImageManager m_TrackedImageManager;

    private Dictionary<string, GameObject> arObjects = new Dictionary<string, GameObject>();

    void Start(){
        arOrigin = FindObjectOfType<ARSessionOrigin>();
        mobile = GameObject.Find("MobileSingleStickControl");
        StartMenu = mobile.transform.Find("StartMenu").gameObject;
        MainMenu = mobile.transform.Find("MainMenu").gameObject;
        Joystick = mobile.transform.Find("MobileJoystick").gameObject;
        runOcclusion = mobile.transform.Find("runOcclusion").gameObject;
        BoatControl = MainMenu.transform.Find("BoatControl").gameObject;
        finishButton.onClick.AddListener(finishCali);
        ARcamera = GameObject.Find("AR Camera"); 
        //mobile.transform.Find("Toggle").gameObject.SetActive(false);
        //m_Toggle.gameObject.SetActive(false);
        
    }

    void OcclusionEffect(Vector3 occlusionPosition){
        Vector3 occluciontDir = occlusionPosition - cameraPosition;
        GameObject cha = GameObject.Find("character");
        GameObject man = cha.transform.Find("man").gameObject;
        Vector3 characterPosition = cha.transform.position; 
        Vector3 characterDir = characterPosition - cameraPosition;
        float angle = Vector3.Angle(characterDir, occluciontDir);
        //compute the distance 
        float distOcclu = Vector3.Distance(occlusionPosition, cameraPosition);
        float distCha= Vector3.Distance(characterPosition, cameraPosition);

        if((distCha > distOcclu)&&(angle < 20.0f)){
            man.GetComponent<Renderer>().enabled = false;
            imageTrackedText.text =  "occlued";
        }else{
            man.GetComponent<Renderer>().enabled = true;
            imageTrackedText.text =  "not occlued";
        }


    }

    void Update(){
        cameraPosition =  ARcamera.transform.position;
    }

    private void finishCali(){
        StartMenu.SetActive(false);
        MainMenu.SetActive(true);
        GameObject FootGuard = GameObject.Find("FootGuard");
        FootGuard.transform.Find("Plane").gameObject.SetActive(false);
        // Joystick.SetActive(true);
        BoatControl.SetActive(false);
        isCalibration = true; 
    }



    void Awake()
    {
        //arOrigin = FindObjectOfType<ARSessionOrigin>();
        m_TrackedImageManager = GetComponent<ARTrackedImageManager>();
        
        // setup all game objects in dictionary
        foreach(GameObject arObject in arObjectsToPlace)
        {
            GameObject newARObject = Instantiate(arObject, Vector3.zero, Quaternion.identity);
            newARObject.name = arObject.name;
            arObjects.Add(arObject.name, newARObject);
            newARObject.SetActive(false);
        }
    }

    void OnEnable()
    {
        m_TrackedImageManager.trackedImagesChanged += OnTrackedImagesChanged;
    }

    void OnDisable()
    {
        m_TrackedImageManager.trackedImagesChanged -= OnTrackedImagesChanged;
    }

   // private void Dismiss() => welcomePanel.SetActive(false);

    void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        foreach (ARTrackedImage trackedImage in eventArgs.added)
        {
            UpdateARImage(trackedImage);
        }

        foreach (ARTrackedImage trackedImage in eventArgs.updated)
        {
            UpdateARImage(trackedImage);
        }

        foreach (ARTrackedImage trackedImage in eventArgs.removed)
        {
            //arObjects[trackedImage.name].SetActive(false);
        }
    }

    private void UpdateARImage(ARTrackedImage trackedImage)
    {
        
        // Assign and Place Game Object
        //AssignGameObject(trackedImage.referenceImage.name, trackedImage.transform.position);
   
        string name = trackedImage.referenceImage.name;
        Vector3 position = trackedImage.transform.position;
        GameObject FootGuard = arObjects["FootGuard"];

        if (!isCalibration){
             //Calibration of the coordinates
            if(WorldPosition.x == 1000.0f || GhostPosition.x == 1000.0f){
                if(name == "GhostBook"){
                    GhostPosition = position;
                }

                if(name == "FootGuard"){
                    WorldPosition = position;
                }

            }else{
                Vector3 trueDir = new Vector3(0.06f, -0.02f, 0.2f);
                Vector3 estimateDir = GhostPosition - WorldPosition;
              
                if(name == "FootGuard" ){
                    //imageTrackedText.text =  WorldPosition.ToString();
                    FootGuard.transform.position = WorldPosition;
                    rotationEulerAngles = Quaternion.LookRotation(estimateDir).eulerAngles - Quaternion.LookRotation(trueDir).eulerAngles;
                    currentRotation = Quaternion.identity;
                    currentRotation.eulerAngles = rotationEulerAngles;
                    FootGuard.transform.rotation = currentRotation;
                    FootGuard.SetActive(true);
                    StartMenu.transform.Find("Info").gameObject.SetActive(false);
                    StartMenu.transform.Find("secondInfo").gameObject.SetActive(true);

                    //light rotation
                    // GameObject rotatelight = GameObject.Find("Directional Light");
                    // rotatelight.transform.rotation = currentRotation;
        
                }
            }

        }else{
            GameObject cha = GameObject.Find("character");
            // if(cha != null){
            //     cha.GetComponent<manController>().rotationEulerAngles = rotationEulerAngles;
            // }
            //imageTrackedText.text = "Right!";
            //adjust the position of each interaction based on the image tracking.
            // GameObject child = FootGuard.transform.Find("new"+name).gameObject;
            // GameObject virobject = child.transform.GetChild(1).gameObject;
            // virobject.transform.position = position;

            //handle the ship 
            if(name == "ship"){
                GameObject ship = arObjects["ship"];
                if(ship != null){
                    ship.SetActive(true);
                    ship.transform.position = position;
                    BoatControl.SetActive(true);
                    //ship.GetComponent<ShipController>().rotationEulerAngles = rotationEulerAngles;
                }
                
                // MainMenu.transform.Find("BoatControl").gameObject.SetActive(true)
            }

            //handle the swimming pool
            if(name == "Swimming"){
                GameObject swimming = arObjects["Swimming"];
                swimming.SetActive(true);
                swimming.transform.position = position;
                swimming.transform.rotation = currentRotation;
            }else{
                GameObject swimming = arObjects["Swimming"];
                swimming.SetActive(false);
            }

            //Occlusion
            if(name == "TheO"){
                //m_Toggle.gameObject.SetActive(true);
                if(m_Toggle.isOn){
                    occlusionPosition = position;
                    OcclusionEffect(occlusionPosition);
                    // GameObject ARcamera = arOrigin.transform.Find("AR Camera").gameObject;
                    // ARcamera.GetComponent<AROcclusionManager>().enabled = true;

                }else{
                     //GameObject cha = GameObject.Find("character");
                     if(cha != null){
                        GameObject man = cha.transform.Find("man").gameObject;
                        man.GetComponent<Renderer>().enabled = true;
                     }
                    // GameObject ARcamera = arOrigin.transform.Find("AR Camera").gameObject;
                    // ARcamera.GetComponent<AROcclusionManager>().enabled = false;
                     
                }

            }else{
                m_Toggle.isOn = false;
                //m_Toggle.gameObject.SetActive(false);
                occlusionPosition = new Vector3(1000.0f,0,0);
            }

            //Handle the interaction with bridge
            if(name == "side2"){
                GameObject sideBlock = arObjects["side2"];
                
                //float dist = Vector3.Distance(position, WorldPosition);
                float dist = Mathf.Abs(position.y - WorldPosition.y);
                // if(minBridge == null){
                //     minBridge = dist;
                // }

                // if(maxBridge == null){
                //     maxBridge = dist;
                // }

                if (dist < minBridge){
                minBridge = dist;
                }

                if (dist > maxBridge){
                    maxBridge = dist;
                }


                if(Mathf.Abs(dist - minBridge) > Mathf.Abs(dist - maxBridge)){
                    imageTrackedText.text = "open";
                    GameObject side = arObjects["side2"];
                    side.SetActive(false);
                }else{
                    imageTrackedText.text = "close";
                    GameObject side = arObjects["side2"];
                    side.transform.position = position;
                    //side.transform.rotation = currentRotation;
                    side.transform.Rotate(0.0f, 45.0f, 0.0f, Space.Self);
                    side.SetActive(true);
                }

            }


        }


        




   

    

        // if(WorldPosition.x == 1000.0f || GhostPosition.x == 1000.0f){
        //     imageTrackedText.text = "scan footguard and the red ghost book";
      
      

        // }else{
        //     Vector3 trueDir = new Vector3(0.1f, 0.0f, 0.2f);
        //     Vector3 estimateDir = GhostPosition - WorldPosition;
        //     //Vector3 newDirection = Vector3.RotateTowards(estimateDir, trueDir, 0.0f, 0.0f);
        //     GameObject FootGuard = arObjects["FootGuard"];
        //     if(name == "FootGuard" ){
        //         FootGuard.transform.position = WorldPosition;
        //         FootGuard.transform.rotation = Quaternion.LookRotation(estimateDir0ok)-Quaternion.LookRotation(trueDir);
        //         FootGuard.SetActive(true);
        //         //adjust each small element .
        //     }else{
        //         //GameObject child = FootGuard.transform.GetChild(4).gameObject;
        //         GameObject child = FootGuard.transform.Find("new"+name).gameObject;
        //         GameObject virobject = child.transform.GetChild(1).gameObject;
        //         virobject.transform.position = position;
        //         //GameObject collidertrigger = child.transform.Find("collidertrigger").gameObject;
        //         //GameObject objectTracking = arObjects[name];
        //        // objectTracking.SetActive(true);
        //         //objectTracking.transform.position = position;

        //     }
        
       // }



     
        

    

        // count  = count - 1; 
        // if(count == 0){
        //     maxBridge = 0;
        //     minBridge = 100;
        //     count = 1000;
        // }

        // if(name == "side"){
        //     float dist = Vector3.Distance(position, taxiposition);

        //     if(minBridge == null){
        //         minBridge = dist;
        //     }

        //     if(maxBridge == null){
        //         maxBridge = dist;
        //     }

        //     if (dist < minBridge){
        //         minBridge = dist;
        //     }

        //     if (dist > maxBridge){
        //         maxBridge = dist;
        //     }

        //     if(Mathf.Abs(dist - minBridge) > Mathf.Abs(dist - maxBridge)){
        //         imageTrackedText.text = "open";
        //         GameObject prefab = arObjects[name];
        //         prefab.transform.position = position;
        //         prefab.SetActive(false);
        //     }else{
        //         imageTrackedText.text = "close";
        //         GameObject prefab = arObjects[name];
        //         prefab.transform.position = position;
        //         prefab.SetActive(true);
        //     }
            
        // }else{
        //     GameObject prefab = arObjects[name];
        //     prefab.transform.position = position;
        //     //prefab.SetActive(false);// change true!!!

        //     // if we want to multiple mdoels show at the same time, commend out this.
        //     // foreach(GameObject go in arObjects.Values)
        //     // {
        //     //     if(go.name != name)
        //     //     {
        //     //     go.SetActive(false);
        //     //     }
        //     // }

        // }

 

    }

}