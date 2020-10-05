using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DynamicPanels;

public class MenuController : MonoBehaviour
{

    public DynamicPanelsCanvas canvas;
    public RectTransform content1, content2;

    // Start is called before the first frame update
    void Start()
    {
        Panel panel1 = PanelUtils.CreatePanelFor(content1, canvas);
        panel1.AddTab(content2);
        panel1.DockToRoot(Direction.Left);
        canvas.ForceRebuildLayoutImmediate();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
