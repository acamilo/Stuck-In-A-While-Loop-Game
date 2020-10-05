using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOptionsScript : MonoBehaviour
{
    public GameObject robot;
    private RobotBrain robotBrain;
    public Toggle toggle;
    public Button ResetRobotPositionButton;
    public Button ExitApplicationButton;

    public RawImage DirectControlImage;
    public RawImage AutonomousControlImage;

    Vector3 InitialPosition;
    Quaternion InitialRotation;
    // Start is called before the first frame update
    void Start()
    {
        robotBrain = robot.GetComponent<RobotBrain>();
        ResetRobotPositionButton.onClick.AddListener(resetRobotPosition);
        ExitApplicationButton.onClick.AddListener(QuitGame);
        InitialPosition = robot.transform.position;
        InitialRotation = robot.transform.rotation;
        
    }

    // Update is called once per frame
    void Update()
    {
        robotBrain.autoMode = !toggle.isOn;
        if (robotBrain.autoMode)
        {
            AutonomousControlImage.enabled = true;
            DirectControlImage.enabled = false;
        } else
        {
            AutonomousControlImage.enabled = false;
            DirectControlImage.enabled = true;
        }

    }

    void resetRobotPosition()
    {
        robot.transform.position =InitialPosition;
        robot.transform.rotation = InitialRotation;
    }

    void QuitGame()
    {
        Application.Quit();
    }
}
