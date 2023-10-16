using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ElementCard : MonoBehaviour
{
    private Button btn = null;
    [SerializeField] private Text textTitle = null;
    [SerializeField] private Text textDescription = null;
    [SerializeField] private Text textPrice = null;
    public int Price { get; private set; } = 0;
    public int Id { get; private set; } = 0;
    public bool IsRare { get; private set; } = false;
    public void Initailize()
    {
        btn = GetComponent<Button>();
    }

    public void OnClick(Action callback)
    {
        btn.onClick.RemoveAllListeners();
        btn.onClick.AddListener(() => callback?.Invoke());
    }

    public void UpdateUI(string title, string content, int price, int id, bool isRare)
    {
        textTitle.text = title;
        textDescription.text = content;
        textPrice.text = price.ToString();
        Price = price;
        Id = id;
        IsRare = isRare;
    }

}
