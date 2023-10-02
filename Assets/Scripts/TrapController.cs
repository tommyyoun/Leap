using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapController : MonoBehaviour
{
    private GameObject[] traps;
    private GameObject trap;
    private Rigidbody rb;
    private Vector3 startingPos;
    // Start is called before the first frame update
    void Start()
    {
        startingPos = transform.position;
        traps = GameObject.FindGameObjectsWithTag("Trap");

        foreach (GameObject tr in traps)
        {
            if (tr.transform.position == startingPos)
            {
                trap = tr;
            }
        }

        rb = trap.GetComponent<Rigidbody>();
        rb.isKinematic = true;
        rb.useGravity = false;
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

    private IEnumerator waitForGravity()
    {
        yield return new WaitForSeconds(2);

        rb.isKinematic = false;
        rb.useGravity = true;

        StartCoroutine(hide());
    }

    private IEnumerator hide()
    {
        yield return new WaitForSeconds(2);

        rb.useGravity = false;
        trap.SetActive(false);

        transform.position = startingPos;
        rb.isKinematic = true;
    }
}
