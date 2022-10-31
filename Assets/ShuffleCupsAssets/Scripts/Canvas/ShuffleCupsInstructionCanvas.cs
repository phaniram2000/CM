using DG.Tweening;
using UnityEngine;


namespace ShuffleCups
{
    public class ShuffleCupsInstructionCanvas : MonoBehaviour
    {
        [SerializeField] private GameObject instructionGameObject;
        [SerializeField] private bool isPaperPullLevel;

        private void Start()
        {
            instructionGameObject.SetActive(false);
        }

        private void OnEnable()
        {
            GameEvents.Singleton.ShowInstruction += ShowInstruction;
            GameEvents.Singleton.GameResult += OnGameResult;

            global::GameEvents.TapToPlay += OnPaperGameStart;


            if (!isPaperPullLevel) return;
            
            PaperGameEvents.Singleton.aiCrossFinishLine += DisableInstruction;
            PaperGameEvents.Singleton.playerCrossFinishLine += DisableInstruction;
        }

        private void OnDisable()
        {
            GameEvents.Singleton.ShowInstruction -= ShowInstruction;
            GameEvents.Singleton.GameResult -= OnGameResult;
            
            global::GameEvents.TapToPlay -= OnPaperGameStart;
            
            
            if (!isPaperPullLevel) return;
            PaperGameEvents.Singleton.aiCrossFinishLine -= DisableInstruction;
            PaperGameEvents.Singleton.playerCrossFinishLine -= DisableInstruction;
        }

        
        private void OnGameResult(bool obj)
        {
            DisableInstruction();
        }

        private void ShowInstruction()
        {
           EnableInstruction();
        }
        
        private void OnPaperGameStart()
        {
            DOVirtual.DelayedCall(0.4f, () => EnableInstruction());
        }

        private void EnableInstruction()
        {
            instructionGameObject.SetActive(true);
        }

        private void DisableInstruction()
        {
            instructionGameObject.SetActive(false);
        }
    }
}

