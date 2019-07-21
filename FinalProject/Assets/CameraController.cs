//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
    public KeyCode forward_key;
    public KeyCode left_key;
    public KeyCode right_key;
    public KeyCode backward_key;
    public KeyCode up_key;
    public KeyCode down_key;

    public KeyCode crawl_key;
    public KeyCode sprint_key;

    public float speedPitch = 2;
    public float speedYaw = 2;

    public float crawl_step;
    public float normal_step;
    public float sprint_step;

    float step;

    float pitch = 0.5f*Mathf.PI, yaw = 0;

    /*
    // Start is called before the first frame update
    void Start() {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update() {
        yaw += Time.deltaTime * speedYaw * Input.GetAxis("Mouse X");
        pitch -= Time.deltaTime * speedPitch * Input.GetAxis("Mouse Y");
        transform.eulerAngles = new Vector3(pitch, yaw, 0);

        if (Input.GetKey(sprint_key))
            step = sprint_step;
        else if (Input.GetKey(crawl_key))
            step = crawl_step;
        else
            step = normal_step;

        step *= Time.deltaTime;

        if (Input.GetKey(forward_key))
            transform.Translate(0, 0, step);
        if (Input.GetKey(left_key))
            transform.Translate(-step, 0, 0);
        if (Input.GetKey(right_key))
            transform.Translate(step, 0, 0);
        if (Input.GetKey(backward_key))
            transform.Translate(0, 0, -step);
        if (Input.GetKey(up_key))
            transform.Translate(0, step, 0);
        if (Input.GetKey(down_key))
            transform.Translate(0, -step, 0);
    }
    */
}
