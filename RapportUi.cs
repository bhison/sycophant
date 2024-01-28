using System;
using TMPro;
using UnityEngine;

namespace DefaultNamespace
{
    [RequireComponent(typeof(TextMeshPro))]
    public class RapportUi : MonoBehaviour
    {
        private TextMeshPro _textMeshPro;

        private void Awake()
        {
            _textMeshPro = GetComponent<TextMeshPro>();
        }

        private void Update()
        {
            _textMeshPro.text = (Math.Floor(GameManager.Instance.RapportPercent * 100)) + "%";
        }
    }
}