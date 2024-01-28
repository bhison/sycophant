using System;
using UnityEngine;

namespace DefaultNamespace
{
    [RequireComponent(typeof(TMPro.TextMeshPro))]
    public class TipsWorldObject : MonoBehaviour
    {
        private void Start()
        {
            var text = GetComponent<TMPro.TextMeshPro>();
            text.text = "TIPS: £"+GameManager.Instance.Tips;
        }
    }
}