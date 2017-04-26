using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class AddButton : MonoBehaviour {
    
    // to get gameobject from scene ; can use this or use getcomponent
    [SerializeField]
    private Transform puzzleField;

    [SerializeField]
    private GameObject btn;
    
    void Awake()
    {
        for (int i = 0; i < 6; i++)
        {
            GameObject button = Instantiate(btn);
            button.name = "" + i;
            button.transform.SetParent(puzzleField, false);
        }
    }
}
