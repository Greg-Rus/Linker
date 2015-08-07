using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {
	public BoardBackground boardBackground;
	public Network network;
	public int boardWidth;
	public int boardHeight;
	// Use this for initialization
	void Start () {
		boardBackground.initialize (boardWidth, boardHeight);
		network.initialize (boardWidth, boardHeight);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
