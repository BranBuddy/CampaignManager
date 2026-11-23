using UnityEngine;
using UnityEngine.SceneManagement;
using CampaignManager.Singleton;

namespace CampaignManager.SceneLoad
{   

    public class SceneLoadManager : Singleton<SceneLoadManager>
    {
        protected override void Awake()
        {
            base.Awake();
        }

        public void LoadScene(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}
