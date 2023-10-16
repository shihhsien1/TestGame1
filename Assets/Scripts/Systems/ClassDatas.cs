using System;
using System.Collections.Generic;
using UnityEngine.Tilemaps;

[Serializable]
public class LoadDeckData
{
    public string deckName = string.Empty;
}

namespace Tables
{
    public class BaseData
    {
        public int id = 0;
    }
    public class BaseTable<T, D> where T : class where D : BaseData
    {
        private List<D> tableDatas = null;
        private Dictionary<int, D> tableDic = new Dictionary<int, D>();
        public BaseTable()
        {
            tableDatas = DataHelper.JsonDeserializeData<D>(typeof(T).Name);
            if (tableDatas != null)
            {
                foreach (var data in tableDatas)
                {
                    tableDic.Add(data.id, data);
                }
            }
        }
        public D GetData(int id)
        {
            return tableDic[id];
        }
    }

    [Serializable]
    public class MessageData : BaseData
    {
        public string message = string.Empty;
    }
    public class MessageTable : BaseTable<MessageTable, MessageData>
    {
        public string GetMessageParameter(int id, params object[] args)
        {
            var msg = GetData(id).message;
            msg = string.Format(msg, args);
            return msg;
        }
    }
}
