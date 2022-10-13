using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;

public class Tile : MonoBehaviour
{
    public bool isBomb;
    public bool isTapped;
    public Collider myCol;
    public GameObject balloon, bomb;
    public SpriteRenderer radialEffect;
    public ParticleSystem balloonBlast, bombBlast, bombFuse;

    Vector3 bombDefaultScale;
    Vector3 balloonDefaultScale;

	void Awake()
    {
        myCol = GetComponent<Collider>();
        bomb = transform.GetChild(2).gameObject;
        balloon = transform.GetChild(1).gameObject;
		radialEffect = transform.GetChild(0).GetComponent<SpriteRenderer>();

        bombFuse = bomb.transform.GetChild(0).GetComponent<ParticleSystem>();
        bombBlast = bomb.transform.GetChild(1).GetComponent<ParticleSystem>();
        balloonBlast = balloon.transform.GetChild(0).GetComponent<ParticleSystem>();

        radialEffect.gameObject.SetActive(false);
		radialEffect.transform.DORotate(Vector3.up * 180f, 5f, RotateMode.WorldAxisAdd)
			.SetLoops(-1, LoopType.Incremental)
			.SetEase(Ease.Linear);

        bombDefaultScale = bomb.transform.localScale;
        balloonDefaultScale = balloon.transform.localScale;
    }

    private void OnMouseDown()
    {
        if (isTapped || !MemoryBetUI.instance.gameplayUI.alreadyReveal)
            return;

        TapToRotate();
    }

    private void OnMouseOver()
    {
        if (isTapped || !MemoryBetUI.instance.gameplayUI.alreadyReveal)
            return;

        TapToRotate();
    }

    public void TurnBombOn()
    {
        isBomb = true;
        bomb.SetActive(true);
        balloon.SetActive(false);
    }

    public void TurnBalloonOn()
    {
        isBomb = false;
        bomb.SetActive(false);
        balloon.SetActive(true);
    }

    public void BlastTheBalloon(UnityAction callback)
    {
        balloon.transform.DOScale(Vector3.one * 0.3f, 0.2f).SetEase(Ease.InOutQuad).OnComplete(() =>
        {
            GameEssentials.instance.shm.PlayConfettiSound();
            balloon.GetComponent<MeshRenderer>().enabled = false;
            BalloonBlastEffect();
            radialEffect.DOPause();
            callback?.Invoke();
        });
    }

    public void BlastTheBomb(UnityAction callback)
    {
        bomb.transform.DOScale(Vector3.one * 0.005f, 0.2f).SetEase(Ease.InOutQuad).OnComplete(() =>
         {
             bomb.GetComponent<MeshRenderer>().enabled = false;
             BomBBlastEffect();
             radialEffect.DOPause();
             callback?.Invoke();
         });
    }

    public void BalloonBlastEffect()
    {
        balloonBlast.Play();
    }

    public void BomBBlastEffect()
    {
        bombFuse.Stop();
        bombBlast.Play();
        bombBlast.GetComponent<AudioSource>().Play();
    }

    public void Reveal(float delay)
    {
        transform.DORotate(new Vector3(0, 0, 180), 0.25f).SetDelay(delay).SetEase(Ease.InOutQuad).OnComplete(() =>
        {
            transform.DORotate(Vector3.zero, 0.25f).SetDelay(0.45f).SetEase(Ease.InOutQuad).OnComplete(() =>
            {
                MemoryBetUI.instance.gameplayUI.alreadyReveal = true;
            });
        });
    }

    public void TapToRotate()
    {
        if (isTapped || MemoryBetUI.instance.bombBlast) return;

        GameEssentials.instance.shm.PlayCorrectSound();
        isTapped = true;
        radialEffect.gameObject.SetActive(true);
        GameEssentials.instance.shm.Vibrate(18);
        transform.DORotate(new Vector3(0, 0, 180), 0.1f)
			.SetEase(Ease.InOutQuad)
			.OnComplete(() =>
			    {
					if (isBomb) 
			        {
			            MemoryBetUI.instance.bombBlast = true;
			            bombFuse.Play();
			            BlastTheBomb(()=> MemoryBetUI.instance.Invoke(nameof(MemoryBetUI.ActivateLooseUI),0.5f));
			        }
					else
						BlastTheBalloon(() => MemoryBet.instance.mbGameplay.CheckForSemiComplete());
				});
    }

    public void ResetThisTile()
    {
        // reset this tile
        isBomb = false;
        isTapped = false;
        
        bombFuse.Stop();
        bombBlast.Stop();
        balloonBlast.Stop();

        myCol.enabled = true;
        
        transform.rotation = Quaternion.identity;

        bomb.transform.localScale = bombDefaultScale;
        bomb.GetComponent<MeshRenderer>().enabled = true;
        balloon.transform.localScale = balloonDefaultScale;
        balloon.GetComponent<MeshRenderer>().enabled = true;
        MemoryBetUI.instance.gameplayUI.alreadyReveal = false;

        radialEffect.DOPlay();
        radialEffect.gameObject.SetActive(false);
	}
}

