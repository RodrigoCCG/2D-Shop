using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TextOnlyScriptable", menuName = "2D-Shop/TextOnlyScriptable", order = 0)]
public class TextOnlyScriptable : ScriptableObject {
    //Base scriptable object for objects that only display text
    public List<string> textList;
}
