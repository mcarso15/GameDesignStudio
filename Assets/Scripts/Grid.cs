using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{

    Node[,] grid;
    public Vector2 gridWorldSize;

    public float nodeRadius;
    public LayerMask unwalkableMask;

    public float nodeDiameter;
    int gridX;
    int gridY;
    void Start(){
        nodeDiameter = nodeRadius * 2;
        gridX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
        CreateGrid();

    }

    //Gizmos are for visual testing, disable before finishing
    public List<Node> path;
    private void OnDrawGizmos(){
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x,gridWorldSize.y,1));

        if(grid != null){
            foreach (Node n in grid){
                Gizmos.color = (n.walkable)?Color.yellow:Color.red;
                if(path != null){
                    if(path.Contains(n)){
                        Gizmos.color = Color.green;
                    }
                }
                Gizmos.DrawCube(n.pos, Vector3.one * (nodeDiameter - .1f));
            }
        }
    }

    void CreateGrid(){
        grid = new Node[gridX,gridY];

        
        Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x/2 - Vector3.up * gridWorldSize.y/2;

        for(int x = 0; x < gridX; x++){
            for(int y = 0; y < gridY; y++){
                Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.up * (y * nodeDiameter + nodeRadius);
                bool walkable = !(Physics.CheckSphere(worldPoint,nodeRadius,unwalkableMask));
                grid[x,y] = new Node(walkable, worldPoint, x, y);
            }
        }
    }

    public Node NodeFromWorldPoint(Vector3 worldPos){
        float percX = (worldPos.x + gridWorldSize.x / 2) / gridWorldSize.x;
        float percY = (worldPos.y + gridWorldSize.y / 2) / gridWorldSize.y;
        percX = Mathf.Clamp01(percX);
        percY = Mathf.Clamp01(percY);

        int x = Mathf.RoundToInt((gridX - 1) * percX);
        int y = Mathf.RoundToInt((gridY - 1) * percY);

        return grid[x,y];
    }

    public List<Node> GetNeighbors(Node node){
        List<Node> neighbors = new List<Node>();

        for(int x = -1; x <= 1; x++){
            for(int y = -1; y <= 1; y++){
                if(x == 0 && y == 0){
                    continue;
                }

                //gridX and gridY are the X,Y 'coordinates' within the grid array
                int checkX = node.gridX + x;
                int checkY = node.gridY + y;

                //Here, gridX and gridY are the grid sizes for x and y.
                if(checkX >= 0 && checkX < gridX && checkY >= 0 && checkY < gridY){
                    neighbors.Add(grid[checkX,checkY]);
                }
            }
        }

        return neighbors;
    }
    
}
