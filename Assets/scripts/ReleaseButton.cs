using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class ReleaseButton : MonoBehaviour
{
	public GameObject Interaction;


	void Start () {
		Button btn = this.GetComponent<Button> ();
		btn.onClick.AddListener (Click);
	}	


	public void Click(){
		TapToPlaceObject inter = Interaction.GetComponent<TapToPlaceObject>();
		inter.characterOn = true;
		inter.release = true;
		GameObject mobile = GameObject.Find("MobileSingleStickControl");
		GameObject MainMenu = mobile.transform.Find("MainMenu").gameObject;
		MainMenu.transform.Find("ReleaseBoat").gameObject.SetActive(false);
        //mobile.transform.GetChild(2).gameObject.SetActive(false);
	}
}
