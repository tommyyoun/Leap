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
        this.startingPos = this.transform.position;
        this.traps = GameObject.FindGameObjectsWithTag("Trap");

        foreach (GameObject tr in this.traps)
        {
            if (tr.transform.position == this.startingPos)
            {
                this.trap = tr;
            }
        }

        this.rb = GetComponent<Rigidbody>();
        this.rb.isKinematic = true;
        this.rb.useGravity = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Frog")
        {
            StartCoroutine(this.waitForGravity());
        }
    }

    private IEnumerator waitForGravity()
    {
        yield return new WaitForSeconds(2);

        this.rb.isKinematic = false;
        this.rb.useGravity = true;

        StartCoroutine(this.hide());
    }

    private IEnumerator hide()
    {
        yield return new WaitForSeconds(2);

        this.rb.useGravity = false;
        this.trap.SetActive(false);

        this.transform.position = startingPos;
        this.rb.isKinematic = true;
    }
}
