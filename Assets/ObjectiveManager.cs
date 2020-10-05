using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectiveManager : MonoBehaviour
{
    public GameObject Robot;

    public GameObject RemoteControlTasksSubcategory;
    public GameObject DriveFor30Seconds;
    public GameObject GrabAnObject;
    public GameObject DragObjectInCircle;

    public GameObject MakeRobotDriveForward;
    public GameObject MakeRobotTurn;
    public GameObject MakeScriptReturnRange;
    public GameObject MakeScriptReturnColor;

    public GameObject AvoidAWall;
    public GameObject FollowALine;


    // Start is called before the first frame update
    void Start()
    {
 
       
    }

    void Update()
    {

    }
    // Update is called once per frame
    void FixedUpdate()
    {
        checkObjective(DriveFor30Seconds, CheckForDrive30Sec());
        checkObjective(GrabAnObject, CheckForGrabOBject());
        checkObjective(DragObjectInCircle, CheckForDragObjectToCenter());
        checkObjective(MakeRobotDriveForward, CheckForMakeRobotMoveForward());
        checkObjective(MakeRobotTurn, CheckForMakeRobotTurn());
        checkObjective(MakeScriptReturnColor, CheckForMakeScriptReturnColor());
        checkObjective(MakeScriptReturnRange, CheckForMakeScriptReturnRange());
        checkObjective(AvoidAWall,CheckForAvoidWall());
        checkObjective(FollowALine, CheckForFollowLine());
    }

    void checkObjective(GameObject ui, bool result)
    {
        if (result) {
            Text o = ui.transform.Find("ObjectiveResult").GetComponent<Text>();
            Text L = ui.transform.Find("ObjectiveLabel").GetComponent<Text>();
            o.text = "✓";
            o.color = new Color(0, 0.4f, 0, 1); ;
            L.color = new Color(0, 0.4f, 0, 1); ;
        }
    }

    float Drive30ElapsedTime = 0f;
    public float Drive30ElapsedTimeGoal = 30f;
    bool CheckForDrive30Sec()
    {
        if (Drive30ElapsedTime < Drive30ElapsedTimeGoal)
        {
            float vel = Robot.GetComponent<Rigidbody>().velocity.magnitude;
            if (vel > 0.3)
            {
                Drive30ElapsedTime += Time.deltaTime;
            }
            return false;
        } else
        {
            return true;
        }
    }

    float CheckForGrabElapsedTime = 0f;
    public float CheckForGrabElapsedTimeGoal = 3f;
    bool CheckForGrabOBject()
    {
        RobotArmBrain b = Robot.transform.Find("RobotArm").GetComponent<RobotArmBrain>();
        //Debug.Log(b);
        if (b.GrabbedBody != null)
        {
            if (CheckForGrabElapsedTime < CheckForGrabElapsedTimeGoal)
            {
                //Debug.Log("not yet " + CheckForGrabElapsedTime +  "  " + CheckForGrabElapsedTimeGoal);
                CheckForGrabElapsedTime += Time.deltaTime;
            } else
            {
                return true;
            }
        } else
        {
            CheckForGrabElapsedTime = 0f;
        }
        return false;

    }

    float CheckForDragObjectToCenterElapsedTime = 0f;
    public float CheckForDragObjectToCenterElapsedTimeGoal = 5f;
    public float CheckForDragObjectToCenterAcceptableDistance = 5f;
    bool CheckForDragObjectToCenter()
    {

        RobotArmBrain b = Robot.transform.Find("RobotArm").GetComponent<RobotArmBrain>();
        if (b.GrabbedBody != null)
        {
            float dist = Vector3.Distance(new Vector3(0, 0, 0),b.GrabbedBody.position);
            if (dist < CheckForDragObjectToCenterAcceptableDistance)
            {
                if (CheckForDragObjectToCenterElapsedTime< CheckForDragObjectToCenterElapsedTimeGoal)
                {
                    CheckForDragObjectToCenterElapsedTime += Time.deltaTime;
                } else
                {
                    return true;
                }
            }
        }
            // Grabbing an object AND in the center of the map
            return false;
    }

    float CheckForMakeRobotMoveForwardElapsedTime = 0f;
    public float CheckForMakeRobotMoveForwardElapsedTimeGoal = 5f;
    bool CheckForMakeRobotMoveForward()
    {
        RobotBrain b = Robot.GetComponent<RobotBrain>();
        if (b.motor.right!=0f && b.motor.left != 0f && b.autoMode)
        {
            if (CheckForMakeRobotMoveForwardElapsedTime < CheckForMakeRobotMoveForwardElapsedTimeGoal){
                CheckForMakeRobotMoveForwardElapsedTime += Time.deltaTime;
            } else
            {
                return true;
            }
        }
        return false;
    }

    float CheckForMakeRobotTurnElapsedTime = 0f;
    public float CheckForMakeRobotTurnElapsedTimeGoal = 3f;
    public float AcceptableAngularVelocityMagnitude = 0.1f;
    bool CheckForMakeRobotTurn()
    {
        RobotBrain b = Robot.GetComponent<RobotBrain>();
        Rigidbody o = Robot.GetComponent<Rigidbody>();
        // Are we autonomous and do we have an angular velocity
        if (b.autoMode && (o.angularVelocity.magnitude > AcceptableAngularVelocityMagnitude))
        {
            // if one of the motors is moving and they're not equal.
            // Are we responsable for this angular velocity?
            if ( (b.motor.right!=0 || b.motor.left!=0) && b.motor.right != b.motor.left )
            {
                // are we doing it for enough time?
                if (CheckForMakeRobotTurnElapsedTime < CheckForMakeRobotTurnElapsedTimeGoal)
                {
                    CheckForMakeRobotTurnElapsedTime += Time.deltaTime;
                }
                else
                {
                    return true;
                }
            }
        }
        return false;
    }

    bool CheckForMakeScriptReturnColor()
    {
        RobotBrain b = Robot.GetComponent<RobotBrain>();
        if (b.scriptReturn != null)
        {
            if (b.scriptReturn.ToObject() is LuaColor)
            {
                return true;
            }
        }
        return false;
    }

    bool CheckForMakeScriptReturnRange()
    {
        RobotBrain b = Robot.GetComponent<RobotBrain>();
        if (b.scriptReturn != null)
        {
            if (b.scriptReturn.ToObject() is double)
            {
                return true;
            }
        }
        return false;
    }




    public bool HelperTaggedItemInRange(float dist, string tag)
    {
        RaycastHit[] hits = Physics.SphereCastAll(
                                Robot.transform.position,
                                dist,
                                Robot.transform.up,
                                0f);
        //Debug.Log("Found Objects: "+ hits.Length);
        foreach (RaycastHit r in hits)
        {
            if (r.transform.tag == tag)
            {
                return true;
            }
        }
        return false;
    }

    public float RobotDetectLineDistance = 0.25f;
    float CheckForFollowLineTurnElapsedTime = 0f;
    public float CheckForFollowLineTurnElapsedTimeGoal = 10f;
    bool CheckForFollowLine()
    {
        RobotBrain b = Robot.GetComponent<RobotBrain>();
        if (b.autoMode)
        {
            if (HelperTaggedItemInRange(RobotDetectLineDistance,"Line"))
            {
                if (CheckForFollowLineTurnElapsedTime< CheckForFollowLineTurnElapsedTimeGoal)
                {
                    CheckForFollowLineTurnElapsedTime += Time.deltaTime;
                    
                } else
                {
                    return true;
                }
            }
        }
        return false;
    }

    bool wallInRange;
    public float CheckForWallDistance = 3f;
    bool CheckForAvoidWall()
    {
        // while code is running
        RobotBrain b = Robot.GetComponent<RobotBrain>();
        if (b.autoMode)
        {
            // Wait untill a wall is close enough
            if (HelperTaggedItemInRange(CheckForWallDistance, "Wall")) wallInRange = true;
            //now, wait untill it goes away
            if (wallInRange==true && !HelperTaggedItemInRange(CheckForWallDistance, "Wall"))
            {
                return true;
            }
        }

        return false;
    }
}
