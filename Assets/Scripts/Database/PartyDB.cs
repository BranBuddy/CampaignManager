using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace CampaignManager.Database
{
    public class PartyDB : IDatabase
    {
        public override string DatabaseName => databaseName;
        public override string PrimaryKey => primaryKey;

        public override List<ForeignKey> ForeignKeys => foreignKeys;

        [SerializeField] protected string databaseName = "Party";
        [SerializeField] protected string primaryKey = "PARTY_ID";
        [SerializeField] protected List<ForeignKey> foreignKeys = new List<ForeignKey>
        {

        };

        public List<PartyDBColumns> entries;

        public override T GetData<T>(string primaryKey)
        {
            foreach (var entry in entries)
            {
                // Ensure both sides are compared as strings
                if (entry.PARTY_ID.ToString().Equals(primaryKey))
                {
                    return (T)(object)entry;
                }
            }

            Debug.LogWarning($"No entry found for primary key: {primaryKey}");
            return default(T);
        }
    }

    [System.Serializable]
    public class PartyDBColumns
    {
        public int PARTY_ID;
        public string PARTY_NAME;
        public string PARTY_DESCRIPTION;
        public Image PARTY_IMAGE;
        public Color PARTY_COLOR;
    }
}
