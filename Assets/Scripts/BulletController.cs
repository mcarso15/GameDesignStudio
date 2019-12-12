using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float speed = 10f;

    Rigidbody rigidbody;

    public Vector3 home = new Vector3(-10,0,0);

    void Start(){
        rigidbody = GetComponent<Rigidbody>();
    }
    void OnCollisionEnter(Collision other){
        if(other.gameObject.tag == "Wall" || other.gameObject.tag == "Enemy"){
            this.transform.position = home;
            rigidbody.velocity = Vector3.zero;
        }
    }
}
