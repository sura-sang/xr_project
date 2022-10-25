using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuraSang
{
    public class Global
    {
        private Global() 
        {
            _resourceManager = new ResourceManager();
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

    }
}
