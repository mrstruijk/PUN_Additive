using System;
using System.Collections.Generic;
using UnityEngine;

public class SendActionIfNotOnPlatform : MonoBehaviour
{
    [SerializeField] private List<RuntimePlatform> doNotSendActionFromPlatform;

    public static Action DoOnOtherPlatform = delegate { };


    private void Start()
    {
        if (!doNotSendActionFromPlatform.Contains(Application.platform))
        {
            DoOnOtherPlatform();
        }
    }
}
