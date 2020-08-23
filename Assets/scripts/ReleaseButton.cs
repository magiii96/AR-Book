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
		GameObject BoatControl = MainMenu.transform.Find("BoatControl").gameObject;
		BoatControl.transform.Find("ReleaseBoat").gameObject.SetActive(false);
	}
}
