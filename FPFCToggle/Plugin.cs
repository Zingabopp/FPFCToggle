using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using IPA;
using IPA.Config;
using IPA.Config.Stores;
using UnityEngine.SceneManagement;
using UnityEngine;
using IPALogger = IPA.Logging.Logger;

namespace FPFCToggle
{

    [Plugin(RuntimeOptions.SingleStartInit)]
    public class Plugin
    {
        internal static Plugin instance { get; private set; }
        internal static string Name => "FPFCToggle";
        internal static PluginConfig config;
        internal static IPALogger log;

        [Init]
        /// <summary>
        /// Called when the plugin is first loaded by IPA (either when the game starts or when the plugin is enabled if it starts disabled).
        /// [Init] methods that use a Constructor or called before regular methods like InitWithConfig.
        /// Only use [Init] with one Constructor.
        /// </summary>
        public void Init(IPALogger logger)
        {
            instance = this;
            log = logger;
            log.Debug("Logger initialized.");
        }

        #region BSIPA Config
        [Init]
        public void InitWithConfig(Config conf)
        {
            config = conf.Generated<PluginConfig>();
            log.Debug("Config loaded");
        }
        #endregion

        [OnStart]
        public void OnApplicationStart()
        {
            if(FPFCController.instance == null)
                new GameObject("FPFCToggle_Controller").AddComponent<FPFCController>();
        }

        [OnExit]
        public void OnApplicationQuit()
        {
        }
    }
}
