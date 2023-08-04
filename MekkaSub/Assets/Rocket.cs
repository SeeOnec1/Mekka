using System.Collections;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    /*
    public float rotationOffset;
    public float zpos;
    */

    public Transform shootingPoint;
    public GameObject bombPrefab;

    public Rigidbody2D playerRB;

    //GameObject bomb;

    bool canFireBomb;
    float bombsFired;
    public float bombDelay;

    bool delayOver;

    Vector2 recoilVector;
    public float recoil;

    private void Start()
    {
        canFireBomb = true;
        bombsFired = 0;
        delayOver = true;
    }
    // Update is called once per frame
    void Update()
    {

        Vector2 dir = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
        transform.rotation = rotation;


        //Debug.Log(bombsFired);

        #region Secondary
        /*
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 0;
        Vector3 objectPos = Camera.main.WorldToScreenPoint(transform.position);
        objectPos.z = 0;

        mousePos.x = mousePos.x - objectPos.x;
        mousePos.y = mousePos.y - objectPos.y;

        float angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, angle + rotationOffset));

        Vector3 targetPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        targetPos.z = 0;
        */
        #endregion

        

        if (Input.GetMouseButtonDown(0) && canFireBomb && delayOver)
        {
            RocketLaunch();
        }

        
    }

    void RocketLaunch()
    {
        canFireBomb = false;

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 distance = mousePos - this.transform.position;

        recoilVector = -distance.normalized * recoil * 100;
        Recoil();

        Instantiate(bombPrefab, shootingPoint.position, Quaternion.identity);
        bombsFired++;

        if (bombsFired == 2)
            StartCoroutine(BombFiringDelay());
        else if (bombsFired == 1)
            StartCoroutine(BombsMidReset());
            

    }

    public void BombExploded()
    {
        canFireBomb = true;
    }

    IEnumerator BombFiringDelay()
    {
        delayOver = false;
        yield return new WaitForSeconds(bombDelay);
        delayOver = true;
        bombsFired = 0;
    }

    IEnumerator BombsMidReset()
    {
        yield return new WaitForSeconds(bombDelay);
        bombsFired = 0;
    }

    void Recoil()
    {
        playerRB.AddForce(recoilVector);
    }

}
