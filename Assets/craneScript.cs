using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;
using UnityEngine.UI;

public class craneScript : MonoBehaviour
{
    public Camera cam;
    
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

    private float trolley_moved_amount = 0;

    [SerializeField]private GameObject concrete_block;
    [SerializeField]private Vector3 concrete_pos;

    [SerializeField]private float current_rotation;

    [SerializeField] private Vector3 normal_vector;

    [SerializeField] private float concrete_angle;

    [SerializeField] private float distance;
    
    //values relating to auto-find
    [SerializeField]private bool step1 = false;
    [SerializeField]private bool step2 = false;
    [SerializeField]private bool step3 = false;
    [SerializeField]private bool step4 = false;
    [SerializeField]private bool step5 = false;
    
    [SerializeField] private bool has_found = false;
    [SerializeField] private bool is_searching = false;



    // Start is called before the first frame update
    void Start()
    {
        concrete_block = GameObject.FindWithTag("concrete");
        concrete_pos = concrete_block.transform.position;
        current_rotation = 0;
    }

    // Update is called once per frame
    void Update()
    {   
        
        rotate_crane();
        move_trolley();
        scale_cable();
        
        if (is_searching == true)
        {
            StartCoroutine(auto_move());
        }

        if (Input.GetMouseButtonDown(0) && is_searching == false)
        {
            if (mouse_input())
            {
                var hook = crane_hook.GetComponent<hookScript>();
                hook.is_grabbed = false;
                concrete_pos = concrete_block.transform.position;
                current_rotation = 0;
                step1 = false;
                step2 = false;
                step3 = false;
                step4 = false;
                step5 = false;
                is_searching = true;
            }
            
        }


    }
    
    
    public void rotate_crane()
    {   
        if (right_held && !is_searching)    
        {
            rotate_right();
        }
        
        if (left_held && !is_searching)
        {
            rotate_left();
        }
        
        if (current_rotation > 360)
        {
            current_rotation = 0;
        }
        else if (current_rotation < -360)
        {
            current_rotation = 0;
        }

        if (!is_searching)
        {
            update_points();
        }
        
    }

    public void rotate_right()
    {
        //Debug.Log("right held");
        transform.Rotate(Vector3.down,0.1f,Space.World);

        crane_trolley.transform.Rotate(Vector3.down,0.1f,Space.World);

        crane_cable.transform.Rotate(Vector3.down,0.1f,Space.World);

        crane_hook.transform.Rotate(Vector3.down,0.1f,Space.World);

        near_limit.transform.Rotate(Vector3.down,0.1f,Space.World);

        far_limit.transform.Rotate(Vector3.down,0.1f,Space.World);

        current_rotation -= 0.1f;
        
    }

    public void rotate_left()
    {
        //Debug.Log("left held");
        transform.Rotate(Vector3.up,0.1f, Space.World);

        crane_trolley.transform.Rotate(Vector3.up,0.1f,Space.World);

        crane_cable.transform.Rotate(Vector3.up,0.1f,Space.World);

        crane_hook.transform.Rotate(Vector3.up,0.1f,Space.World);

        near_limit.transform.Rotate(Vector3.up,0.1f,Space.World);

        far_limit.transform.Rotate(Vector3.up,0.1f,Space.World);
            
        current_rotation += 0.1f;
        
    }

    public void update_points()
    {
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
        if (!is_searching)
        {
            crane_trolley.transform.Translate(new Vector3(1  * trolley_slider_value,0,0));
            crane_cable.transform.Translate(new Vector3(1  * trolley_slider_value,0,0));
            crane_hook.transform.Translate(new Vector3(1  * trolley_slider_value,0,0));
        }
        
        
    }

    public void move_trolley_amount(float amount)
    {
        var near_limit_pos = near_limit.transform.position;
        var far_limit_pos = far_limit.transform.position;

        crane_trolley.transform.Translate(new Vector3(1  * amount,0,0));
        crane_cable.transform.Translate(new Vector3(1  * amount,0,0));
        crane_hook.transform.Translate(new Vector3(1  * amount,0,0));

        trolley_moved_amount += Mathf.Abs(amount);
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
    
    public void find_concrete()
    {
        normal_vector = Vector3.ProjectOnPlane(concrete_pos, Vector3.up);
        
        concrete_angle = (Vector3.SignedAngle(transform.forward, concrete_pos, Vector3.up)) * -1;

        if (90 - concrete_angle <= 0.1f && 90 - concrete_angle > 0)
        {
            step1 = true;
        }
        else
        {
            rotate_right();
            step1 = false;
        }
        
    }

    public void move_trolley_to_concrete()
    {  
        distance = Vector3.Distance(new Vector3(concrete_pos.x, 0, concrete_pos.z), new Vector3(
            crane_trolley.transform.position.x, 0,
            crane_trolley.transform.position.z));
        
        if (distance > 0.1f)
        {
            if (trolley_moved_amount <= 50)
            {
                move_trolley_amount(0.01f);
            }
            
        }
        if (distance < 0.1f)
        {
            step2 = true;
        }
        
        
    }

    public void grab_concrete()
    {
        var hook = crane_hook.GetComponent<hookScript>();
        
        
        if (!hook.is_grabbed)
        {
            cable_slider_value += 0.002f;
            var new_hook_position = crane_cable.transform.TransformPoint(Vector3.down * 15);
            crane_hook.transform.position = new_hook_position;
        }
        else
        {
            step3 = true;
        }
        
    }
    public void raise_concrete()
    {   
        var hook = crane_hook.GetComponent<hookScript>();
        
        if (hook.is_grabbed && cable_slider_value > 0f)
        {   
            cable_slider_value -= 0.002f;
            var new_hook_position = crane_cable.transform.TransformPoint(Vector3.down * 15);
            crane_hook.transform.position = new_hook_position;
        }
        else
        {
            step4 = true;
        }
    }


    public void teleport_concrete()
    {
        var hook = crane_hook.GetComponent<hookScript>();
        hook.is_grabbed = false;

        float angle = Random.Range(0f, 2 * Mathf.PI);
        float radius = Random.Range(10f, 20f);
        
        float x_pos = Mathf.Cos(angle) * radius;
        float z_pos = Mathf.Sin(angle) * radius;
        float y_pos = Random.Range(10f, 20f);
        
        concrete_block.transform.position = new Vector3(x_pos, y_pos, z_pos);
        
        step5 = true;
        
    }

    public bool mouse_input()
    {
        var ray = cam.ScreenPointToRay(Input.mousePosition);
        var difference = ray.origin.y - 10;
        Vector3 direction = ray.direction / ray.direction.y;
        Vector3 point = ray.origin - direction * difference;
        Debug.Log(point);

        if (Vector3.Distance(point, concrete_block.transform.position) < 30f)
        {
            return true;
        }
        else
        {
            return false;
        }
        
    }
    
    
    public IEnumerator auto_move()
    {
        //find concrete
        if (step1 == false)
        {
            find_concrete();
            update_points();
        }

        if (step2 == false && step1 == true)
        {
            move_trolley_to_concrete();
        }

        if (step3 == false && step2 == true)
        {
            grab_concrete();
        }

        if (step4 == false && step3 == true)
        {   
            yield return new WaitForSeconds(1f);
            raise_concrete();
        }

        if (step5 == false && step4 == true)
        {
            teleport_concrete();
            
        }

        if (step5 == true)
        {   
            yield return new WaitForSeconds(0.1f);
            is_searching = false;
        }

        
        

    }
    
}
