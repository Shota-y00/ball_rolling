using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class collision_func : MonoBehaviour
{
    private Ending_func ending;
    private int count;
    private floor_func floor_obj;

    // Start is called before the first frame update
    void Start()
    {
        ending = new Ending_func();
        count = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnCollisionEnter(Collision collision)
    {
        count++;
        if(collision.gameObject.tag == "Goal")
        {
            ending.goClear();
        }

    }
}
