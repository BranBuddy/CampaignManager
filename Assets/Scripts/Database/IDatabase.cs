using System.Collections.Generic;
using UnityEngine;

namespace CampaignManager.Database
{
    [System.Serializable]
    public abstract class IDatabase : MonoBehaviour
    {
        public virtual string DatabaseName { get; }
        public virtual string PrimaryKey { get; }
        public virtual List<ForeignKey> ForeignKeys { get; }

        public virtual T GetData<T>(string primaryKey)
        {
            return default(T);
        }
    }

    [System.Serializable]
    public class ForeignKey
    {
        public string TableName;
        public string ColumnName;
    }
}
