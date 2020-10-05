using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using SFB;

public class CodePanelManager : MonoBehaviour
{
    // Start is called before the first frame update
    public Button SaveButton;
    public Button LoadButton;
    public InputField CodeEditor;

    void Start()
    {
        SaveButton.onClick.AddListener(SaveCode);
        LoadButton.onClick.AddListener(LoadCode);
        Debug.Log(Application.persistentDataPath);
    }

    private void SaveCode()
    {
        string path = StandaloneFileBrowser.SaveFilePanel("Save File", "", "", "");
        if (path != "")
        {
            StreamWriter writer = new StreamWriter(path, true);
            writer.Write(CodeEditor.text);
            writer.Close();
        }

    }

    private void LoadCode()
    {
        string[] paths = StandaloneFileBrowser.OpenFilePanel("Open File", "", "", false);
        if (paths.Length > 0)
        {
            StreamReader reader = new StreamReader(paths[0]);
            string codefile = reader.ReadToEnd();
            CodeEditor.text = codefile;
            Debug.Log(codefile);
            reader.Close(); 
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
