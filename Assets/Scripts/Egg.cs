using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(Rigidbody2D))]
public class Egg : MonoBehaviour
{
    [Header(" Physics Settins ")]
    [SerializeField] private float bounceVelocity;
    private Rigidbody2D rig;
    private bool isAlive;
    private float gravityScale;
    [Header(" Events ")]
    public static Action onHit;
    public static Action onFellInWater;
    // Start is called before the first frame update
    void Start()
    {
          rig = GetComponent<Rigidbody2D>();
          isAlive=true;
          gravityScale= rig.gravityScale;
          rig.gravityScale=0;
          StartCoroutine("WaitAndFall");
    }
    IEnumerator WaitAndFall(){
        yield return new WaitForSeconds(2);
        rig.gravityScale=gravityScale;
    }
    public void Reuse(){
        transform.position =Vector2.up *5;
        rig.velocity=Vector2.zero;
        rig.angularVelocity=0;
        rig.gravityScale=0;
        isAlive=true;
        StartCoroutine("WaitAndFall");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Bounce(Vector2 normal){
        rig.velocity = normal * bounceVelocity;
    }
    private void OnCollisionEnter2D(Collision2D c){
        if (!isAlive)
            return;
        if(c.collider.TryGetComponent(out PlayerController pc)){
            Bounce(c.GetContact(0).normal);
            onHit?.Invoke();
        }
    }
    private void OnTriggerEnter2D(Collider2D collider){
        if (!isAlive)
            return;
        if(collider.CompareTag("Water")){
            isAlive=false;
            onFellInWater?.Invoke();
        }
    }
}
