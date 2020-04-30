using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class DialogueTool : EditorWindow
{
    //  string myString = "Hello, World!";
    Color color;
    string firstKey = null;
    public UnityEngine.Object source;
    [SerializeField]
    Dictionary<string, Dialogue> dialogues;
    List<Dialogue> dialoguesList;

    [MenuItem("Dialogue Tool/Example")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow<DialogueTool>("Colorizer");
    }

    private void OnGUI()
    {
      //  EditorGUILayout.LabelField("Color the selected obj", EditorStyles.boldLabel);

        
        if (GUILayout.Button("OpenFIle"))
        {
            string path = EditorUtility.OpenFilePanel("Load text", "", "txt");
            string fileName = (path.Substring(path.LastIndexOf("/") + 1, path.LastIndexOf(".") - path.LastIndexOf("/") - 1));
            if (path.Length != 0)
            {
                firstKey = null;
                var fileContent = File.ReadAllLines(path);
                for (int i = 0; i < fileContent.Length; i++)
                {
                    fileContent[i] = fileContent[i].Trim();
                }

                 dialogues = new Dictionary<string, Dialogue>();
                dialogues = ProcessTextToDialogue(fileContent);

                DialogueObj dialogueObj = (DialogueObj)ScriptableObject.CreateInstance(typeof(DialogueObj));
                dialogueObj.dialogues = dialogues;
                dialogueObj.name = fileName;
                dialogueObj.firstKey = firstKey;
                dialogueObj.dialogues2 = dialoguesList;
                //AssetDatabase.CreateAsset(dialogueObj, Application.dataPath + "/" + "Dialogue/" + dialogueObj.name + ".asset");
                AssetDatabase.CreateAsset(dialogueObj, "Assets/" + fileName + ".asset");
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();

                
            }
        }
        //EditorGUILayout.ObjectField(source, typeof(UnityEngine.Object), true);
     //   EditorGUILayout.ObjectField(source, typeof(DialogueObj), true);
    }

    private Dictionary<string, Dialogue> ProcessTextToDialogue(string[] fileContent)
    {
        Dictionary<string, Dialogue> dialogues = new Dictionary<string, Dialogue>();
        dialoguesList = new List<Dialogue>();

        Dialogue d = new Dialogue();
        Options opt = new Options();
        Argument arg = new Argument();

        for (int i = 0; i < fileContent.Length; i++)
        {
            if (fileContent[i] != String.Empty)
            {
                switch (fileContent[i][0])
                {
                    case '#':
                        d = new Dialogue();
                        d.options = new List<Options>();
                        // dTitle = fileContent[i].Substring(1, fileContent[i].Length - 1);
                        d.title = fileContent[i].Substring(1, fileContent[i].Length - 1);
                        if (firstKey == null)
                        {
                            firstKey = d.title;
                        }
                        i = i + 1;
                        d.mainText = fileContent[i];
                        // TODO multiline
                        // checks next line if it is pure text -> if it is, add it 
                        //while (fileContent[i + 1][0] != '#' && fileContent[i + 1][0] != '-' && fileContent[i + 1][0] != ':' && fileContent[i + 1] != String.Empty)
                        //{
                        //    i = i + 1;
                        //    d.mainText = d.mainText + Environment.NewLine + fileContent[i];
                        //}

                        break;
                    case '-':
                        opt = new Options();
                        bool hasArgument = fileContent[i].Contains("{");
                        if (hasArgument)
                        {
                            // Debug.Log("Argument");
                            int firstBracketPosMain = fileContent[i].IndexOf("{");
                            int lastBracketPosMain = fileContent[i].IndexOf("}");
                            int firstBracketPosSecondary = fileContent[i].IndexOf("(");
                            int lastBracketPosSecondary = fileContent[i].IndexOf(")");
                            opt.text = fileContent[i].Substring(1, firstBracketPosMain - 1);

                            string commandType = fileContent[i].Substring(firstBracketPosMain + 1, firstBracketPosSecondary - firstBracketPosMain - 1);
                            //Debug.Log(firstBracketPosSecondary);
                            //Debug.Log(commandType);
                            switch (commandType)
                            {
                                case "print":
                                    arg = new Argument();
                                   // Print p = new Print();
                                    //p.print = fileContent[i].Substring(firstBracketPosSecondary + 1, lastBracketPosSecondary - firstBracketPosSecondary - 1);
                                    arg.print = fileContent[i].Substring(firstBracketPosSecondary + 1, lastBracketPosSecondary - firstBracketPosSecondary - 1);
                                    opt.arg = arg;
                                    //opt.arg = p;
                                    Debug.Log(opt.arg.print);
                                    //Debug.Log(p.print);
                                    break;
                                default:
                                    break;
                            }
                        }
                        else
                        {
                            opt.text = fileContent[i].Substring(1, fileContent[i].Length - 1);
                        }

                        if (!String.IsNullOrEmpty(fileContent[i + 1]) && fileContent[i + 1] == ":")
                        {
                            opt.dialogueToJumpTo = fileContent[i + 1].Substring(1, fileContent[i].Length - 1);
                        }
                        if (fileContent[i + 1][0] != ':' && fileContent[i + 1] != String.Empty)
                        {
                            d.options.Add(opt);
                        }
                        break;
                    case ':':
                        opt.dialogueToJumpTo = fileContent[i].Substring(1);
                        d.options.Add(opt);
                        break;
                    case '$':
                        dialogues.Add(d.title, d);
                        dialoguesList.Add(d);
                        break;
                    default:
                        break;
                }
            }

        }

        return dialogues;
    }

    private void Colorize()
    {
        foreach (var obj in Selection.gameObjects)
        {
            Renderer r = obj.GetComponent<Renderer>();
            if (r != null)
            {
                r.sharedMaterial.color = color;
            }
        }
    }
}
