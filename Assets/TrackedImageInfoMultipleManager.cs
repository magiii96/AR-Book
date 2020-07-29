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

    [SerializeField]
    private Text imageTrackedText;

    [SerializeField]
    private GameObject[] arObjectsToPlace;

    //[SerializeField]
    private Vector3 taxiposition = new Vector3(0,0,0);

    private float minBridge = 100;
    private float maxBridge = 0;
    private float count = 1000;

    private ARTrackedImageManager m_TrackedImageManager;

    private Dictionary<string, GameObject> arObjects = new Dictionary<string, GameObject>();

    void Awake()
    {
        //dismissButton.onClick.AddListener(Dismiss);
        m_TrackedImageManager = GetComponent<ARTrackedImageManager>();
        
        // setup all game objects in dictionary
        foreach(GameObject arObject in arObjectsToPlace)
        {
            GameObject newARObject = Instantiate(arObject, Vector3.zero, Quaternion.identity);
            newARObject.name = arObject.name;
            switch (newARObject.name){
                case "GhostBook":
                newARObject.transform.Rotate (new Vector3(0,180,0));
                break;

                case "LondonOly":
                newARObject.transform.Rotate (new Vector3(90,180,0));
                break;

                case "side2":
                newARObject.transform.Rotate (new Vector3(0,90,0));
                break;

                default:
                break;
            }
      
            
            //GameObject newARObject = Instantiate(arObject, Vector3.zero, this.transform.rotation);
            
            //newARObject.transform.Rotate (new Vector3(90,180,0));
            
            arObjects.Add(arObject.name, newARObject);
            arObject.SetActive(false);
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
            arObjects[trackedImage.name].SetActive(false);
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

        count  = count - 1; 
        if(count == 0){
            maxBridge = 0;
            minBridge = 100;
            count = 1000;
        }

        if(name == "LondonOly"){
            position.z = position.z + 0.1f;
        }

        if(name == "side2"){
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
            prefab.SetActive(true);

            foreach(GameObject go in arObjects.Values)
            {
                if(go.name != name)
                {
                go.SetActive(false);
                }
            }

        }

 

    }

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