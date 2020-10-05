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
    // Start is called before the first frame update
    void Start()
    {
        robotBrain = robot.GetComponent<RobotBrain>();
        ResetRobotPositionButton.onClick.AddListener(resetRobotPosition);
        ExitApplicationButton.onClick.AddListener(QuitGame);
        
    }

    // Update is called once per frame
    void Update()
    {
        robotBrain.autoMode = toggle.isOn;

    }

    void resetRobotPosition()
    {
        robot.transform.position = new Vector3(0,0.1f,0);
        robot.transform.rotation = Quaternion.EulerAngles(new Vector3(0, 0, 0));
    }

    void QuitGame()
    {
        Application.Quit();
    }
}
