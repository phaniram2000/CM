using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class DialogueBaseController : MonoBehaviour
{
    [SerializeField] protected GameObject dialogBox;


    [SerializeField] protected TextMeshPro dialogText;
    [SerializeField] protected float dialogBoxScale;
    
    [System.Serializable]
    public struct  Messages
    {
        public int id;
        [Multiline] public string message;
    }

    [SerializeField] protected List<Messages> messagesList;
    
    
    private void Start()
    {
        if(!dialogBox) return;
        
       DisableDialogBox();
       Initialise();
    }

    protected virtual void Initialise() {}
    
    protected void EnableDialogBox()
    {
        print("Enable boy dialog");
        dialogBox.transform.localScale = Vector3.zero;
        dialogBox.SetActive(true);
        dialogBox.transform.DOScale(Vector3.one * dialogBoxScale, 0.5f).SetEase(Ease.InBack);
        
        
    }
    
    protected string GetMessage(int id)
    {
        for (int i = 0; i < messagesList.Count; i++)
        {
            if (messagesList[i].id == id)
            {
                return messagesList[i].message;
            }
        }

        return "";
    }

    protected void DisableDialogBox()
    {
        dialogBox.SetActive(false);
    }
}
