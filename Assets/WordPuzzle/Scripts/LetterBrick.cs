/// <summary>
/// Letter brick. Controls the drag and drop of the bricks
/// </summary>
using UnityEngine;
using System.Collections;

public class LetterBrick : MonoBehaviour {
	Renderer rend;
	Color initialColor;
	// Use this for initialization
	void Start () {
		rend = this.GetComponent<SpriteRenderer> ();
		initialColor = rend.material.color;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnMouseOver()
	{
		//rend.material.color = Color.blue;
	}

	void OnMouseEnter()
	{
		rend.material.color = Color.blue;
	}

	void OnMouseExit(){
		rend.material.color = initialColor;

	}

	void OnMouseDrag()
	{
		rend.material.color = Color.red;
		PuzzleController.instance.StartDrag (this.gameObject.transform);
		//GetComponent<BoxCollider2D> ().enabled = false;
	}

	void OnMouseDown()
	{
	}

	void OnMouseUp()
	{
		rend.material.color = initialColor;

	}
}
