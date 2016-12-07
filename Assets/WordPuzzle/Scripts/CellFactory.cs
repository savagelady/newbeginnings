/// <summary>
/// Cell factory.Makes the cells and bricks for the puzzle
/// </summary>
using UnityEngine;
using System.Collections;

public class CellFactory : MonoBehaviour {

	public GameObject emptyCellSprite;
	public GameObject brickSprite;
	public Transform cellParent;
	public static Transform[] cellPositions;

	public float cellSpacing = 10.0f;

	// Use this for initialization
	void Start () {
		MakeEmptyCells (PuzzleConfig.cellSize);
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	//Make empty Cells for addign our bricks
	public void MakeEmptyCells(int cellDim)
	{
		cellPositions = new Transform[cellDim * cellDim];
		int cellCount = 0;

		float cellSizeX = emptyCellSprite.GetComponent<SpriteRenderer> ().bounds.size.x;
		float cellSizeY = emptyCellSprite.GetComponent<SpriteRenderer> ().bounds.size.y;


		//For rows
		for (int i = 0; i < cellDim; i++) {
			//For columns
			for (int j = 0; j < cellDim; j++) {
				Vector3 cellPosition = new Vector3 ((cellSizeX  * j)+(cellSpacing*j) , (cellSizeY  * i)+(cellSpacing*i) , 0f);
				GameObject cellInstance =  Instantiate (emptyCellSprite, cellPosition, Quaternion.identity)as GameObject;
				cellInstance.transform.parent = cellParent;
				cellPositions [cellCount] = cellInstance.transform;
				cellCount++;
			}
			
		}

		GeneratePuzzleBricks ();
		//Debug.Log (emptyCellSprite.GetComponent<SpriteRenderer> ().bounds.size.x.ToString());
	}

	public void GeneratePuzzleBricks()
	{
		int strLen = PuzzleConfig.randString.Length;
		for (int i = 0; i < strLen; i++) {
			//bricks to be infront of the slots
			Vector3 cellpos = new Vector3(cellPositions[i].position.x,cellPositions[i].position.y,cellPositions[i].position.z-.5f);//.5 displacement in z axis to bring brick to front
			GameObject brickInstance =  Instantiate (brickSprite, cellpos, Quaternion.identity)as GameObject;
			brickInstance.name = "brick";//or else all the instances will have "(clone)" added to its name.
			brickInstance.transform.FindChild ("letter").GetComponent<TextMesh> ().text = PuzzleConfig.randString.Substring(i,1);
			//parent the brick to the slot, so that later we can read the words
			brickInstance.transform.parent = cellPositions[i];
		}

	}
}
