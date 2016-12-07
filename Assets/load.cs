using UnityEngine;
using System.Collections;

public class load : MonoBehaviour {

	// Use this for initialization
	void Start () {
		StartCoroutine ("logan");
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public IEnumerator logan() {
		yield return new WaitForSeconds (3);
		Application.LoadLevel ("Selecting");

	}


}
