using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MoonSharp.Interpreter;
using System;

public class RobotBrain : MonoBehaviour
{
    public WheelCollider RightWheel;
    public WheelCollider LeftWheel;

    public float RightWheelTorqueMax = 30f;
    public float LeftWheelTorqueMax = 30f;
    public float WheelBrakeTorque = 50f;

    public Text codeEditor;

    public bool autoMode = false;
    private bool codeError = false;
    public float RightWheelTorque = 0f;
    public float LeftWheelTorque = 0f;

    public float teleopLeftMotorTorque = 0f;
    public float teleopRightMotorTorque = 0f;

    private Script luaScript;
    private string scriptCode = "";


    // These are private classes to be shared with the lua inerp.
    private robotMotor motor;
    private RangeValueUpdater range;
    ColorValueUpdater colorscanner;
    RobotArmUpdater arm;
  

    public GameObject LeftRangefinder;
    public GameObject FrontRangefinder;
    public GameObject RightRangefinder;

    public GameObject LeftColor;
    public GameObject FrontColor;
    public GameObject RightColor;

    public GameObject RobotArm;


    // Start is called before the first frame update
    void Start()
    {
        luaScript = new Script();
        UserData.RegisterAssembly();
        motor = new robotMotor(RightWheel, LeftWheel, WheelBrakeTorque, RightWheelTorqueMax,LeftWheelTorqueMax);
        range = new RangeValueUpdater(RightRangefinder,FrontRangefinder,LeftRangefinder);
        luaScript.Globals["motor"] = motor;
        luaScript.Globals["range"] = range;

        colorscanner = new ColorValueUpdater(LeftColor, FrontColor, RightColor);
        luaScript.Globals["color"] = colorscanner;

        arm = new RobotArmUpdater(RobotArm.GetComponent<RobotArmBrain>() );
        luaScript.Globals["arm"] = arm;

        scriptCode = codeEditor.text;
        RightWheel.brakeTorque = WheelBrakeTorque;
        LeftWheel.brakeTorque = WheelBrakeTorque;
    }

    // Update is called once per frame
    void Update()
    {
        scriptCode = codeEditor.text;
        if (Input.GetKey(KeyCode.Q))
        {
            teleopRightMotorTorque = RightWheelTorqueMax;
            //RightWheel.brakeTorque = 0f;
        } else if(Input.GetKey(KeyCode.A))
        {
            teleopRightMotorTorque = -RightWheelTorqueMax;
            //RightWheel.brakeTorque = 0f;
        } else
        {
            teleopRightMotorTorque = 0f;
            //RightWheel.brakeTorque = 500f;
        }

        if (Input.GetKey(KeyCode.W))
        {
            teleopLeftMotorTorque = LeftWheelTorqueMax;
            //LeftWheel.brakeTorque = 0f;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            teleopLeftMotorTorque = -LeftWheelTorqueMax;
            //LeftWheel.brakeTorque = 0f;
        }
        else
        {
            teleopLeftMotorTorque = 0f;
            //LeftWheel.brakeTorque = 500f;
        }
        

    }

    private void FixedUpdate()
    {
        try
        {
            DynValue res = luaScript.DoString(scriptCode);
            Debug.Log(res);
            codeError = false;
            codeEditor.color = Color.black;
        } catch (Exception ex)
        {
            Debug.Log(ex.Message);
            codeError = true;
            if (ex is SyntaxErrorException)
            {
                codeEditor.color = Color.red;
                
            } else throw; // yeet this back to unity.
        }


        // So we can see it in the inspector
        RightWheelTorque = (float)motor.right;
        LeftWheelTorque = (float)motor.left;
        
        // Set RobotTorques
        //double scriptRightMotorTorque = luaScript.Globals.Get("right_motor_torque").Number;
        //double scriptLeftMotorTorque = luaScript.Globals.Get("left_motor_torque").Number;

        if (autoMode && codeError)
        {
            motor.right = 0;
            motor.left = 0;
        } else { 
            if (!autoMode) motor.right = teleopRightMotorTorque;
            if (!autoMode) motor.left = teleopLeftMotorTorque;
        }

        motor.FixedUpdate();
        range.FixedUpdate();
        colorscanner.FixedUpdate();



    }
}
[MoonSharpUserData]
public class robotMotor
{
    public double left = 0;
    public double right = 0;

