using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SensorStatusUI : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject robot;
    private RobotBrain robotBrain;
    public Text ColorRight;
    public Text ColorFront;
    public Text ColorLeft;
    public Text RangeRight;
    public Text RangeFront;
    public Text RangeLeft;
    void Start()
    {
        robotBrain = robot.GetComponent<RobotBrain>();
    }

    // Update is called once per frame
    void Update()
    {
        //ColorRight.text=robotBrain.RightColor
    }
}
