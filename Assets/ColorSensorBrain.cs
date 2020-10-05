using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorSensorBrain : MonoBehaviour
{
    public GameObject SensorHead;
    public GameObject IndicatorLight;

    public Color DetectedColor;

    public float SensorRange = 1;

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

        Vector3 rayAngle = sensorPos.up*-1;
        Debug.DrawRay(sensorPos.position,
              rayAngle* SensorRange,
              Color.red);

        if (Physics.Raycast(sensorPos.position,
                                rayAngle,
                                out hit, SensorRange))
        {
            Renderer renderer = hit.collider.GetComponent<MeshRenderer>();
            Texture2D texture2D = renderer.material.mainTexture as Texture2D;
            Vector2 pCoord = hit.textureCoord;
            try
            {
                pCoord.x *= texture2D.width;
                pCoord.y *= texture2D.height;
                Vector2 tiling = renderer.material.mainTextureScale;
                DetectedColor = texture2D.GetPixel(Mathf.FloorToInt(pCoord.x * tiling.x), Mathf.FloorToInt(pCoord.y * tiling.y));
            } catch (NullReferenceException)
            {
                // Doesn't have a texture
                DetectedColor = renderer.material.color;
            }

            //DetectedColor = GetComponent<Renderer>().material.color;


        } else
        {
            DetectedColor = Color.clear;
        }
        if (DetectedColor!=Color.clear) IndicatorLight.GetComponent<Renderer>().material.color = DetectedColor;
        else IndicatorLight.GetComponent<Renderer>().material.color = Color.gray;

    }
}
