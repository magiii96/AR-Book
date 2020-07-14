using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class PlayerButton : MonoBehaviour
{
	public GameObject Interaction;


	void Start () {
		Button btn = this.GetComponent<Button> ();
		btn.onClick.AddListener (Click);
	}	

	public void Click(){
		TapToPlaceObject inter = Interaction.GetComponent<TapToPlaceObject>();
		//Interaction.SetActive(false);
		inter.reset = true;
	}
}
