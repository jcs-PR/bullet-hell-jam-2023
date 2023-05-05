using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathManager : MonoBehaviour
{
    [SerializeField] private TwoD_Pathfinding pathfinder;
    [SerializeField] private Transform player;
    [SerializeField] private TwoD_Grid grid;

    int index = 0;

    void Awake()
    {
        pathfinder = this.GetComponent<TwoD_Pathfinding>();
        grid = this.GetComponent<TwoD_Grid>();
    }

    // Update is called once per frame
    void Update()
    {
        if (WantPath.AI.Count > 0)
        {
            ShooterAI user = WantPath.AI[index];
            List<TwoD_Node>_path = pathfinder.GetPath(user.transform.position,
                user.targetNode.nodePosition);

            if (pathfinder.path.Count > 1)
            {
                user.GetVariables(true, _path);
                WantPath.AI.Remove(user);
            }
            else if(WantPath.AI.Count > 1)
                index++;
        }
        else
            index = 0;
    }

    public static class WantPath
    {
        [SerializeField] static public List<ShooterAI>
            AI = new List<ShooterAI>();
    }
}
