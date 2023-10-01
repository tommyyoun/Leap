using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapController : MonoBehaviour
{
    private GameObject trap;
    private Rigidbody rb;
    private Vector3 startingPos = new Vector3(-0.8640066f, 0.582f, 5.881f);
    // Start is called before the first frame update
    void Start()
    {
        trap = GameObject.Find("Trap");
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
        rb.useGravity = false;
        transform.position = startingPos;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Frog")
        {
            StartCoroutine(waitForGravity());
        }
    }

    private IEnumerator hide()
    {
        yield return new WaitForSeconds(2);

        rb.useGravity = false;
        trap.SetActive(false);

        transform.position = startingPos;
        rb.isKinematic = true;
    }

    private IEnumerator waitForGravity()
    {
        yield return new WaitForSeconds(2);

        rb.isKinematic = false;
        rb.useGravity = true;

        StartCoroutine(hide());
    }
}
