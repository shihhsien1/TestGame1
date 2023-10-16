using System.Collections.Generic;
using Unity.VisualScripting;

public class UIManager
{
    private Dictionary<string, BasePanel> panelDic = new Dictionary<string, BasePanel>();

    public void Regist(string key, BasePanel panel)
    {
        if (panelDic.ContainsKey(key) == true) return;
        panelDic.Add(key, panel);
    }

    public void UnRegist(string key)
    {
        if (panelDic.ContainsKey(key) == false) return;
        panelDic.Remove(key);
    }

    public T GetPanel<T>() where T : BasePanel
    {
        var key = typeof(T).Name;
        if(panelDic.ContainsKey(key) == false) return null;
        return panelDic[key] as T;
    }

}
