using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;
using UnityEngine.XR.ARFoundation;

[RequireComponent(typeof(ARTrackedImageManager))]
public class TrackedImageInfoMultipleManager : MonoBehaviour
{
    //[SerializeField]
    //private GameObject welcomePanel;

    //[SerializeField]
    //private Button dismissButton;

    private ARSessionOrigin arOrigin;

    [SerializeField]
    private Text imageTrackedText;

    [SerializeField]
    private GameObject[] arObjectsToPlace;

    //[SerializeField]
    private Vector3 taxiposition = new Vector3(0.0f,0.0f,0.0f);
    private Vector3 WorldPosition = new Vector3(1000.0f,0,0); 
    private Vector3 GhostPosition = new Vector3(1000.0f,0,0); 
 

    private float minBridge = 100;
    private float maxBridge = 0;
    private float count = 1000;
    private bool show = true;

    private ARTrackedImageManager m_TrackedImageManager;

    private Dictionary<string, GameObject> arObjects = new Dictionary<string, GameObject>();

    void Awake()
    {
        arOrigin = FindObjectOfType<ARSessionOrigin>();
        //dismissButton.onClick.AddListener(Dismiss);

        m_TrackedImageManager = GetComponent<ARTrackedImageManager>();
        
        // setup all game objects in dictionary
        foreach(GameObject arObject in arObjectsToPlace)
        {
            GameObject newARObject = Instantiate(arObject, Vector3.zero, Quaternion.identity);
            newARObject.name = arObject.name;
            // switch (newARObject.name){
            //     case "GhostBook":
            //     newARObject.transform.Rotate (new Vector3(0,180,0));
            //     break;

            //     case "LondonOly":
            //     newARObject.transform.Rotate (new Vector3(90,180,0));
            //     break;

            //     case "side2":
            //     newARObject.transform.Rotate (new Vector3(0,90,0));
            //     break;

            //     default:
            //     break;
            // }
      
            
            //GameObject newARObject = Instantiate(arObject, Vector3.zero, this.transform.rotation);
            
            //newARObject.transform.Rotate (new Vector3(90,180,0));
            
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
        // Display the name of the tracked image in the canvas
        //imageTrackedText.text = trackedImage.transform.position.ToString();

        // Assign and Place Game Object
        //AssignGameObject(trackedImage.referenceImage.name, trackedImage.transform.position);

        //Debug.Log($"trackedImage.referenceImage.name: {trackedImage.referenceImage.name}");
        
        string name = trackedImage.referenceImage.name;
        Vector3 position = trackedImage.transform.position;
        
        imageTrackedText.text = position.ToString();

        // if(name == "side2"){
        //     if(lastPosition ==  Vector3.zero){
        //         lastPosition = position;
        //     }else{
        //         if(lastPosition.y > (position.y +0.05)){
        //             imageTrackedText.text = lastPosition.ToString() + "close" + position.ToString();
        //         }else{
        //             imageTrackedText.text = lastPosition.ToString() + "open" + position.ToString();
        //         }
        //     }
        //     lastPosition = position;
        // }

        //set the world coordinate as the footguard
        if(name == "ship"){
            GameObject ship = arObjects[name];
            ship.SetActive(true);
        }

        if(WorldPosition.x == 1000.0f || GhostPosition.x == 1000.0f){
            //imageTrackedText.text = "scan footguard and the red ghost book";
            if(name == "FootGuard"){
                WorldPosition = position;
            //     GameObject prefab = arObjects[name];
            //     prefab.transform.position = WorldPosition;
            //     prefab.SetActive(true);
            }
            if(name == "GhostBook"){
                GhostPosition = position;
            }

        }else{
            Vector3 trueDir = new Vector3(0.2f, -0.1f, 0.0f);
            Vector3 estimateDir = GhostPosition - WorldPosition;
            Vector3 newDirection = Vector3.RotateTowards(estimateDir, trueDir, 0.0f, 0.0f);
            GameObject FootGuard = arObjects["FootGuard"];
            if(name == "FootGuard" ){
                FootGuard.transform.position = WorldPosition;
                FootGuard.transform.rotation = Quaternion.LookRotation(newDirection);
                FootGuard.SetActive(true);
                //adjust each small element .
            }else{
                //GameObject child = FootGuard.transform.GetChild(4).gameObject;
                GameObject child = FootGuard.transform.Find("new"+name).gameObject;
                GameObject virobject = child.transform.GetChild(1).gameObject;
                virobject.transform.position = position;
                //GameObject collidertrigger = child.transform.Find("collidertrigger").gameObject;
                //GameObject objectTracking = arObjects[name];
               // objectTracking.SetActive(true);
                //objectTracking.transform.position = position;

            }
        
        }



     
        
        // if(name == "GhostBook"){
        //     GameObject prefab = arObjects[name];
        //     position.x = WorldPostion.x + 0.1f;
        //     position.y = WorldPostion.y;
        //     position.z = WorldPostion.z + 0.1f;
        //     //arOrigin.MakeContentAppearAt(prefab.transform,position);
        //     //prefab.transform.position = position;
        //     //prefab.SetActive(false);
        // }

    

        count  = count - 1; 
        if(count == 0){
            maxBridge = 0;
            minBridge = 100;
            count = 1000;
        }

        if(name == "side"){
            float dist = Vector3.Distance(position, taxiposition);

            if(minBridge == null){
                minBridge = dist;
            }

            if(maxBridge == null){
                maxBridge = dist;
            }

            if (dist < minBridge){
                minBridge = dist;
            }

            if (dist > maxBridge){
                maxBridge = dist;
            }

            if(Mathf.Abs(dist - minBridge) > Mathf.Abs(dist - maxBridge)){
                imageTrackedText.text = "open";
                GameObject prefab = arObjects[name];
                prefab.transform.position = position;
                prefab.SetActive(false);
            }else{
                imageTrackedText.text = "close";
                GameObject prefab = arObjects[name];
                prefab.transform.position = position;
                prefab.SetActive(true);
            }
            
        }else{
            GameObject prefab = arObjects[name];
            prefab.transform.position = position;
            //prefab.SetActive(false);// change true!!!

            // if we want to multiple mdoels show at the same time, commend out this.
            // foreach(GameObject go in arObjects.Values)
            // {
            //     if(go.name != name)
            //     {
            //     go.SetActive(false);
            //     }
            // }

        }

 

    }



    // void OnTriggerEnter(Collider objectTri)
    // {
    //   Debug.Log($"ok");
    //   objectTri.isTrigger = true;
    // } 

    // void OnTriggerExit(Collider objectTri)
    // {
    //     // Destroy everything that leaves the trigger
    //     objectTri.isTrigger = false;
    // }

//     void AssignGameObject(string name, Vector3 newPosition)
//     {
//         if(arObjectsToPlace != null)
//         {
//             GameObject goARObject = arObjects[name];
//             goARObject.SetActive(true);
//             goARObject.transform.position = newPosition;
//             goARObject.transform.localScale = scaleFactor;
//             foreach(GameObject go in arObjects.Values)
//             {
//                 Debug.Log($"Go in arObjects.Values: {go.name}");
//                 if(go.name != name)
//                 {
//                     go.SetActive(false);
//                 }
//             } 
//         }
//     }
}