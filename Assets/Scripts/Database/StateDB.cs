using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace CampaignManager.Database
{
    public class StateDB : IDatabase
    {
        public override string DatabaseName => databaseName;
        public override string PrimaryKey => primaryKey;

        public override List<ForeignKey> ForeignKeys => foreignKeys;

        [SerializeField] protected string databaseName = "State";
        [SerializeField] protected string primaryKey = "STATE_ID";
        [SerializeField] protected List<ForeignKey> foreignKeys = new List<ForeignKey>
        {

        };

        public List<StateDBColumns> entries;

        public override T GetData<T>(string primaryKey)
        {
            foreach (var entry in entries)
            {
                // Ensure both sides are compared as strings
                if (entry.STATE_ABBR.ToString().Equals(primaryKey))
                {
                    return (T)(object)entry;
                }
            }

            Debug.LogWarning($"No entry found for primary key: {primaryKey}");
            return default(T);
        }
    }

    [System.Serializable]
    public class StateDBColumns
    {
        public string STATE_ABBR;
        public string STATE_NAME;
    }
}
