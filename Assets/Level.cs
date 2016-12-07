using UnityEngine;
using System.Collections;

public class Level : MonoBehaviour {

	public void Load(string name){
		Application.LoadLevel (name);
	}
}
