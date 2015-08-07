using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Network : MonoBehaviour {

	public GameObject nodePrefab;
	private int boardWidth;
	private int boardHeight;
	private NodeController[,] networkMap;


	public void initialize( int boardWidth, int boardHeight)
	{
		this.boardWidth = boardWidth;
		this.boardHeight = boardHeight;
		networkMap = new NodeController[boardWidth,boardHeight];
		networkMap = buildNetwork (networkMap);
	}

	private NodeController[,] buildNetwork(NodeController[,] map)
	{

		for (int i = 0; i< boardWidth; i++)
		{
			for (int j = 0; j < boardHeight; j++)
			{
				GameObject newNode = Instantiate(nodePrefab, new Vector3(i,j,0f), Quaternion.identity) as GameObject;
				newNode.transform.parent = this.transform;
				Node newNodeView = newNode.GetComponent<Node>();
				NodeModel newNodeModel = new NodeModel(new Vector2(i,j));
				map[i,j] = new NodeController(newNodeModel, newNodeView);
			}
		}
		Utilities.alignGroupCenterToOrigin(transform,nodePrefab,boardWidth,boardHeight);
		return map;
	}

	private void connectNodes(NodeController[,] map)
	{
		List<Vector2> neighboursGlobalLocations = new List<Vector2> ();

		for (int i = 0; i< boardWidth; i++)
		{
			for (int j = 0; j < boardHeight; j++)
			{
				NodeController node = map[i,j];
				findNeigboursForNode(node, neighboursGlobalLocations);
				foreach( Vector2 location in neighboursGlobalLocations)
				{
					node.addNeighbour(map[(int)location.x, (int)location.y]);
				}
			}
		}
	}

	private List<Vector2> findNeigboursForNode(NodeController node, List<Vector2> neighbours)
	{
		neighbours.Clear();
		Vector2 nodeLocation = node.getNodeCoordinates();

		for (int xOffset = -1; xOffset<=1; xOffset++) 
		{
			int neighbourX = (int)nodeLocation.x + xOffset;

			if (neighbourX < 0 || 
			    neighbourX >= boardWidth) continue;

			for (int yOffset = -1; yOffset<= 1; yOffset++)
			{
				int neighbourY = (int)nodeLocation.y + yOffset;

				if(neighbourY < 0 || 
				   neighbourY >= boardHeight || 
				   (neighbourX == nodeLocation.x && neighbourY == nodeLocation.y)) continue;

				Vector2 neighbourGlobalLocation = new Vector2(neighbourX, neighbourY);
				neighbours.Add (neighbourGlobalLocation);
			}
		}
		return neighbours;
	}

}
