using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;
using UnityEngine.XR.ARFoundation;

public class occlusionControl : MonoBehaviour
{
    [SerializeField]
    private Toggle m_Toggle;


    // Update is called once per frame
    void Update()
    {
        if(m_Toggle.isOn){
            this.GetComponent<AROcclusionManager>().enabled = true;
        }else{
            this.GetComponent<AROcclusionManager>().enabled = false;
        }
        
    }
}
