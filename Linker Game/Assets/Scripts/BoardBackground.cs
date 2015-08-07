using UnityEngine;
using System.Collections;

public class BoardBackground : MonoBehaviour {
	public GameObject backgroundTile;
	private int boardWidth;
	private int boardHeight;
	private GameObject newTile;
	private GameObject[] backgroudTiles;
	// Use this for initialization
	void Start () {
	
	}

	public void initialize( int boardWidth, int boardHeight)
	{
		this.boardWidth = boardWidth;
		this.boardHeight = boardHeight;
		setupBoardBackground ();
	}

	// Update is called once per frame
	void Update () {
	
	}

	private void setupBoardBackground()
	{
		cleanUpOldTiles();
		backgroudTiles = new GameObject[boardWidth * boardHeight];
		for (int i =0 ; i< boardWidth; i++){
			for ( int j=0; j< boardHeight; j++)
			{
				newTile = Instantiate(backgroundTile, new Vector3(i,j,0f), Quaternion.identity) as GameObject;
				newTile.transform.parent = transform;
				backgroudTiles[i * boardHeight + j] = newTile;
			}
			
		}
		Vector3 newPosition = new Vector3 (boardWidth * -0.5f + backgroundTile.transform.localScale.x *0.5f, 
		                                   boardHeight * -0.5f + backgroundTile.transform.localScale.y *0.5f,
		                                   0f);
		transform.position = newPosition;
	}
	public void cleanUpOldTiles()
	{
		if(backgroudTiles != null)
		{
			
			foreach (GameObject tile in backgroudTiles)
			{
				Destroy(tile);
			}
			backgroudTiles = null;
		}
	}
}
