using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIManager : MonoBehaviour
{
    public DialogueObj dialogueObj;
    Button[] buttons;
    Text mainText;
    // Start is called before the first frame update
    void Start()
    {
        buttons = GetComponentsInChildren<Button>();
        mainText = GetComponentInChildren<Text>();

        SetGUI(dialogueObj.dialogues2[0]);
    }

    private void SetGUI(Dialogue dialogue)
    {
        mainText.text = dialogue.mainText;
        for (int i = 0; i < buttons.Length; i++)
        {
            if (i < dialogue.options.Count)
            {
                buttons[i].gameObject.SetActive(true);
                buttons[i].GetComponentInChildren<Text>().text = dialogue.options[i].text;
                string value = dialogue.options[i].dialogueToJumpTo;
                buttons[i].onClick.RemoveAllListeners();
                string debugValue = "";

                if (dialogue.options[i].arg != null)
                {
                    if (dialogue.options[i].arg.print != null && dialogue.options[i].arg.print != "")
                    {
                        debugValue = (dialogue.options[i].arg.print);
                    }
                }

                buttons[i].onClick.AddListener(() => OnButtonClick(value, debugValue));

            }

            if (i >= dialogue.options.Count)
            {
                buttons[i].gameObject.SetActive(false);
            }


        }
        Button btn = buttons[buttons.Length - 1];
        btn.gameObject.SetActive(true);
        btn.GetComponentInChildren<Text>().text = "Exit";
        string value2 = "";
        btn.onClick.RemoveAllListeners();
        btn.onClick.AddListener(() => OnButtonClick(value2, ""));
    }

    public void OnButtonClick(string dialogueToJumpTo, string debugValue)
    {
        if (debugValue != "")
        {
            Debug.Log(debugValue);
        }

        if (dialogueToJumpTo != null && dialogueToJumpTo != "")
        {
            foreach (var item in dialogueObj.dialogues2)
            {
                if (item.title == dialogueToJumpTo)
                {
                    Dialogue dg = item;
                    SetGUI(dg);
                }
            }
        }
        else
        {
            this.gameObject.SetActive(false);
        }
    }
}
