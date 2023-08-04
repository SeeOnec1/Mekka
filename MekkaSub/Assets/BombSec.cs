using System.Collections;
using UnityEngine;

public class BombSec : MonoBehaviour
{

    GameObject playerObj;
    float distanceToPlayerX, distanceToPlayerY;
    Vector2 distanceToPlayer;
    Rigidbody2D playerRB;

    public float expForceX, expForceY;
    PlayerMovement playerMovement;

    Vector3 throwVector;
    public Rigidbody2D rb;
    public float throwForce;
    public Collider2D triggerCollider2D;

    public float explosionDelay;

    public SpriteRenderer ren;
    public Color color;
    public float speed;

    Rocket rocket;
    GameObject rocketObj;

    // Start is called before the first frame update
    void Start()
    {
        playerObj = GameObject.FindGameObjectWithTag("Player");
        playerRB = playerObj.GetComponent<Rigidbody2D>();
        playerMovement = playerObj.GetComponent<PlayerMovement>();

        rocketObj = GameObject.FindGameObjectWithTag("Rocket");
        rocket = rocketObj.GetComponent<Rocket>();

        StartCoroutine(ChangeEngineColour());

        //rb = GetComponent<Rigidbody2D>();
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 distance = mousePos - this.transform.position;

        throwVector = distance.normalized * throwForce * 100;
        Throw();
    }

    // Update is called once per frame
    void Update()
    {
        distanceToPlayerX = transform.position.x - playerObj.transform.position.x;
        distanceToPlayerY = transform.position.y - playerObj.transform.position.y;
        distanceToPlayer = new Vector2(distanceToPlayerX, distanceToPlayerY);

        if (Input.GetMouseButtonDown(1))
        {
            Explode();
        }

    }

    void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("Ground"))
        {
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
            StartCoroutine(ExplosionDelay());
            //Explode();
            //Debug.Log("Yes");
        }

    }

    IEnumerator ExplosionDelay()
    {
        yield return new WaitForSeconds(explosionDelay);
        Explode();
        yield return null;
    }

    void Explode()
    {
        //Debug.Log(distanceToPlayer.magnitude);
        if (distanceToPlayer.magnitude < 2.5f)
        {
            //Debug.Log("Yes");
            playerMovement.ExplosionBufferEnabler();
            playerRB.AddForce(new Vector2(expForceX, expForceY) * -distanceToPlayer.normalized);
        }

        rocket.BombExploded();

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
        Gizmos.DrawWireSphere(transform.position, 2.5f);
    }

    private void Throw()
    {
        rb.AddForce(throwVector);
    }

    private IEnumerator ChangeEngineColour()
    {
        float tick = 0f;
        while (ren.color != color)
        {
            tick += Time.deltaTime * speed;
            ren.color = Color.Lerp(Color.green, color, tick);
            yield return null;
        }
    }

}
