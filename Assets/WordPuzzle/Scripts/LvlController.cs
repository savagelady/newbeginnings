using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class LvlController : MonoBehaviour {
	public Button button1;
	public Button button2;
	public Button button3;
	public Button button4;
	public Button button5;
	// Use this for initialization
	void Start () {
		button1.interactable = true;
		button2.interactable = false;
		button3.interactable = false;
		button4.interactable = false;
		button5.interactable = false;
	}
	
	// Update is called once per frame
	void Update () {
		if(PlayerPrefs.GetInt("LVL2") == 1){
			button2.interactable = true;
	}

		if(PlayerPrefs.GetInt("LVL3") == 1){
			button3.interactable = true;
		}

		if(PlayerPrefs.GetInt("LVL4") == 1){
			button4.interactable = true;
		}

		if(PlayerPrefs.GetInt("LVL5") == 1){
			button5.interactable = true;
		}
}
}