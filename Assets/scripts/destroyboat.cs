using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class destroyboat : MonoBehaviour
{
    // Start is called before the first frame update
	public GameObject Interaction;

	void Start () {
		Button btn = this.GetComponent<Button> ();
		btn.onClick.AddListener (Click);
	}	

	public void Click(){
		// TapToPlaceObject inter = Interaction.GetComponent<TapToPlaceObject>();
		// inter.characterOn = true;
		// inter.release = true;

        GameObject shipObject = GameObject.Find("ship");
        shipObject.SetActive(false);
		GameObject mobile = GameObject.Find("MobileSingleStickControl");
        GameObject MainMenu = mobile.transform.Find("MainMenu").gameObject;
        MainMenu.transform.Find("BoatControl").gameObject.SetActive(false);
	}

}
