using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinding : MonoBehaviour
{
    public Grid grid;

    public List<Node> finalPath;


    //Testing
    private Transform seeker, target;

    void Update(){
        if(target != null){
            FindPath(seeker.position,target.position);
        }
    }

    void Awake(){
        //Added the GameObject.Find("PathingGrid") part
        grid = GameObject.Find("PathingGrid").GetComponent<Grid>();

        //Automatically find target so I don't have to keep doing it

        seeker = this.gameObject.transform;

        target =  GameObject.FindWithTag("Player").transform;

    }
    void FindPath(Vector3 startPos, Vector3 targetPos){
        Node startNode = grid.NodeFromWorldPoint(startPos);
        Node targetNode = grid.NodeFromWorldPoint(targetPos);

        List<Node> openSet = new List<Node>();
        HashSet<Node> closedSet = new HashSet<Node>();

        openSet.Add(startNode);
        while(openSet.Count > 0){
            Node currNode = openSet[0];
            for (int i = 1; i < openSet.Count; i++){
                if(openSet[i].fCost < currNode.fCost || openSet[i].fCost == currNode.fCost && openSet[i].hCost < currNode.hCost){
                    currNode = openSet[i];
                }
            }

            openSet.Remove(currNode);
            closedSet.Add(currNode);

            if(currNode == targetNode){
                RetracePath(startNode, targetNode);
                return;
            }

            foreach(Node neighbor in grid.GetNeighbors(currNode)){
                if(!neighbor.walkable || closedSet.Contains(neighbor)){
                    continue;
                }
                
                int costToNeighbor = currNode.gCost + GetDistance(currNode, neighbor);

                if(costToNeighbor < neighbor.gCost || !openSet.Contains(neighbor)){
                    neighbor.gCost = costToNeighbor;
                    neighbor.hCost = GetDistance(neighbor, targetNode);
                    neighbor.parent = currNode;

                    if(!openSet.Contains(neighbor)){
                        openSet.Add(neighbor);
                    }
                }
            }
        }
    }

    void RetracePath(Node startNode, Node endNode){
        List<Node> path = new List<Node>();
        Node currentNode = endNode;

        while(currentNode != startNode){
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }

        path.Reverse();

        //Test
        grid.path = path;

        finalPath = path;
    }

    int GetDistance(Node nodeA, Node nodeB){
        int distX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int distY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

        if(distX > distY){
            return 14*distY + 10*(distX - distY);
        }
        return 14*distX + 10*(distY - distX);
    }
}
