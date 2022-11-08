using UnityEngine;
using DG.Tweening;
using TMPro;
using Image = UnityEngine.UI.Image;

public class UiDotween : MonoBehaviour
{
    public GameObject handTut;
    public GameObject instruction;
    void Start()
    {
        handTut.transform.DOLocalMove(new Vector3(32, -987, 0), 3).SetEase(Ease.OutQuint).SetLoops(-1,LoopType.Restart);
        handTut.transform.GetComponent<Image>().DOFade(0, 12).SetEase(Ease.Linear);
        // instruction.transform.DOShakePosition(1, new Vector3(15, 15, 15), 2, 90, fadeOut: true).SetEase(Ease.OutQuad)
        //     .SetLoops(-1, LoopType.Incremental);
       
        instruction.transform.GetComponent<TextMeshProUGUI>().DOFade(0, 12).SetEase(Ease.Linear);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
