using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushProjectile : MonoBehaviour
{
    // Start is called before the first frame update
    Rigidbody rigidbody;
    float damageMulitplier = 2f;
    float projectileSpeed = 400;
    public GameObject shooter;
    Units shooterStat;
    public Grid grid;
    
    void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        shooterStat = shooter.GetComponent<Units>();
    }

    void Update() {
        if(transform.position.magnitude > 1000.0f)
        {
            Destroy(gameObject);
        }
    }

    public void Launch(Vector3 direction) {
        rigidbody.AddForce(direction * projectileSpeed);
    }

    void OnTriggerEnter(Collider other) {
        if(other.tag == "Enemy") {
            Units enemyStat = other.gameObject.GetComponent<Units>();
            enemyStat.TakeDmg(shooterStat.dmg);
            //Debug.Log("Enemy HP: " + enemyStat.currHP);        
            Destroy(gameObject);
        }
    }
}