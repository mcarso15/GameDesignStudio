using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public float speed = 10f;

    Rigidbody rigidbody;

    public Vector3 home;

    void Start(){
        rigidbody = GetComponent<Rigidbody>();
        home = this.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision other){
        this.transform.position = home;
        rigidbody.velocity = Vector3.zero;

        //Still bugfixing
        print(other.gameObject.name);
    }
}
