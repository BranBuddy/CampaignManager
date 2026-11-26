using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace CampaignManager.Database
{
    public class RoleDB : IDatabase
    {
        public override string DatabaseName => databaseName;
        public override string PrimaryKey => primaryKey;

        public override List<ForeignKey> ForeignKeys => foreignKeys;


        [SerializeField] protected string databaseName = "Role";
        [SerializeField] protected string primaryKey = "ROLE_ID";
        [SerializeField] protected List<ForeignKey> foreignKeys = new List<ForeignKey>
        {

        };

        public List<RoleDBColumns> entries;

        public override T GetData<T>(string primaryKey)
        {
            foreach (var entry in entries)
            {
                // Ensure both sides are compared as strings
                if (entry.ROLE_ID.ToString().Equals(primaryKey))
                {
                    return (T)(object)entry;
                }
            }

            Debug.LogWarning($"No entry found for primary key: {primaryKey}");
            return default(T);
        }
    }

    [System.Serializable]
    public class RoleDBColumns
    {
        public string ROLE_ID;
        public string ROLE_NAME;
    }
}
