using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardButton : MonoBehaviour
{
    public int itemId = 0;
    public bool isRare = false;
    public Button button = null;
    public Text title = null;
    public Text description = null;

    public void UpdateData(int itemId, bool isRare)
    {
        this.itemId = itemId;
        this.isRare = isRare; 
    }

    public void Clear()
    {
        title.text = string.Empty;
        description.text = string.Empty;
        itemId = 0;
        isRare = false;
        button.onClick.RemoveAllListeners();
        gameObject.SetActive(false);
    }
}
