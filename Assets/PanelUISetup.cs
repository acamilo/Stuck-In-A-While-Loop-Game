using DynamicPanels;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelUISetup : MonoBehaviour
{
    // Start is called before the first frame update
    public DynamicPanelsCanvas canvas;
    void Start()
    {
        canvas = this.GetComponent<DynamicPanelsCanvas>();

        // Update is called once per frame
        void Update()
        {

        }
    }
}