using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TwoD_Grid : MonoBehaviour
{
	public List<TwoD_Node> path;
	[SerializeField] private LayerMask unwalkableMask;
	[SerializeField] public Vector2 gridWorldSize;
	[SerializeField] public  float nodeRadius;
	[SerializeField] private bool showGizmos;
	[SerializeField] private bool showGrid;
	[SerializeField] public bool showStartToTarget;
	[SerializeField] public bool showPath;

	private TwoD_Pathfinding pathfinding;
	private TwoD_Node[,] grid;
	public float nodeDiameter;
	private int gridSizeX, gridSizeY;

	void Awake()
	{
		pathfinding = GetComponent<TwoD_Pathfinding>();
		nodeDiameter = nodeRadius * 2;
		gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
		gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
		CreateGrid();
	}
    
	private void CreateGrid()
	{
		grid = new TwoD_Node[gridSizeX, gridSizeY];
		Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.up * gridWorldSize.y / 2;
		//Loop through all nodes and check if is walkable
		for (int x = 0; x < gridSizeX; x++)
		{
			for (int y = 0; y < gridSizeY; y++)
			{
				Vector3 nodePoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) +
					Vector3.up * (y * nodeDiameter + nodeRadius);
				bool walkable = !(Physics2D.OverlapCircle(nodePoint, nodeRadius, unwalkableMask));
				grid[x, y] = new TwoD_Node(walkable, nodePoint, x, y);
			}
		}
	}

	public List<TwoD_Node> GetNeighbours(TwoD_Node node)
	{
		List<TwoD_Node> neighbours = new List<TwoD_Node>();

		for (int x = -1; x <= 1; x++)
		{
			for (int y = -1; y <= 1; y++)
			{
				if (x == 0 && y == 0)
					continue;

				//Do diagonal movement
				int checkX = node.gridX + x;
				int checkY = node.gridY + y;
				if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
					neighbours.Add(grid[checkX, checkY]);
			}
		}
		return neighbours;
	}

	public TwoD_Node NodeFromWorldPosition(Vector3 worldPosition)
	{
		float percentX = (worldPosition.x - transform.position.x + gridWorldSize.x / 2) / gridWorldSize.x;
		float percentY = (worldPosition.y - transform.position.y + gridWorldSize.y / 2) / gridWorldSize.y;

		percentX = Mathf.Clamp01(percentX);
		percentY = Mathf.Clamp01(percentY);

		int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
		int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);
		//Debug.Log(grid[x, y, z].nodePosition);
		return grid[x, y];
	}

	void OnDrawGizmos()
	{
		if (showGizmos)
		{
            if (showGrid)
            {
				Gizmos.DrawWireCube(transform.position, new Vector2(gridWorldSize.x, gridWorldSize.y));
			}
		}
	}
}