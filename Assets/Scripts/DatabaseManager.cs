using UnityEngine;
using CampaignManager.Singleton;
using System.Collections.Generic;

namespace CampaignManager.Database
{
    public class DatabaseManager : Singleton<DatabaseManager>
    {
        public List<GameObject> databases = new List<GameObject>();
        public CandidateDB candidateDB;
        public PartyDB partyDB;
        public StateDB stateDB;
        public RoleDB roleDB;  
        protected override void Awake()
        {
            base.Awake();
        } 

        private void Start()
        {
            candidateDB = FetchDatabase("Candidate").GetComponent<CandidateDB>();
            partyDB = FetchDatabase("Party").GetComponent<PartyDB>();
            stateDB = FetchDatabase("State").GetComponent<StateDB>();
            roleDB = FetchDatabase("Role").GetComponent<RoleDB>();

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
