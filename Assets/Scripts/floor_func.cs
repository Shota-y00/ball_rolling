using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class floor_func : MonoBehaviour
{
    public float time = 5.0f;
    private Rigidbody rb;
    private bool is_drop;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
        is_drop = false;
        setTimer(input_data.time);
    }

    public void setTimer(float t)
    {
        time = t;
    }

    // Update is called once per frame
    void Update()
    {
        if (is_drop)
        {
            time -= Time.deltaTime;
            if(time <= 0)
            {
                rb.isKinematic = false;

            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "ball")
        {
            is_drop = true;
        }
    }
}
