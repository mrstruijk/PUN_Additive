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


        [Test] public void AutoSyncIsDisabled()
        {
            Assert.AreEqual(false, settings.AutoSyncScenes);
        }


        [TearDown] public void PUNTearDown()
        {
            settings = null;
        }
    }
}
