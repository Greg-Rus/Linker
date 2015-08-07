using UnityEngine;
using System.Collections;

public interface INode {

	TileType getTileType();
	int getNumberOfTilesInNode();
	void addTile(Tile tile);
	Tile removeTile();
	Vector2 getNodeCoordinates();
	INode getNeighbourNodeByLocation(Vector2 location);
	void addNeighbour(INode neighbour);
	void removeNiegbour(Vector2 neighbourLocation);

}
