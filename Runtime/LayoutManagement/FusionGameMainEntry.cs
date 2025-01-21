using System;
using FusionGame.Core.Data;
using UnityEngine;

namespace FusionGame.Core.LayoutManagement
{
    internal class FusionGameMainEntry : MonoBehaviour
    {
        private IClientStartUp clientStartUp;
        private IPlayerTerminalData playerTerminalData;

        private void Awake()
        {
            clientStartUp = GetComponentInChildren<IClientStartUp>();
            if (clientStartUp == null)
            {
                throw new Exception("No IClientStartUp component found in children of FusionGameMainEntry");
            }
        }

        private void Start()
        {
            if (clientStartUp != null)
            {
                clientStartUp.ResetGameLayoutOnStartUp(playerTerminalData);
            }
        }
    }
}