    private float wheelBrakeTorque;
    private WheelCollider rightCollider;
    private WheelCollider leftCollider;

    private float rightWheelMaxTorque=0;
    private float leftWheelMaxTorque=0;
    public robotMotor(WheelCollider r, WheelCollider l, float t, float lwtm,float rwtm)
    {
        rightCollider = r;
        leftCollider = l;
        wheelBrakeTorque = t;
        rightWheelMaxTorque = rwtm;
        leftWheelMaxTorque = lwtm;
    }

    public void FixedUpdate()
    {
        float leftTorque = (float)left;
        leftTorque = Mathf.Clamp(leftTorque, -1f, 1f);
        leftCollider.motorTorque = leftTorque* leftWheelMaxTorque;
        if (left != 0f) leftCollider.brakeTorque = 0f;
        else leftCollider.brakeTorque = wheelBrakeTorque;

        float rightTorque = (float)right;
        rightTorque = Mathf.Clamp(rightTorque, -1f, 1f);
        rightCollider.motorTorque = rightTorque*rightWheelMaxTorque;
        if (right != 0f) rightCollider.brakeTorque = 0f;
        else rightCollider.brakeTorque = wheelBrakeTorque;
    }
}
[MoonSharpUserData]
public class RangeValueUpdater
{

    public float left = 0;
    public float front = 0;
    public float right = 0;
    private ProximitySensorBrain rightBrain;
    private ProximitySensorBrain frontBrain;
    private ProximitySensorBrain leftBrain;
    public RangeValueUpdater(GameObject r, GameObject m, GameObject l)
    {
        rightBrain = r.GetComponent<ProximitySensorBrain>();
        frontBrain = m.GetComponent<ProximitySensorBrain>();
        leftBrain = l.GetComponent<ProximitySensorBrain>();
    }
    public void FixedUpdate()
    {
        left = leftBrain.sensorOutput;
        front = frontBrain.sensorOutput;
        right = rightBrain.sensorOutput;
    }
}

[MoonSharpUserData] 
public class ColorValueUpdater
{

    private ColorSensorBrain leftBrain;
    private ColorSensorBrain frontBrain;
    private ColorSensorBrain rightBrain;

    public LuaColor left;
    public LuaColor front;
    public LuaColor right;

    public ColorValueUpdater(GameObject r, GameObject m, GameObject l)
    {
        rightBrain = r.GetComponent<ColorSensorBrain>();
        frontBrain = m.GetComponent<ColorSensorBrain>();
        leftBrain = l.GetComponent<ColorSensorBrain>();
        left = new LuaColor(leftBrain.DetectedColor);
        front = new LuaColor(frontBrain.DetectedColor);
        right = new LuaColor(rightBrain.DetectedColor);

    }

    private void updateColorFromSensors()
    {
        left.setColor(leftBrain.DetectedColor);
        front.setColor(frontBrain.DetectedColor);
        right.setColor(rightBrain.DetectedColor);
    }
    public void FixedUpdate()
    {
        updateColorFromSensors();
    }
}

[MoonSharpUserData]
public class LuaColor
{
    public double red{ get { return (double)myColor.r; } }
    public double blue { get { return (double)myColor.b; } }
    public double green { get { return (double)myColor.g; } }
    public double h;
    public double s;
    public double v;

    public double y;

    private float  kr = 0.299f;
    private float  kg = 0.587f;
    private float  kb = 0.114f;
    private Color myColor;
    public LuaColor(Color c)
    {
        setColor(c);
    }

    public void setColor(Color c)
    {
        myColor = c;
        float hue, sat, val;
        Color.RGBToHSV(c, out hue, out sat, out val);
        h = (double)hue;
        s = (double)sat;
        v = (double)val;

        y = (kr * red) + (kg * green) + (kb + blue);
    }
}
[MoonSharpUserData]
public class RobotArmUpdater
{
    RobotArmBrain armBrain;
    public RobotArmUpdater(RobotArmBrain brain)
    {
        armBrain = brain;
    }

    public bool grab { 
        get { return armBrain.armActive;  }
        set { armBrain.armActive=value; }
    }
}
