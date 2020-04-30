using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Dialogue", menuName = "Dialogue")]
public class DialogueObj : ScriptableObject
{
    public Dictionary<string, Dialogue> dialogues;
    public List<Dialogue> dialogues2;
    public string firstKey;

    
}
