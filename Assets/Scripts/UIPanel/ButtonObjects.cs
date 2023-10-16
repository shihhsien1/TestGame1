using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonObjects : MonoBehaviour
{
    public Button Button { get; set; } = null;
    public Text Text { get; set; } = null;
    private void Awake()
    {
        Button = GetComponent<Button>();
        Text = GetComponentInChildren<Text>();
    }
}
