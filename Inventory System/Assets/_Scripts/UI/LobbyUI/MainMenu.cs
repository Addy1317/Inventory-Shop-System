using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace SavageWarrior
{
    public class MainMenu : MonoBehaviour
    {
        [Header("Button Reference")]
        [SerializeField] private Button playButton;
        [SerializeField] private Button inventoryButton;
        [SerializeField] private Button settingButton;
        [SerializeField] private Button backButton;
        [SerializeField] private Button quitButton;

        public void PlayButton()
        {
            SceneManager.LoadScene(1);
            Debug.Log("Opening Game");
        }

        public void InventoryButton()
        {
            SceneManager.LoadScene(2);
            Debug.Log("Opening Inventory");
        }

        public void SettingButton()
        {
            Debug.Log("Setting are Activated");
        }

        public void BackButton()
        {

        }

        public void QuitButton()
        {
            Application.Quit();
            Debug.Log("Application is Quitting");
        }
    }
}