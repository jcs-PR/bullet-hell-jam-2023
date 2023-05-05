using UnityEngine;
using System.Collections.Generic;

public class TwoD_Pathfinding : MonoBehaviour
{
	[HideInInspector]
	public List<TwoD_Node> path;
	public TwoD_Node startNode, targetNode;
	public TwoD_Grid grid;

	void Awake()
	{
		grid = GetComponent<TwoD_Grid>();
	}

    public List<TwoD_Node> GetPath(Vector3 start, Vector3 end)
    {
		FindPath(start, end);
		return path;
    }

	private void FindPath(Vector3 startPos, Vector3 targetPos)
	{
		startNode = grid.NodeFromWorldPosition(startPos);
		targetNode = grid.NodeFromWorldPosition(targetPos);
		if (startNode == targetNode)
			return;

		List<TwoD_Node> openSet = new List<TwoD_Node>();
		HashSet<TwoD_Node> closedSet = new HashSet<TwoD_Node>();
		openSet.Add(startNode);

		while (openSet.Count > 0)
		{
			TwoD_Node node = openSet[0];
			for (int i = 1; i < openSet.Count; i++)
			{
				if (openSet[i].fCost < node.fCost || openSet[i].fCost == node.fCost)
				{
					if (openSet[i].hCost < node.hCost)
						node = openSet[i];
				}
			}

			openSet.Remove(node);
			closedSet.Add(node);

			if (node == targetNode)
			{
				RetracePath(startNode, targetNode);
				return;
			}

			foreach (TwoD_Node neighbour in grid.GetNeighbours(node))
			{
				if (!neighbour.walkable || closedSet.Contains(neighbour))
				{
					continue;
				}
				int newCostToNeighbour = node.gCost + GetDistance(node, neighbour);
				if (newCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
				{
					neighbour.gCost = newCostToNeighbour;
					neighbour.hCost = GetDistance(neighbour, targetNode);
					neighbour.parent = node;

					if (!openSet.Contains(neighbour))
						openSet.Add(neighbour);
				}
			}
		}
	}

	private void RetracePath(TwoD_Node startNode, TwoD_Node endNode)
	{
		path = new List<TwoD_Node>();
		TwoD_Node currentNode = endNode;

		while (currentNode != startNode)
		{
			path.Add(currentNode);
			currentNode = currentNode.parent;
		}
		path.Reverse();
		grid.path = path;
	}
	private int GetDistance(TwoD_Node nodeA, TwoD_Node nodeB)
	{
		int dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
		int dstY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

		if (dstX > dstY)
			return 14 * dstY + 10 * (dstX - dstY);
		return 14 * dstX + 10 * (dstY - dstX);
	}
}