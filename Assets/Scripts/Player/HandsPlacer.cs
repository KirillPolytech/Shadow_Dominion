using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

namespace HellBeavers
{
    public class HandsPlacer : MonoBehaviour
    {
        [SerializeField] private Transform aim;

        [Space] [Header("LHand")] [SerializeField]
        private Transform LArmPos;

        [SerializeField] private BoneController LArm;

        [SerializeField] private Transform LForePos;
        [SerializeField] private BoneController LForeArm;

        [SerializeField] private Transform LHandPos;
        [SerializeField] private BoneController LHand;

        [Space] [Header("LHand")] [SerializeField]
        private Transform RArmPos;

        [SerializeField] private BoneController RArm;

        [SerializeField] private Transform RForePos;
        [SerializeField] private BoneController RForeArm;

        [SerializeField] private Transform RHandPos;
        [SerializeField] private BoneController RHand;

        private List<BoneController> bones = new List<BoneController>();

        [Button]
        public void EnableFabrics()
        {
            List<BoneController> bones = new List<BoneController>
            {
                LArm,
                LForeArm,
                LHand,
                RArm,
                RForeArm,
                RHand
            };

            foreach (var bone in bones)
            {
                bone.BoneSettings.SetDrive(
                    0,
                    0,
                    0,
                    false,
                    1500,
                    1500,
                    150,
                    false);
            }
        }

        private void Awake()
        {
            bones = new List<BoneController>
            {
                LArm,
                LForeArm,
                LHand,
                RArm,
                RForeArm,
                RHand
            };

            RArmPos.position = RArm.CurrentPosition;
            RForePos.position = RForeArm.CurrentPosition;

            RArmPos.rotation = RArm.CurrentRotation;
            RForePos.rotation = RForeArm.CurrentRotation;
        }

        private void FixedUpdate()
        {
            return;
            RArm.transform.position = RArmPos.position;
            RForeArm.transform.position = RForePos.position;
            RHand.transform.position = RHandPos.position;

            RArm.transform.rotation = RArmPos.rotation;
            RForeArm.transform.rotation = RForePos.rotation;
            RHand.transform.rotation = RHandPos.rotation;
        }
    }
}