using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotArmBrain : MonoBehaviour
{

    public float RotationSpeed = 3.0f; // seconds

    public bool armActive = false;
    private float angle = 0;

    public GameObject Hinge;
    public GameObject Magnet;

    public Rigidbody GrabbedBody;

    public float MagnetGrabDistance=0.4f;
    public float MagnetStrength = 10f;
    public float magnetEngageAngle = 160f;
    // Start is called before the first frame update
    void Start()
    {

    }

    void grabObject()
    {
        RaycastHit hit;
        Transform magnetpos = Magnet.transform;
        Vector3 rayAngle = magnetpos.right * -1;

        Debug.DrawRay(magnetpos.position,
                      rayAngle * MagnetGrabDistance,
                      Color.red);

        if (Physics.Raycast(magnetpos.position,
                        rayAngle,
                        out hit, MagnetGrabDistance))
        {
            GrabbedBody = hit.rigidbody;
        } else
        {
            GrabbedBody = null;
        }

        if (GrabbedBody != null && Hinge.transform.localEulerAngles.y>= magnetEngageAngle)
        {
            GrabbedBody.AddForce(rayAngle * -1 * MagnetStrength);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        grabObject();
        if (armActive)
        {
            if (Hinge.transform.localEulerAngles.y < 180) angle += RotationSpeed;
            Hinge.transform.localRotation = Quaternion.Euler(0, angle, 0);
        }
            
        else
        {
            if (Hinge.transform.localEulerAngles.y > 0) angle -= RotationSpeed;
            Hinge.transform.localRotation = Quaternion.Euler(0, angle, 0);
        }
    }
}
