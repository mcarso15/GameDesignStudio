using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour{


    public AudioClip gunshot;

    public AudioClip deathSound;
    public AudioSource gunSource;

    public AudioSource deathSource;

    private Rigidbody rb;

    private bool canTime = false;

    public float speed = 3;

    GameObject bullet;

    GameController gameController;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        
        bullet = GameObject.Find("Bullet");

        gunSource.clip = gunshot;

        gameController = GameObject.FindWithTag("MainCamera").GetComponent<GameController>();

        deathSource.clip = deathSound;

    }

    // Update is called once per frame
    void Update()
    {
        if(!gameController.isPaused){
            if(Input.GetButtonDown("Fire1")){
                Shoot();
            }
        }
    }

    void FixedUpdate(){
        //Player Movement
        if(!gameController.isPaused){
        MovePlayer();
        }

    }

    private void MovePlayer(){
        /*
        GetAxisRaw disables "smoothing" when releasing the movement keys, giving the player tighter movement control.
        For now I prefer this as opposed to sliding when I release, but with time dilation and slower movement may
        prefer to return to GetAxis.
         */
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        if(x != 0 || y != 0){
            canTime = true;
        }
        
        if(x == 0 && y == 0){
            canTime = false;
        }

        Vector2 move = new Vector2 (x, y);

        rb.velocity = move * speed;

        //Player rotates to face mouse
        Vector3 mouseScreen = Input.mousePosition;
        Vector3 mouse = Camera.main.ScreenToWorldPoint(mouseScreen);
        transform.rotation = Quaternion.Euler(0,0,Mathf.Atan2(mouse.y - transform.position.y, mouse.x - transform.position.x) * Mathf.Rad2Deg - 90);  
    }

    private void ResetCanTime(){
        canTime = false;
    }

    public bool GetCanTime(){
        if (bullet.transform.position != bullet.GetComponent<BulletController>().home){
            canTime = true;
        }
        return canTime;
    }

    private void Shoot(){

        if(bullet.transform.position == bullet.GetComponent<BulletController>().home){
            //0.15 feel arbitrary, but it's the scale of the player object. This puts the bullet at the front edge of the player
            bullet.transform.position = this.transform.position + (this.transform.up * 0.15f);

            bullet.GetComponent<Rigidbody>().velocity = this.transform.up * bullet.GetComponent<BulletController>().speed;
            canTime = true;
            gunSource.Play();
        }
    }

    void OnCollisionEnter(Collision other){
        if(other.gameObject.tag == "EnemyBullet"){
            deathSource.Play();
            gameController.PlayerDeath();
            gameObject.AddComponent<TriangleExplosion>();
            StartCoroutine(gameObject.GetComponent<TriangleExplosion>().SplitMesh(true));
            
        }
    }
}
