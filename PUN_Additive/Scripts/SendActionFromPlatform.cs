using System;
using System.Collections.Generic;
using UnityEngine;

public class SendActionFromPlatform : MonoBehaviour
{
    [SerializeField] private List<RuntimePlatform> sendActionFromPlatform;

    private static readonly Action DoOnPlatform = delegate { };

    private void Start()
    {
        if (sendActionFromPlatform.Contains(Application.platform))
        {
            DoOnPlatform();
        }
    }
}
