using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace EndlessExistence.Item_Interaction.Scripts.ObjectScripts.SingleObjectScripts
{
    public class StatueObject : ObjectContainer
    {
        [SerializeField] private GameObject OkomfoMenuPanel;
        [SerializeField] private FirstpersonController Firstperson;
        [SerializeField] private Button PlayOkomfo;
        private void Start()
        {
            PlayOkomfo.onClick.AddListener(OpenOkomfo);
        }
        private bool isEnabled = false;
        public override void Interact()
        {
            isEnabled = !isEnabled; 
            OkomfoMenuPanel.SetActive(isEnabled);
            Firstperson.SetLockCursor(isEnabled);
            

        }

        public void OpenOkomfo()
        {
            SceneManager.LoadScene("Okomfo", LoadSceneMode.Additive);
         
        }


    }
}
