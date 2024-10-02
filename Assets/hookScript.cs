using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Timeline;

public class hookScript : MonoBehaviour
{   

    public bool is_grabbed = false;
    public GameObject crane_cable;

    private GameObject concrete;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (is_grabbed)
        {
            concrete.transform.position = transform.TransformPoint(Vector3.down * 3);
        }
    }

    private void OnTriggerEnter(Collider col)
    {
        Debug.Log("collision!");
        is_grabbed = true;

        concrete = col.gameObject;
    }


}
