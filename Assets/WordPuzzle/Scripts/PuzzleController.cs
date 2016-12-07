/// <summary>
/// Puzzle controller.Controls the puzzle, Drag/drops, word matching etc
/// </summary>
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PuzzleController : MonoBehaviour {
	//public string find = "box";

	public static PuzzleController instance;
	public int counts;
	public string streng;
	//public list;
	public string[] strengz;
	public enum GameStates {
		IDLE,
		DRAGGING,
		DROPPED,
		FOUND_WORDS

	}
	struct BrickArray{
		public Transform[] brickWord;
	}
	public GameStates currentGameState;
	Transform currentDragginBrick;
	Vector3 brickInitPos;

	public Text scoreText,movesText,wordsGot;
	public int moves,score;


	// Use this for initialization
	void Start () {
		//string[] strengz = new string[5];
		currentGameState = GameStates.IDLE;
		score =0;
		moves=0;



	}

	void Awake()
	{
		if(!instance)
			instance = this;
	}



	// Update is called once per frame
	void Update () {
		//Debug.Log(strengz[0]);
		if (counts == PlayerPrefs.GetInt("count") && Application.loadedLevelName == "Stage1") {
			PlayerPrefs.SetInt ("LVL2", 1);
			StartCoroutine("hangTime1");
		}

		if (counts == PlayerPrefs.GetInt("count") && Application.loadedLevelName == "Stage2") {
			PlayerPrefs.SetInt ("LVL3", 1);
			StartCoroutine("hangTime2");
		}


		if (counts == PlayerPrefs.GetInt("count") && Application.loadedLevelName == "Stage3") {
			PlayerPrefs.SetInt ("LVL4", 1);
			StartCoroutine("hangTime3");
		}


		if (counts == PlayerPrefs.GetInt("count") && Application.loadedLevelName == "Stage4") {
			PlayerPrefs.SetInt ("LVL5", 1);
			StartCoroutine("hangTime4");
		}


		scoreText.text = "Score: " + score;
		if (currentGameState == GameStates.FOUND_WORDS)
			return;
		//For testing purpose only
		if (Input.GetKeyUp (KeyCode.A)) {
			GetPuzzleStrings ();
		}

		//Check for Mouse button down
		if(Input.GetMouseButton(0)){

			if (currentGameState == GameStates.DRAGGING) {

				Vector2 currentMousePos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
				currentDragginBrick.position = new Vector3(currentMousePos.x,currentMousePos.y,currentDragginBrick.position.z);
			}
		}
		//On mouse button up release the object
		if (Input.GetMouseButtonUp (0)&& currentGameState == GameStates.DRAGGING) {
			//Debug.Log ("Left mouse button up");
			//Raycast to find if we have a slot below the current position
			Ray ray  = Camera.main.ScreenPointToRay(Input.mousePosition);
			//Debug.Log ("Mouse pos : " + Input.mousePosition.ToString ());
			RaycastHit hitObj;

			if (Physics.Raycast(ray, out hitObj, 1000000f)) {
			//	Debug.Log ("Hit on:"+ hitObj.collider.gameObject.name);

				//If the hitted object is a cell position it over that
				if (hitObj.collider.gameObject.tag == "cell") {
				//	Debug.Log ("Hit on empty Slot");

					//If the cell is already having a brick reset to old position
					if (hitObj.collider.transform.FindChild ("brick")) {
						currentDragginBrick.position = brickInitPos;
					} else {
						currentDragginBrick.position = hitObj.collider.transform.position;
						currentDragginBrick.Translate (new Vector3 (0,0,-.5f));
						//parent to the new cell
						currentDragginBrick.parent = hitObj.collider.transform;
						//increase the move by one and update
						moves++;
						movesText.text = "Moves : " + moves.ToString ();
					}

				} 
			}else {
				//Debug.Log ("Not in any slots , returning to home");

				currentDragginBrick.position = brickInitPos;
			}

			currentGameState = GameStates.DROPPED;

			currentDragginBrick = null;

			//check for words formed after the play
			GetPuzzleStrings ();
		}


	}


	public IEnumerator hangTime1(){

		yield return new WaitForSeconds (3);
		Application.LoadLevel ("Stage2");
	}

	public IEnumerator hangTime2(){

		yield return new WaitForSeconds (3);
		Application.LoadLevel ("Stage3");
	}

	public IEnumerator hangTime3(){

		yield return new WaitForSeconds (3);
		Application.LoadLevel ("Stage4");
	}

	public IEnumerator hangTime4(){

		yield return new WaitForSeconds (3);
		Application.LoadLevel ("Stage5");
	}


	public void StartDrag(Transform brick)
	{
		if (currentGameState != GameStates.DRAGGING) {
			currentDragginBrick = brick;
			brickInitPos = brick.position;
			currentGameState = GameStates.DRAGGING;
		}

	}


	//After each move the puzzle strings should be checked for completed words
	void GetPuzzleStrings()
	{
		string letterAtSlot;
		int cs = PuzzleConfig.cellSize;
		//interate through the cellslots and see which letters are there. Make vertical and horizontal strings and check for words
		string [] horizontalWords = new string[cs];
		BrickArray[] horizontalBricks = new BrickArray[cs];
		for(int x=0;x<cs;x++) {
			horizontalBricks[x].brickWord = new Transform[cs];
		}
		string[] verticalWords = new string[cs];

		BrickArray[] verticalBricks = new BrickArray[cs];
		for(int y=0;y<cs;y++) {
			verticalBricks[y].brickWord = new Transform[cs];
		}
		//Analyze row
		for (int i = 0; i < cs; i++) {
			for (int k = 0; k < cs; k++) {
				//Get cell
				Transform brickAtSlot =  CellFactory.cellPositions[(cs*i)+k].FindChild("brick");


				//If there is a brick only look for a letter or else put an asterix instead
				if (brickAtSlot == null) {
					letterAtSlot = "*";                                                                                
				} else {
					horizontalBricks [i].brickWord [k] = brickAtSlot;
					letterAtSlot=brickAtSlot.FindChild("letter").GetComponent<TextMesh>().text;
				}
				//Debug.Log (letterAtSlot);
				horizontalWords [i] += letterAtSlot;

			}

			Debug.Log (horizontalWords[i]);

			string[] Arr = GameObject.Find ("PuzzleConfig").GetComponent<PuzzleConfig> ().puzzleWords;
			foreach (string ar in Arr) {
				if (horizontalWords [i].Contains (ar)) {
					if (Application.loadedLevelName == "Stage4" || Application.loadedLevelName == "Stage5" || Application.loadedLevelName == "Room for 2") {
						if (GameObject.Find ("w1").GetComponentInChildren<Text> ().text == ar) {
							GameObject.Find ("w1").GetComponent<Button> ().interactable = false;
						} 
						if (GameObject.Find ("w2").GetComponentInChildren<Text> ().text == ar) {
							GameObject.Find ("w2").GetComponent<Button> ().interactable = false;
						}
						if (GameObject.Find ("w3").GetComponentInChildren<Text> ().text == ar) {
							GameObject.Find ("w3").GetComponent<Button> ().interactable = false;
						}
						if (GameObject.Find ("w4").GetComponentInChildren<Text> ().text == ar) {
							GameObject.Find ("w4").GetComponent<Button> ().interactable = false;
						}
						if (GameObject.Find ("w5").GetComponentInChildren<Text> ().text == ar) {
							GameObject.Find ("w5").GetComponent<Button> ().interactable = false;
						}
						if (GameObject.Find ("w6").GetComponentInChildren<Text> ().text == ar) {
							GameObject.Find ("w6").GetComponent<Button> ().interactable = false;
						}
						if (GameObject.Find ("w7").GetComponentInChildren<Text> ().text == ar) {
							GameObject.Find ("w7").GetComponent<Button> ().interactable = false;
						}
					}

				}

				if (horizontalWords [i].Contains (ar)) {
					if (Application.loadedLevelName == "Stage1") {
						if (GameObject.Find ("w1").GetComponentInChildren<Text> ().text == ar) {
							GameObject.Find ("w1").GetComponent<Button> ().interactable = false;
						} 
						if (GameObject.Find ("w2").GetComponentInChildren<Text> ().text == ar) {
							GameObject.Find ("w2").GetComponent<Button> ().interactable = false;
						}
						if (GameObject.Find ("w3").GetComponentInChildren<Text> ().text == ar) {
							GameObject.Find ("w3").GetComponent<Button> ().interactable = false;
						}
						if (GameObject.Find ("w4").GetComponentInChildren<Text> ().text == ar) {
							GameObject.Find ("w4").GetComponent<Button> ().interactable = false;
						}
					}

				}

				if (horizontalWords [i].Contains (ar)) {
					if (Application.loadedLevelName == "Stage2") {
						if (GameObject.Find ("w1").GetComponentInChildren<Text> ().text == ar) {
							GameObject.Find ("w1").GetComponent<Button> ().interactable = false;
						} 
						if (GameObject.Find ("w2").GetComponentInChildren<Text> ().text == ar) {
							GameObject.Find ("w2").GetComponent<Button> ().interactable = false;
						}
						if (GameObject.Find ("w3").GetComponentInChildren<Text> ().text == ar) {
							GameObject.Find ("w3").GetComponent<Button> ().interactable = false;
						}
						if (GameObject.Find ("w4").GetComponentInChildren<Text> ().text == ar) {
							GameObject.Find ("w4").GetComponent<Button> ().interactable = false;
						}
						if (GameObject.Find ("w5").GetComponentInChildren<Text> ().text == ar) {
							GameObject.Find ("w5").GetComponent<Button> ().interactable = false;
						}
					}

				}

			

				if (horizontalWords [i].Contains (ar)) {
					if (Application.loadedLevelName == "Stage3") {


						if (GameObject.Find ("w1").GetComponentInChildren<Text> ().text == ar) {

						//	GameObject.Find ("w1").GetComponentInChildren<Text> ().text = strengz[1];
			

							GameObject.Find ("w1").GetComponent<Button> ().interactable = false;
						} 
						if (GameObject.Find ("w2").GetComponentInChildren<Text> ().text == ar) {
							//strengz[1] = ar;
							GameObject.Find ("w2").GetComponent<Button> ().interactable = false;
						}
						if (GameObject.Find ("w3").GetComponentInChildren<Text> ().text == ar) {
							//strengz[2] = ar;
							GameObject.Find ("w3").GetComponent<Button> ().interactable = false;
						}
						if (GameObject.Find ("w4").GetComponentInChildren<Text> ().text == ar) {
							//strengz[3] = ar;
							GameObject.Find ("w4").GetComponent<Button> ().interactable = false;
						}

						if (GameObject.Find ("w5").GetComponentInChildren<Text> ().text == ar) {
							//strengz[4] = ar;
							GameObject.Find ("w5").GetComponent<Button> ().interactable = false;
						}

						if (GameObject.Find ("w6").GetComponentInChildren<Text> ().text == ar) {
							//strengz[5] = ar;
							GameObject.Find ("w6").GetComponent<Button> ().interactable = false;
						}



		

					}
				}
			
			}


		}



		//Now we have the horizontal words, look for the actual words inside

		StringSearch (horizontalWords, PuzzleConfig.searchWords,horizontalBricks);

		//Analyze column
		for (int m = 0; m < cs; m++) {
			for (int n = cs-1; n >= 0; n--) {
				//Get cell
				Transform brickAtSlot =  CellFactory.cellPositions[m+(cs*n)].FindChild("brick");




				//If there is a brick only look for a letter or else put an asterix instead
				if (brickAtSlot == null) {
					letterAtSlot = "*";
				} else {
					//Debug.Log ("Loop : " + ((cs - 1)-n).ToString()+"--"+m.ToString()+"--"+n.ToString());
					letterAtSlot=brickAtSlot.FindChild("letter").GetComponent<TextMesh>().text;
					//Debug.Log ("Letter at :" + letterAtSlot);
					verticalBricks [m].brickWord [(cs - 1)-n] = brickAtSlot;


				}

				verticalWords [m] += letterAtSlot;

			}
			string[] Arr = GameObject.Find ("PuzzleConfig").GetComponent<PuzzleConfig> ().puzzleWords;
			foreach (string ar in Arr) {
				if (verticalWords [m].Contains (ar)) {
					if (Application.loadedLevelName == "Stage4" || Application.loadedLevelName == "Stage5" || Application.loadedLevelName == "Room for 2") {
						if (GameObject.Find ("w1").GetComponentInChildren<Text> ().text == ar) {
							GameObject.Find ("w1").GetComponent<Button> ().interactable = false;
						} 
						if (GameObject.Find ("w2").GetComponentInChildren<Text> ().text == ar) {
							GameObject.Find ("w2").GetComponent<Button> ().interactable = false;
						}
						if (GameObject.Find ("w3").GetComponentInChildren<Text> ().text == ar) {
							GameObject.Find ("w3").GetComponent<Button> ().interactable = false;
						}
						if (GameObject.Find ("w4").GetComponentInChildren<Text> ().text == ar) {
							GameObject.Find ("w4").GetComponent<Button> ().interactable = false;
						}
						if (GameObject.Find ("w5").GetComponentInChildren<Text> ().text == ar) {
							GameObject.Find ("w5").GetComponent<Button> ().interactable = false;
						}
						if (GameObject.Find ("w6").GetComponentInChildren<Text> ().text == ar) {
							GameObject.Find ("w6").GetComponent<Button> ().interactable = false;
						}
						if (GameObject.Find ("w7").GetComponentInChildren<Text> ().text == ar) {
							GameObject.Find ("w7").GetComponent<Button> ().interactable = false;
						}
					}
				}
				

					if (verticalWords [m].Contains (ar)) {
						if (Application.loadedLevelName == "Stage1") {
							if (GameObject.Find ("w1").GetComponentInChildren<Text> ().text == ar) {
								GameObject.Find ("w1").GetComponent<Button> ().interactable = false;
							} 
							if (GameObject.Find ("w2").GetComponentInChildren<Text> ().text == ar) {
								GameObject.Find ("w2").GetComponent<Button> ().interactable = false;
							}
							if (GameObject.Find ("w3").GetComponentInChildren<Text> ().text == ar) {
								GameObject.Find ("w3").GetComponent<Button> ().interactable = false;
							}
							if (GameObject.Find ("w4").GetComponentInChildren<Text> ().text == ar) {
								GameObject.Find ("w4").GetComponent<Button> ().interactable = false;
							}

						}
				}

					if (verticalWords [m].Contains (ar)) {
							if (Application.loadedLevelName == "Stage2") {
								if (GameObject.Find ("w1").GetComponentInChildren<Text> ().text == ar) {
									GameObject.Find ("w1").GetComponent<Button> ().interactable = false;
								} 
								if (GameObject.Find ("w2").GetComponentInChildren<Text> ().text == ar) {
									GameObject.Find ("w2").GetComponent<Button> ().interactable = false;
								}
								if (GameObject.Find ("w3").GetComponentInChildren<Text> ().text == ar) {
									GameObject.Find ("w3").GetComponent<Button> ().interactable = false;
								}
								if (GameObject.Find ("w4").GetComponentInChildren<Text> ().text == ar) {
									GameObject.Find ("w4").GetComponent<Button> ().interactable = false;
								}
								if (GameObject.Find ("w5").GetComponentInChildren<Text> ().text == ar) {
									GameObject.Find ("w5").GetComponent<Button> ().interactable = false;
								}

							}
				}

					if (verticalWords [m].Contains (ar)) {
							if(Application.loadedLevelName == "Stage3"  ){
					if (GameObject.Find ("w1").GetComponentInChildren<Text> ().text == ar) {
						GameObject.Find ("w1").GetComponent<Button> ().interactable = false;
					} 
					if (GameObject.Find ("w2").GetComponentInChildren<Text> ().text == ar) {
						GameObject.Find ("w2").GetComponent<Button> ().interactable = false;
					}
					if (GameObject.Find ("w3").GetComponentInChildren<Text> ().text == ar) {
						GameObject.Find ("w3").GetComponent<Button> ().interactable = false;
					}
					if (GameObject.Find ("w4").GetComponentInChildren<Text> ().text == ar) {
						GameObject.Find ("w4").GetComponent<Button> ().interactable = false;
					}
					if (GameObject.Find ("w5").GetComponentInChildren<Text> ().text == ar) {
						GameObject.Find ("w5").GetComponent<Button> ().interactable = false;
					}

					if (GameObject.Find ("w5").GetComponentInChildren<Text> ().text == ar) {
						GameObject.Find ("w6").GetComponent<Button> ().interactable = false;
					}


				}
				}
			}


		}
		for (int h = 0; h < PuzzleConfig.cellSize; h++) {
			//Debug.Log ("vertical word : " + verticalWords [h]);
		}

		StringSearch (verticalWords, PuzzleConfig.searchWords,verticalBricks);
	}

	//Look for strings matching
	void StringSearch(string [] puzzleWords,string [] searchWords)
	{

		for (int i = 0; i < puzzleWords.Length; i++) {
			for (int j = 0; j < searchWords.Length; j++) {
				int indexOfWord = puzzleWords [i].IndexOf (searchWords [j]);
				if (indexOfWord != -1) {

					//once the words are identified pass the index and length of the word 
					//get the words absolute startign position in the while letter string
					int wordAbsPos = (i*PuzzleConfig.cellSize)+indexOfWord; 
					//Debug.Log (searchWords[j]+" at :" + wordAbsPos.ToString());
					// Get the bricks of the word as array of spriterenderers for further processing
					SpriteRenderer[] brickWordArray = new SpriteRenderer[searchWords[j].Length];

					for (int m = wordAbsPos; m < searchWords[j].Length + wordAbsPos; m++) {
						Transform brick = CellFactory.cellPositions [m].FindChild ("brick");
						SpriteRenderer spr = brick.GetComponent<SpriteRenderer> ();
						brickWordArray [m - wordAbsPos] = spr;
					}

					HandleFoundWords (brickWordArray);

				}
			}
		}
	}


	void StringSearch(string [] puzzleWords,string [] searchWords,BrickArray[] brickArray)
	{

		for (int i = 0; i < puzzleWords.Length; i++) {

			for (int j = 0; j < searchWords.Length; j++) {
				int indexOfWord = puzzleWords [i].IndexOf (searchWords [j]);
				if (indexOfWord != -1) {

					//once the words are identified pass the index and length of the word 


					// Get the bricks of the word as array of spriterenderers for further processing
					SpriteRenderer[] brickWordArray = new SpriteRenderer[searchWords[j].Length];

					for (int m = indexOfWord; m < searchWords[j].Length + indexOfWord; m++) {
						Transform brick = brickArray [i].brickWord[m];
						if (brick != null) {
							SpriteRenderer spr = brick.GetComponent<SpriteRenderer> ();
							brickWordArray [m - indexOfWord] = spr;
						}

					}
					//wordsGot.text += " " + puzzleWords[i];
					HandleFoundWords (brickWordArray);


				}
			}
		}
	}


	//Hanndles the found words
	void HandleFoundWords(SpriteRenderer [] brickArray)
	{
		currentGameState =GameStates.FOUND_WORDS;
		foreach(SpriteRenderer spr in brickArray){
			if(spr != null)
				spr.material.color = Color.green;
			spr.gameObject.GetComponent<LetterBrick> ().enabled = false;

		}

		StartCoroutine (RemoveBricksWithDelay (brickArray));

	}

	IEnumerator RemoveBricksWithDelay(SpriteRenderer [] brickArray)
	{
		yield return new WaitForSeconds (1f);
		foreach(SpriteRenderer spr in brickArray){
			Destroy (spr.gameObject);
			score += 5;
			counts += 1;
		}

		currentGameState = GameStates.IDLE;
	}





}
