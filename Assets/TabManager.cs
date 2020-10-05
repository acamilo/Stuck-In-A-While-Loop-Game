using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabManager : MonoBehaviour
{
    // Start is called before the first frame update
    public Button CodeButton;
    public Button RobotRefButton;
    public Button LuaRefButton;

    public GameObject CodePanel;
    public GameObject RobotRefPanel;
    public GameObject LuaRefPanel;
    void Start()
    {
        CodeButton.onClick.AddListener(switchToCode);
        RobotRefButton.onClick.AddListener(switchToRobot);
        LuaRefButton.onClick.AddListener(switchToLua);
        switchToLua();
    }

    void switchToCode()
    {
        CodePanel.SetActive(true);
        RobotRefPanel.SetActive(false);
        LuaRefPanel.SetActive(false);
    }

    void switchToRobot()
    {
        CodePanel.SetActive(false);
        RobotRefPanel.SetActive(true);
        LuaRefPanel.SetActive(false);
    }

    void switchToLua()
    {
        CodePanel.SetActive(false);
        RobotRefPanel.SetActive(false);
        LuaRefPanel.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
