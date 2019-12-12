using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletController : MonoBehaviour
{
    List<EnemyBullet> bullets = new List<EnemyBullet>();
    void Start(){
        //Populate the list
        foreach(GameObject enemybullet in GameObject.FindGameObjectsWithTag("EnemyBullet")){            
            bullets.Add(enemybullet.GetComponent<EnemyBullet>());
        }
    }

    public void Shoot(Transform enemySource, Transform playerTransform){

        //Only shoot if a bullet is available
        for(int i = 0; i < bullets.Count; i++){
            
            if(bullets[i].transform.position == bullets[i].home){
                bullets[i].transform.position = enemySource.position + (enemySource.forward * 0.25f);
                bullets[i].GetComponent<Rigidbody>().velocity = enemySource.transform.forward * bullets[i].speed;
                i = bullets.Count;

            }
        }
    }
}
