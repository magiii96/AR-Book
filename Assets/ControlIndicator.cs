using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;
using UnityEngine.XR.ARFoundation;


public class ControlIndicator : MonoBehaviour
{
    [SerializeField]
    private Toggle BridgeImg_Toggle;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if(BridgeImg_Toggle.isOn){
            gameObject.SetActive(false);
        }else{
            gameObject.SetActive(true);
        }
        
    }
}
