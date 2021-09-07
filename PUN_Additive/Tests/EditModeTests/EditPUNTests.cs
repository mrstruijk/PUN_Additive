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
        [Test] public void ScaleChangeOnTamper()
        {
            var gameObject = new GameObject();
            var originalScale = gameObject.transform.localScale;
            var tampering = new Tampering(gameObject.transform);
            var newScale = new Vector3(3, 3, 3);
            tampering.TamperScale(newScale);

            Assert.AreNotEqual(originalScale, gameObject.transform.localScale);
            Assert.AreEqual(newScale, gameObject.transform.localScale);
        }


        [Test] public void PositionChangeOnTamper()
        {
            var gameObject = new GameObject();
            var originalPosition = gameObject.transform.position;
            var tampering = new Tampering(gameObject.transform);
            var newPosition = new Vector3(1, 2, 3);
            tampering.TamperPosition(newPosition);

            Assert.AreNotEqual(originalPosition, gameObject.transform.position);
            Assert.AreEqual(newPosition, gameObject.transform.position);
        }

    }
}
