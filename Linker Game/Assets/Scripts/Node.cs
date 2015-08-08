using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Assertions;

//public enum EdgeDir {N, NE,E, SE, S, SW, W, NW};

public class Node : MonoBehaviour {

	//Used only to display the background tile. Can be used to display particle effects and other visuals relating to a node.

}

public class NodeController : INode
{
	private NodeModel nodeModel;
	private Node nodeView; //Not used yet. Exists to enable controll of the GameObject

	public NodeController(NodeModel model, Node view)
	{
		nodeModel = model;
		nodeView = view;
	}

	public TileType getTileType()
	{
		return nodeModel.type;
	}
	public int getNumberOfTilesInNode()
	{
		return nodeModel.bufferedTiles.Count;
	}
	public void addTile(Tile tile)
	{
		nodeModel.bufferedTiles.Add (tile);
	}
	public Tile removeTile()
	{
		Tile tileToReturn = nodeModel.bufferedTiles [nodeModel.bufferedTiles.Count - 1];
		nodeModel.bufferedTiles.Remove (tileToReturn);
		return tileToReturn;
	}
	public Vector2 getNodeCoordinates()
	{
		return nodeModel.nodeCoordinates;
	}
	public INode getNeighbourNodeByLocation(Vector2 location)
	{
		Assert.IsTrue (nodeModel.neighbours.ContainsKey (location));
		return nodeModel.neighbours[location];
	}
	public void addNeighbour(INode neighbour)
	{
		Vector2 neighbourLocalCoordinates = Utilities.globalToLocalBoardCoordinates(getNodeCoordinates(), neighbour.getNodeCoordinates());

		nodeModel.neighbours.Add (neighbourLocalCoordinates, neighbour);


	}
	public void removeNiegbour(Vector2 neighbourLocation)
	{
		nodeModel.neighbours.Remove (neighbourLocation);
	}
	
}
public class NodeModel
{
	public TileType type = TileType.None;
	public List<Tile> bufferedTiles;
	public Vector2 nodeCoordinates;
	public Dictionary<Vector2, INode> neighbours;

	public NodeModel(Vector2 nodeCoordinates)
	{
		bufferedTiles = new List<Tile>();
		neighbours = new Dictionary<Vector2, INode> ();
		this.nodeCoordinates = nodeCoordinates;
	}
}
