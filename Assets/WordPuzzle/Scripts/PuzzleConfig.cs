/// <summary>
/// Puzzle config. Take a array of string as input, which will be used to construct the puzzle
/// </summary>
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.IO;

public class PuzzleConfig : MonoBehaviour {
	public string[] puzzleWords;
	public static int cellSize;
	public string[] arrayOfWords ;
	public static string randString;
	public string[] lines;
	public string stringify;
	public static string[] searchWords;//for accessing in searcher

	// Use this for initialization
	void Start () {
		searchWords = puzzleWords;
		GameObject.Find("w1").GetComponentInChildren<Text>().text = puzzleWords[0];
		GameObject.Find("w2").GetComponentInChildren<Text>().text = puzzleWords[1];
		GameObject.Find("w3").GetComponentInChildren<Text>().text = puzzleWords[2];
		GameObject.Find("w4").GetComponentInChildren<Text>().text = puzzleWords[3];
		GameObject.Find("w5").GetComponentInChildren<Text>().text = puzzleWords[4];
		GameObject.Find("w6").GetComponentInChildren<Text>().text = puzzleWords[5];


	}

	void Awake(){

		arrayOfWords[5] = GameObject.Find("Diction").GetComponent<DictionaryMobi>().Dict;

		lines = new string[] {"love","hate"};
	

		puzzleWords =  arrayOfWords ;

		GetPuzzleSize ();
		RandomizeString (puzzleWords);
	}

	public void Rands(){
		GameObject[] others = GameObject.FindGameObjectsWithTag("brick");

		foreach (GameObject game in others) {

			if(game != gameObject)
			{
				Destroy(game);
			}
		}
		//searchWords = puzzleWords;
		//GetPuzzleSize ();

		GetPuzzleSize ();
		RandomizeString (lines);
		GameObject.Find ("CellFactory").GetComponent<CellFactory> ().GeneratePuzzleBricks ();
	}

	// Update is called once per frame
	void Update () {




	
		//w6.text = puzzleWords [5];

	}

	//Sums up the letters of the puzzle words to calculate the size of the cells needed to
	//hold the puzzle
	public void GetPuzzleSize()
	{
		//First calculate the total number of letters
		int letterCount = 0;
		foreach (string str in puzzleWords) {
			letterCount += str.Length;
		}
		PlayerPrefs.SetInt ("count", letterCount);
		//Debug.Log ("Total letters : " + letterCount.ToString ());

		//For doing a square cell matrix we need to get the square that can hold these cells
		float sqRoot = Mathf.Sqrt((float)letterCount);
		//Extra one cell is added so we have enough room to move blocks around
		cellSize = Mathf.RoundToInt (sqRoot) + 1;

		//Debug.Log ("Matrix size : " + cellSize.ToString ());


	}

	//This will shuffle the letters for the puzzle

	 void RandomizeString(string[] puzzleStringArray)
	{
		//concatenate the string to make a big string of letters
		string bigstring = "";
		foreach (string str in puzzleStringArray) {
			bigstring +=  str;
		}

		//Debug.Log ("Big string is : " + bigstring);
		stringify = bigstring;

	
		//Randomize the string
		//make character array
		System.Random rnd = new System.Random();
		char [] strChars = bigstring.ToCharArray();
		//Debug.Log (strChars.Length);
		int i = strChars.Length;
		while (i>1) {
			i--;
			int j = rnd.Next (i+1);
			char val = strChars [j];
			strChars [j] = strChars [i];
			strChars [i] = val;
		}

		randString = new string (strChars);

		//Debug.Log ("Randomized String : " + randString);

	}


}
