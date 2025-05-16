using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Egg : MonoBehaviour
{
    [Header(" Physics Settins ")]
    [SerializeField] private float bounceVelocity;
    private Rigidbody2D rig;
    // Start is called before the first frame update
    void Start()
    {
          rig = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Bounce(Vector2 normal){
        rig.velocity = normal * bounceVelocity;
    }
    private void OnCollisionEnter2D(Collision2D c){
        if(c.collider.TryGetComponent(out PlayerController pc)){
            Bounce(c.GetContact(0).normal);
        }
    }
}
