using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotArmBrain : MonoBehaviour
{

    public float RotationSpeed = 3.0f; // seconds

    public bool armActive = false;
    private float angle = 0;

    public GameObject hinge;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (armActive)
        {
            if (hinge.transform.localEulerAngles.y < 180) angle += RotationSpeed;
            hinge.transform.localRotation = Quaternion.Euler(0, angle, 0);
        }
            
        else
        {
            if (hinge.transform.localEulerAngles.y > 0) angle -= RotationSpeed;
            hinge.transform.localRotation = Quaternion.Euler(0, angle, 0);
        }
    }
}
