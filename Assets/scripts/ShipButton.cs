using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class ShipButton : MonoBehaviour
{

	[SerializeField]
    private Button ReleaseButton;

	public GameObject Interaction;


	void Start () {
		Button btn = this.GetComponent<Button> ();
		btn.onClick.AddListener (Click);
	}	

	public void Click(){
		Button btn = this.GetComponent<Button> ();
		TapToPlaceObject inter = Interaction.GetComponent<TapToPlaceObject>();
		inter.characterOn = false;
		btn.gameObject.SetActive(false);
		// GameObject mobile = GameObject.Find("MobileSingleStickControl");
        // GameObject MainMenu = mobile.transform.Find("MainMenu").gameObject;
		// GameObject BoatControl = MainMenu.transform.Find("BoatControl").gameObject;
        // BoatControl.transform.Find("DriveBoat").gameObject.SetActive(false);
		// BoatControl.transform.Find("ReleaseBoat").gameObject.SetActive(true);
		ReleaseButton.gameObject.SetActive(true);

	}
}
