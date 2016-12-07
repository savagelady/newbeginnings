using UnityEngine;
using System.Collections;

public class ress : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	public void Randomize(){
		GameObject.Find ("Main Camera").GetComponent<Times> ().num -= 5f;
		GameObject.Find ("PuzzleConfig").GetComponent<PuzzleConfig> ().Rands ();
	}
}
