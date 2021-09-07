using NUnit.Framework;
using UnityEngine;


// ReSharper disable ClassNeverInstantiated.Global

public class EditPUNTests
{
    public class Network
    {
       // private IConnectToNetworkHost networkHoster;
        private PhotonConnectionSettingsSO settings;


        [SetUp] public void PUNSetup()
        {
            settings = ScriptableObject.CreateInstance<PhotonConnectionSettingsSO>();
            //networkHoster = settings;
        }


        [Test] public void AutoSyncIsEnabledTest()
        {
            Assert.AreEqual(true, PhotonConnectionSettingsSO.AutoSyncScenes);
        }


        [TearDown] public void PUNTearDown()
        {
            // networkHoster = null;
            settings = null;
        }
    }


    public class TamperWith
    {


    }
}
