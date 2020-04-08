using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using IPA.Utilities;

namespace FPFCToggle
{
    /// <summary>
    /// Monobehaviours (scripts) are added to GameObjects.
    /// For a full list of Messages a Monobehaviour can receive from the game, see https://docs.unity3d.com/ScriptReference/MonoBehaviour.html.
    /// </summary>
	public class FPFCController : MonoBehaviour
    {
        public static FPFCController instance;
        private bool keyHeld = false;
        private VRCenterAdjust _vrCenter;
        public VRCenterAdjust VRCenter
        {
            get
            {
                if (_vrCenter == null)
                    _vrCenter = UnityEngine.Object.FindObjectOfType<VRCenterAdjust>();
                return _vrCenter;
            }
        }

        private Vector3SO _roomCenter;
        private Vector3SO RoomCenter
        {
            get
            {
                if (_roomCenter == null && VRCenter != null)
                    _roomCenter = VRCenter.GetField<Vector3SO, VRCenterAdjust>("_roomCenter");
                return _roomCenter;
            }
        }

        private FloatSO _roomRotation;
        private FloatSO RoomRotation
        {
            get
            {
                if (_roomRotation == null && VRCenter != null)
                    _roomRotation = VRCenter.GetField<FloatSO, VRCenterAdjust>("_roomRotation");
                return _roomRotation;
            }
        }

        private Vector3 LastPos = Vector3.zero;
        private Vector3 LastRot = Vector3.zero;
        #region Monobehaviour Messages
        /// <summary>
        /// Only ever called once, mainly used to initialize variables.
        /// </summary>
        private void Awake()
        {
            if(instance != null)
            {
                DestroyImmediate(this);
            }
            instance = this;
            DontDestroyOnLoad(this);
            Plugin.log.Debug($"FPFCController awake.");
        }

        FirstPersonFlyingController firstPersonFlyingController;
        public FirstPersonFlyingController GetFlyingController()
        {
            if (firstPersonFlyingController == null)
                firstPersonFlyingController = Resources.FindObjectsOfTypeAll<FirstPersonFlyingController>().FirstOrDefault();
            return firstPersonFlyingController;
        }
        /// <summary>
        /// Called every frame if the script is enabled.
        /// </summary>
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                //Plugin.log.Debug("F detected.");
                if (!keyHeld)
                {
                    var flyingController = GetFlyingController();
                    if (flyingController != null)
                    {
                        if (flyingController.enabled)
                        {
                            flyingController.enabled = false;
                            //Plugin.log.Debug("Flying controller disabled.");
                            if (VRCenter != null)
                            {
                                //Logger.log.Info($"Current Position:\n{TransformValuesString(VRCenter.gameObject.transform.position, VRCenter.gameObject.transform.rotation.eulerAngles)}");
                                VRCenter.ResetRoom();
                                if (RoomCenter != null && RoomRotation != null)
                                {
                                    RoomCenter.value = LastPos;
                                    RoomRotation.value = LastRot.y;
                                    //Logger.log.Info($"Setting RoomCenter to LastPos:\n{TransformValuesString(LastPos, LastRot)}");
                                }
                                //Logger.log.Info($"Switch back to old pos:\n{TransformValuesString(VRCenter.gameObject.transform.position, VRCenter.gameObject.transform.rotation.eulerAngles)}");
                            }
                        }
                        else
                        {
                            if (VRCenter != null)
                            {
                                LastPos = VRCenter.gameObject.transform.position;
                                LastRot = VRCenter.gameObject.transform.rotation.eulerAngles;
                                //Logger.log.Info($"Saving position:\n{TransformValuesString(LastPos, LastRot)}");
                            }
                            flyingController.enabled = true;
                            //Plugin.log.Debug("Flying controller enabled.");
                        }
                    }
                    else
                        Plugin.log.Debug("FirstPersonFlyingController not found.");
                    keyHeld = true;
                }
            }
            else
            {
                keyHeld = false;
            }
        }

        /// <summary>
        /// Called when the script is being destroyed.
        /// </summary>
        private void OnDestroy()
        {
            instance = null;
        }
        #endregion
    }
}
