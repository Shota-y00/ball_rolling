using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lookMaze : MonoBehaviour
{
    private GameObject target;

    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("MazeBase");
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.LookAt(target.transform.position);
    }
}
