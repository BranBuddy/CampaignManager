using UnityEngine;
using CampaignManager.Singleton;
using System.Collections.Generic;

namespace CampaignManager.Database
{
    public class DatabaseManager : Singleton<DatabaseManager>
    {
        public List<GameObject> databases = new List<GameObject>();

        protected override void Awake()
        {
            base.Awake();
        } 

        public GameObject FetchDatabase(string database)
        {
            GameObject db = null;

            foreach (var dbObj in databases)
            {
                if (dbObj.GetComponent<IDatabase>().DatabaseName == database)
                {
                    db = dbObj;
                    break;
                }
            }
            return db;
        }
    }
}
