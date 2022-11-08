using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuraSang
{
    public class Global
    {
        private const string UIManagerPath = "UIManager";

        private Global()
        {
            _resourceManager = new ResourceManager();
            _soDataManager = new SODataManager();

            _uiManager = GameObject.Instantiate(Resources.Load<GameObject>(UIManagerPath)).GetComponent<UIManager>();
        }

        private static Global _instance = null;
        public static Global Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new Global();
                }
                return _instance;
            }
        }

        public void SetCurrentSceneMaster(SceneMaster sceneMaster)
        {
            _sceneMaster = sceneMaster;
        }

        public SceneMaster SceneMaster => _sceneMaster;
        private SceneMaster _sceneMaster;

        public ResourceManager ResourceManager => _resourceManager;
        private ResourceManager _resourceManager;

        public SODataManager SODataManager => _soDataManager;
        private SODataManager _soDataManager;


        public UIManager UIManager => _uiManager;
        private UIManager _uiManager;
    }
}
