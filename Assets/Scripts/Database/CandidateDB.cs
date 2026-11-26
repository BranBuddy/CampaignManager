using UnityEngine;
using CampaignManager.Database;
using System.Collections.Generic;
using UnityEngine.UI;

namespace CampaignManager.Database
{
    public class CandidateDB : IDatabase
    {
        public override string DatabaseName => databaseName;
        public override string PrimaryKey => primaryKey;

        public override List<ForeignKey> ForeignKeys => foreignKeys;

        [SerializeField] protected string databaseName = "Candidate";
        [SerializeField] protected string primaryKey = "CANDIDATE_ID";
        [SerializeField] protected List<ForeignKey> foreignKeys = new List<ForeignKey>
        {
            new ForeignKey { TableName = "Party", ColumnName = "PARTY_ID" },
            new ForeignKey { TableName = "State", ColumnName = "STATE_ID" },
            new ForeignKey { TableName = "Job", ColumnName = "JOB_ID" }
        };

        public List<CandidateDBColumns> entries;

         public override T GetData<T>(string primaryKey)
        {
            foreach (var entry in entries)
            {
                // Ensure both sides are compared as strings
                if (entry.CANDIDATE_ID.ToString().Equals(primaryKey))
                {
                    return (T)(object)entry;
                }
            }

            Debug.LogWarning($"No entry found for primary key: {primaryKey}");
            return default(T);
        }
    }

    [System.Serializable]
    public class CandidateDBColumns
    {
        public int CANDIDATE_ID;
        public string FIRST_NAME;
        public string LAST_NAME;
        public string PARTY_ID;
        public string STATE_ABBR;
        public string ROLE_ID;
        public Image CANDIDATE_IMAGE;
    }
}