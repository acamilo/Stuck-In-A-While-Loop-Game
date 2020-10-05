using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ProximitySensorBrain : MonoBehaviour
{
    public GameObject SensorHead;
    public GameObject IndicatorLight;
    public float sweepRange = 1;
    public float sweepAngle = 15;

    public float SigOffset=0;

    public float sensorOutput=0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void FixedUpdate()
    {
        RaycastHit hit;
        Transform sensorPos = SensorHead.transform;
        sensorOutput = 0f;


        foreach (int rangeval in Enumerable.Range(-5,10))
        {

            float angle = (rangeval/5f) * sweepAngle;
            Vector3 rayAngle = Quaternion.AngleAxis(angle, sensorPos.up) * sensorPos.forward;
            if (Physics.Raycast(sensorPos.position,
                                rayAngle,
                                out hit, sweepRange))
            {
                Debug.DrawRay(sensorPos.position,
                              rayAngle * sweepRange,
                              Color.red);
                sensorOutput += (1-(hit.distance / sweepRange)) * 0.1f;
            }
            else
            {
                Debug.DrawRay(sensorPos.position,
                              rayAngle * sweepRange,
                              Color.blue);
            }
        }
        //Color indicatorcolor = Color.Lerp(Color.white, new Color(sensorOutput, 0, 0, 0), sensorOutput);
        if (sensorOutput>0.2)
        {
            IndicatorLight.GetComponent<Renderer>().material.color = Color.red;
        } else
        IndicatorLight.GetComponent<Renderer>().material.color = Color.white;
    }
}

