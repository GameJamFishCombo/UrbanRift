using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Units
{
    Transform player;
    Vector3 playerPos;
    public float duration = 0.09f;
    public GameObject projectilePrefab;
    Rigidbody rigidbody;
    bool canMove = true;
    public float projectileSpeed = 450;

    private Queue<MovementInput> buffer;
    void Awake()
    {
        buffer = new Queue<MovementInput>();
        rigidbody = GetComponent<Rigidbody>();
        player = GetComponent<Transform>();
        playerPos = rigidbody.position;
    }

    public void Update() {
        // testing why github sucks ass
        if(Input.GetKeyDown(KeyCode.UpArrow)) {
            if(buffer.Count < 3)
                buffer.Enqueue(MovementInput.Up);
        }

        if(Input.GetKeyDown(KeyCode.LeftArrow)) {
            if(buffer.Count < 3)
                buffer.Enqueue(MovementInput.Left);
        }
            
        if(Input.GetKeyDown(KeyCode.DownArrow)) {
            if(buffer.Count < 3)
                buffer.Enqueue(MovementInput.Down);
        }

        if(Input.GetKeyDown(KeyCode.RightArrow)) {
            if(buffer.Count < 3)
                buffer.Enqueue(MovementInput.Right);
        }

        if(Input.GetKeyDown(KeyCode.C) && canMove) //if between tiles, round up or down
        {
            Launch();
        }

        move();

        if(Input.GetKeyDown(KeyCode.K)) {
            TakeDmg(5);
        }
    }

    private void move(){
        if(canMove && buffer.Count > 0) {        
            MovementInput input = buffer.Dequeue();
            if(input == MovementInput.Up) {
                Vector3 move = new Vector3(0, 0, 1f) + player.position;
                if(!(move.x < 0 || move.z < 0 || move.x > 3 || move.z > 3)) {
                    StartCoroutine(LerpPosition(move, duration));
                }

            }

            if(input == MovementInput.Left) {
                Vector3 move = new Vector3(-1f, 0, 0) + player.position;
                if(!(move.x < 0 || move.z < 0 || move.x > 3 || move.z > 3)) {
                    StartCoroutine(LerpPosition(move, duration));
                }

            }
            
            if(input == MovementInput.Down) {
                Vector3 move = new Vector3(0, 0, -1f) + player.position;
                if(!(move.x < 0 || move.z < 0 || move.x > 3 || move.z > 3)) {
                    StartCoroutine(LerpPosition(move, duration));
                }

            }

            if(input == MovementInput.Right) {
                Vector3 move = new Vector3(1f, 0, 0) + player.position;
                if(!(move.x < 0 || move.z < 0 || move.x > 3 || move.z > 3)) {
                    StartCoroutine(LerpPosition(move, duration));
                }

            }
        }
    }

    IEnumerator LerpPosition(Vector3 targetPosition, float duration) {
        canMove = false;
        float time = 0;
        Vector3 startPosition = player.position;

        while (time < duration) {
            player.position = Vector3.Lerp(startPosition, targetPosition, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        player.position = targetPosition;
        canMove = true;
    }

    void Launch() {
        GameObject projectileObject = Instantiate(projectilePrefab, player.position, Quaternion.identity);

        Projectile projectile = projectileObject.GetComponent<Projectile>();
        Vector3 lookDirection = new Vector3(1f, 0, 0);
        projectile.Launch(lookDirection, projectileSpeed);

        // animator.SetTrigger("Launch");
        
        // PlaySound(throwSound);
    }

    public override void Die() {
        base.Die();
        Destroy(this.gameObject);
        Debug.Log(player + " dead.");
    }
}

public enum MovementInput
{
    Up = 0,
    Left = 1,
    Down = 2,
    Right = 3,
}