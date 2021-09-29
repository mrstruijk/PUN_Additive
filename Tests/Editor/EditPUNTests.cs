using NUnit.Framework;
using UnityEngine;


public class EditPUNTests
{
    public class Network
    {
        private PhotonConnectionSettingsSO settings;


        [SetUp] public void PUNSetup()
        {
            settings = ScriptableObject.CreateInstance<PhotonConnectionSettingsSO>();
        }


        [Test] public void AutoSyncIsEnabledTest()
        {
            Assert.AreEqual(true, settings.AutoSyncScenes);
        }


        [TearDown] public void PUNTearDown()
        {
            settings = null;
        }
    }
}
