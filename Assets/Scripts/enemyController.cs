using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyController : MonoBehaviour
{
    [SerializeField] private float minDistance = 2f;
    [SerializeField] private float maxDistance = 3f;
    [SerializeField] public float moveSpeed = 1f;
    [SerializeField] private float maxFollowFrom = 10f;
    [SerializeField] private float fireDelay = 50f;


    public AudioClip gunshot;

    public AudioClip deathSound;
    public AudioSource gunSource;

    public AudioSource deathSource;
    private bool canTime = false;
    private Rigidbody rb;

    Vector2 move;

    Vector3 playerDirection;

    Vector3 neighborDirection;

    private GameObject player;

    private Grid grid;
    private GameObject gridObject;

    private PathFinding pathfinder;

    private GameController gameController;

    private EnemyBulletController bulletController;



    private float _fireDelay = 0f;
    
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        player = GameObject.FindWithTag("Player");

        gridObject = GameObject.Find("PathingGrid");

        grid = gridObject.GetComponent<Grid>();

        pathfinder = GetComponent<PathFinding>();

        gameController = GameObject.FindWithTag("MainCamera").GetComponent<GameController>();

        bulletController = GameObject.FindWithTag("MainCamera").GetComponent<EnemyBulletController>();

        _fireDelay = 0;

        deathSource.clip = deathSound;
        gunSource.clip = gunshot;

    }

    
    void Update()
    {
        if(rb.angularVelocity != Vector3.zero){
            rb.angularVelocity = Vector3.zero;
        }
    }

    void FixedUpdate(){
        //Need to stop enemy when canTime is false
        canTime = gameController.GetCanTime();
        if(player != null){
            MoveEnemy();
        }
    }

    public void setCanTime(bool ct){
        canTime = ct;
    }

    private void MoveEnemy(){
        if(canTime){
            if(Vector3.Distance(transform.position, player.transform.position) > minDistance && Vector3.Distance(transform.position, player.transform.position) < maxFollowFrom){
                FollowPlayer();
            }

            if(Vector3.Distance(transform.position, player.transform.position) <= minDistance){
                //Don't move
                TryShooting();
            }
            else{
                FollowPath();
            }
        }

        else{
            move = new Vector2 (0,0);
            rb.velocity = move;
        }
        
    }

    private void FollowPlayer(){

        //Weird spinning happens on collision. RBs + 3D probably, or using transform here and velocity elsewhere
        playerDirection = player.transform.position - this.transform.position + Vector3.up;
        this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(playerDirection), 1);

        if(Vector3.Distance(transform.position, player.transform.position) > minDistance && Vector3.Distance(transform.position, player.transform.position) < maxFollowFrom){
            transform.position += transform.forward * moveSpeed * Time.fixedDeltaTime;
        }


        if(Vector3.Distance(transform.position, player.transform.position) <= maxDistance){
            
            TryShooting();
        }


    }

    List<Node> path;
    private void FollowPath(){
        //Look at the direction of the next node

        path = pathfinder.finalPath;

        if(path != null){
            neighborDirection = path[0].pos - this.transform.position + Vector3.forward;
            neighborDirection[2] = 0;
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(neighborDirection), 1);
            transform.position += transform.forward * moveSpeed * Time.fixedDeltaTime;
    }

    }

    void OrientEnemies(){
        print("LOOKING AT HIM");
        rb.angularVelocity = Vector3.zero;
        playerDirection = player.transform.position - this.transform.position + Vector3.forward;
        this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(playerDirection), 1);
    }

    void TryShooting(){
        
        if(_fireDelay == 0){
            gunSource.Play();
            OrientEnemies();
            bulletController.Shoot(this.transform, player.transform);
            _fireDelay = fireDelay;
            }
        _fireDelay--;
    }

    void OnCollisionEnter(Collision other){
        if(other.gameObject.tag == "Bullet"){
            deathSource.Play();
            gameObject.AddComponent<TriangleExplosion>();
            StartCoroutine(gameObject.GetComponent<TriangleExplosion>().SplitMesh(true));
            gameController.EnemyDeath();
        }
    }
}
