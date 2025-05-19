using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace EndlessExistence.Item_Interaction.Scripts.ObjectScripts.SingleObjectScripts
{
    public class StatueObject : ObjectContainer
    {
        [SerializeField] private GameObject MineGameMenu;
        [SerializeField] private Button ButtonMiniGame;
        [SerializeField] private string SceneName;

        private FirstpersonController Firstperson;
        private void Start()
        {
            Firstperson = FindAnyObjectByType<FirstpersonController>(); 
            ButtonMiniGame.onClick.AddListener(LoadScene);
        }
        private bool isEnabled = false;
        public override void Interact()
        {
            isEnabled = !isEnabled; 
            MineGameMenu.SetActive(isEnabled);
            Firstperson.SetLockCursor(isEnabled);
            

        }

        public void LoadScene()
        {
            SceneManager.LoadScene(SceneName, LoadSceneMode.Additive);
         
        }


    }
}
