using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using HYPLAY.Core.Runtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace HYPLAY.Demo
{
    public class GetCustomUser : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI text;
        [SerializeField] private CanvasGroup signInSplash;
        [SerializeField] private GameObject mainMenu;

        private void Awake()
        {
            HyplayBridge.LoggedIn += OnLoggedIn;
            if (HyplayBridge.IsLoggedIn)
            {
                OnLoggedIn();
            }
        }

        private async void OnLoggedIn()
        {
            signInSplash.alpha = 0;
            signInSplash.blocksRaycasts = false;
            mainMenu.SetActive(true);
            var res = await HyplayBridge.GetUserAsync();
            if (res.Success)
                text.text = $"Logged as {res.Data.Username}";
        }
    }
}