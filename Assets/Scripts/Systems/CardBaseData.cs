using System.Collections.Generic;
using UnityEngine;

public class CardBaseData
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public List<Condition> Conditions { get; set; } = new List<Condition>();

    public struct Condition
    {
        public ConditionType conditionType;
        public int conditionValue;
        public Condition(ConditionType conditionType, int conditionValue)
        {
            this.conditionType = conditionType;
            this.conditionValue = conditionValue;
        }
    }
}