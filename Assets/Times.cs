using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class Times : MonoBehaviour {
	public Text times;
	public float num;
	// Use this for initialization
	void Start () {
		num = 60;
	}
	
	// Update is called once per frame
	void Update () {
		num -= Time.deltaTime;
		times.text = "Time: " + (int)num;
	}
}
