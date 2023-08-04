using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using JetBrains.Annotations;

public class Bomb : MonoBehaviour
{
    Collider2D[] inExplosionRadius = null;
    public float exRadius;
    public float forceX, forceY;
    private float exForceMulti;

    Vector3 throwVector;
    Rigidbody2D rb;
    // public Rigidbody2D rb2d;
    public float throwForce;

    public Collider2D triggerCollider2D;

    private void Start()
    {

        rb = GetComponent<Rigidbody2D>();
        

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 distance = mousePos - this.transform.position;

        throwVector = distance.normalized * throwForce * 100;
        Throw();

        
    }

    private void Update()
    {
        


    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Ground"))
        {
            Explode();
        }
    }


    void Explode()
    {
        Debug.Log("GG");
        inExplosionRadius = Physics2D.OverlapCircleAll(transform.position, exRadius);

        foreach(Collider2D c in inExplosionRadius)
        {
            Rigidbody2D c_rigigbody = c.GetComponent<Rigidbody2D>();
            {
                if(c_rigigbody != null)
                {
                    Vector2 distanceVector = c.transform.position - transform.position;
                    if(distanceVector.magnitude > 0)
                    {
                        float exForce = exForceMulti / distanceVector.magnitude;
                        c_rigigbody.AddForce(distanceVector.normalized * exForce * 100);  
                    }
                }
            }
        }
        
        StartCoroutine(DestroyBomb());

    }

    IEnumerator DestroyBomb()
    {
        triggerCollider2D.enabled = false;
        yield return new WaitForSeconds(0.1f);
        Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, exRadius);
    }

    private void Throw()
    {
        Debug.Log("pp");
        rb.AddForce(throwVector);
    }

}
