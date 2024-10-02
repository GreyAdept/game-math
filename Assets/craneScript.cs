using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;
using UnityEngine.UI;

public class craneScript : MonoBehaviour
{   
    public GameObject crane_trolley;
    
    private bool left_held = false;
    private bool right_held = false;

    public GameObject left_button;
    public GameObject right_button;

    public GameObject near_limit;
    public GameObject far_limit;

    
    public GameObject crane_hook;
    public GameObject crane_cable;

    public float trolley_slider_value;
    public float cable_slider_value;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {   
        
        rotate_crane();
        
        //Debug.Log(trolley_slider_value);

        move_trolley();
        scale_cable();
        
    }

    public void rotate_crane()
    {   
        if (right_held)
        {
            //Debug.Log("right held");
            transform.Rotate(Vector3.down,0.4f,Space.World);

            crane_trolley.transform.Rotate(Vector3.down,0.4f,Space.World);

            crane_cable.transform.Rotate(Vector3.down,0.4f,Space.World);

            crane_hook.transform.Rotate(Vector3.down,0.4f,Space.World);

            near_limit.transform.Rotate(Vector3.down,0.4f,Space.World);

            far_limit.transform.Rotate(Vector3.down,0.4f,Space.World);

        }
        
        if (left_held)
        {
            //Debug.Log("left held");
            transform.Rotate(Vector3.up,0.4f, Space.World);

            crane_trolley.transform.Rotate(Vector3.up,0.4f,Space.World);

            crane_cable.transform.Rotate(Vector3.up,0.4f,Space.World);

            crane_hook.transform.Rotate(Vector3.up,0.4f,Space.World);

            near_limit.transform.Rotate(Vector3.up,0.4f,Space.World);

            far_limit.transform.Rotate(Vector3.up,0.4f,Space.World);

        }

        var new_trolley_position = transform.TransformPoint(Vector3.left * 20 + Vector3.up * 40);
        crane_trolley.transform.position = new_trolley_position;

        var new_cable_position = crane_trolley.transform.TransformPoint(Vector3.down);
        crane_cable.transform.position = new_cable_position;

        var new_hook_position = crane_cable.transform.TransformPoint(Vector3.down * 15);
        crane_hook.transform.position = new_hook_position;

        var new_far_limit_position = transform.TransformPoint(Vector3.left * 20 + Vector3.up * 40);
        far_limit.transform.position = new_far_limit_position;

        var new_near_limit_position = transform.TransformPoint(Vector3.left * 10 + Vector3.up * 40);
        near_limit.transform.position = new_near_limit_position;


        
        
    }

    public void move_trolley()
    {   
        var near_limit_pos = near_limit.transform.position;
        var far_limit_pos = far_limit.transform.position;

        crane_trolley.transform.Translate(new Vector3(1  * trolley_slider_value,0,0));
        crane_cable.transform.Translate(new Vector3(1  * trolley_slider_value,0,0));
        crane_hook.transform.Translate(new Vector3(1  * trolley_slider_value,0,0));

        

    }


   

    public void scale_cable()
    {
        crane_cable.transform.localScale = new Vector3(1,cable_slider_value,1);
    }


    public void toggle_left()
    {
        left_held = !left_held;
    }

    public void toggle_right()
    {
        right_held = !right_held;
    }


    public void check_slider_value(float slidervalue)
    {
        trolley_slider_value = slidervalue;
    }

    public void check_cable_length(float slidervalue)
    {
        cable_slider_value = slidervalue;
    }

}
