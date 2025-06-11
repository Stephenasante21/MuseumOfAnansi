using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace EndlessExistence.Item_Interaction.Scripts.ObjectScripts.SingleObjectScripts
{
    public class StatueObject : ObjectContainer
    {

        [SerializeField] private GameObject mineGameMenu;

        [SerializeField] private Button easyButton;      
        [SerializeField] private Button hardButton;      
        [SerializeField] private string easySceneName;   
        [SerializeField] private string hardSceneName;   

        private FirstpersonController Firstperson;
        private void Start()
        {
            Firstperson = FindAnyObjectByType<FirstpersonController>();

            easyButton.onClick.AddListener(LoadEasyScene);
            hardButton.onClick.AddListener(LoadHardScene);
        }
        private bool isEnabled = false;
        public override void Interact()
        {
            mineGameMenu.SetActive(true);
            MouseManager.Instance.UnlockCursor();

        }

        private void LoadEasyScene()
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(easySceneName, LoadSceneMode.Additive);
            mineGameMenu.SetActive(false);
        }

        private void LoadHardScene()
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(hardSceneName, LoadSceneMode.Additive);
            mineGameMenu.SetActive(false);
        }
    }
}
