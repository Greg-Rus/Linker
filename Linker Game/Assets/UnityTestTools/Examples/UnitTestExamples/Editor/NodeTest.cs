using System;
using System.Collections.Generic;
using System.Threading;
using NUnit.Framework;
using UnityEngine;
using System.Reflection;
using UnityEditor;

namespace UnityTest
{
    [TestFixture]

    [Category("Network Tests")]
    internal class NetworkTest
    {
		GameObject testNetworkObject;
		Network testNetwork;
		Probe<Network> testNetworkProbe;
		NodeController[,] map;

		[SetUp] public void Init()
		{
			testNetworkObject = GameObject.Instantiate(AssetDatabase.LoadAssetAtPath ("Assets/Prefabs/Network.prefab", typeof(GameObject)), Vector3.zero, Quaternion.identity) as GameObject;
			testNetwork = testNetworkObject.GetComponent<Network> ();
			testNetworkProbe = new Probe<Network> (testNetwork);
		}
		[TearDown] public void Cleanup()
		{
			GameObject.DestroyImmediate(testNetworkObject);
		}

		[Test]
		public void networkInitalizationTest()
		{
			testNetwork.initialize (6, 6);
			Assert.IsNotNull (testNetwork);
		}

        [Test]
        //[Category("Failing Tests")]
        public void networkCreation()
        {

			testNetwork.initialize (6, 6);
			map = testNetworkProbe.getField ("networkMap") as NodeController[,];
			Assert.IsNotNull (map);
			Assert.AreEqual (6 * 6, map.Length);
        }
		[Test]
		public void networkBuildMEthodTest()
		{
			MethodInfo buildMethod = testNetworkProbe.getMethod ("buildNetwork");
			NodeController[,] newMap = new NodeController[4, 4];
			newMap = buildMethod.Invoke(testNetwork, new object[]{newMap}) as NodeController[,];
			Assert.AreEqual (4 * 4, newMap.Length);
		}
		[Test]
		public void networkconnectNodesTest()
		{
			testNetwork.initialize (3, 3);
			MethodInfo connectNodes = testNetworkProbe.getMethod ("connectNodes");
			Assert.IsNotNull (connectNodes);
			
			map = testNetworkProbe.getField ("networkMap") as NodeController[,];
			Assert.IsNotNull (connectNodes);
			
			connectNodes.Invoke (testNetwork, new object[]{map});
			INode testNode = map [1, 1];
			INode expectedLeftNode = map [0, 1];
			INode returnedLeftNode = testNode.getNeighbourNodeByLocation (new Vector2 (0f,1f));
			Assert.AreSame(expectedLeftNode, returnedLeftNode);
		}
		[Test]
		public void findNeigboursForNodeTest()
		{
			testNetwork.initialize (3, 3);
			List<Vector2> neighbourList = new List<Vector2> ();
			map = testNetworkProbe.getField ("networkMap") as NodeController[,];
			INode testNode = map [1, 1];
			MethodInfo findNeigboursForNode = testNetworkProbe.getMethod ("findNeigboursForNode");
			findNeigboursForNode.Invoke(testNetwork, new object[]{testNode,neighbourList});
			Assert.AreNotEqual(0,neighbourList.Count);
			Assert.AreEqual (8, neighbourList.Count);
			neighbourList.Clear ();
			testNode = map [0, 0];
			findNeigboursForNode.Invoke(testNetwork, new object[]{testNode,neighbourList});
			Assert.AreNotEqual(0,neighbourList.Count);
			Assert.AreEqual (3, neighbourList.Count);

		}
		[Test]
		public void findNeigboursForNodeBorderCasesTest()
		{
			testNetwork.initialize (3, 3);
			map = testNetworkProbe.getField ("networkMap") as NodeController[,];
			List<Vector2> neighbourList = new List<Vector2> ();
			INode testNode = map [0, 0];
			MethodInfo findNeigboursForNode = testNetworkProbe.getMethod ("findNeigboursForNode"); 
			findNeigboursForNode.Invoke(testNetwork, new object[]{testNode,neighbourList});
			Assert.AreNotEqual(0,neighbourList.Count);
			Assert.AreEqual (3, neighbourList.Count);
			//Mid left
			neighbourList.Clear ();
			testNode = map [0, 1];
			findNeigboursForNode.Invoke(testNetwork, new object[]{testNode,neighbourList});
			Assert.AreEqual (5, neighbourList.Count);
			//Top Mid
			neighbourList.Clear ();
			testNode = map [1, 2];
			findNeigboursForNode.Invoke(testNetwork, new object[]{testNode,neighbourList});
			Assert.AreEqual (5, neighbourList.Count);
			//Top Right
			testNode = map [2, 2];
			findNeigboursForNode.Invoke(testNetwork, new object[]{testNode,neighbourList});
			Assert.AreEqual (3, neighbourList.Count);
		}


	}



	[Category("Node Tests")]
	internal class NodeTest
	{
		GameObject testNetworkObject;
		Network testNetwork;
		Probe<Network> testNetworkProbe;
		NodeController[,] map;
		
		[SetUp] public void Init()
		{
			testNetworkObject = GameObject.Instantiate(AssetDatabase.LoadAssetAtPath ("Assets/Prefabs/Network.prefab", typeof(GameObject)), Vector3.zero, Quaternion.identity) as GameObject;
			testNetwork = testNetworkObject.GetComponent<Network> ();
			testNetworkProbe = new Probe<Network> (testNetwork);
			testNetwork.initialize (3, 3);
			map = testNetworkProbe.getField ("networkMap") as NodeController[,];
		}
		[TearDown] public void Cleanup()
		{
			GameObject.DestroyImmediate(testNetworkObject);
		}
		[Test]
		public void getNodeCoordinatesTest()
		{
			INode testNode = map [0, 0];
			Vector2 returnedCoordinates = testNode.getNodeCoordinates ();
			Vector2 expectedCoordinates = new Vector2 (0f, 0f);
			Assert.IsTrue (returnedCoordinates.Equals (expectedCoordinates));

		}
		[Test]
		public void getNumberOfTilesInNodeBasicTest()
		{
			INode testNode = map [1, 1];
			int returnedTileCount = testNode.getNumberOfTilesInNode ();
			Assert.AreEqual (0, returnedTileCount);
		}

		[Test]
		public void addNeighbourTest()
		{
			INode testNode = map [1, 1];
			Vector2 testNeighbourLocation = new Vector2 (2, 1);
			INode testNeighbour = map [2, 1];
			testNode.addNeighbour (testNeighbour);
			Probe<NodeController> testNodeProbe = new Probe<NodeController> (map[1,1]);
			NodeModel testNodeModel = testNodeProbe.getField ("nodeModel") as NodeModel;
			Assert.NotNull (testNodeModel);
			Assert.IsTrue (testNodeModel.neighbours.ContainsKey (testNeighbourLocation));
			Assert.IsTrue (testNodeModel.neighbours.ContainsValue (testNeighbour));
			Vector2 incorrectNeighbourLocation = new Vector2 (0, 1);
			Assert.IsFalse (testNodeModel.neighbours.ContainsKey (incorrectNeighbourLocation));
		}

	}
}