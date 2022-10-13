using System.Collections.Generic;
using DG.Tweening;
using StateMachine;
using UnityEngine;
using UnityEngine.UI;

public class GameToShop : MonoBehaviour
{
    [SerializeField] private Button shopButton, backButton;
    [SerializeField] private GameObject shopScene;

    [SerializeField] private RectTransform shopButtonHide, shopButtonShow;

    private readonly Dictionary<Canvas, bool> _objectsToHide = new();

    private void OnEnable()
    {
        GameEvents.TapToPlay += OnTapToPlay;
    }

    private void OnDisable()
    {
        GameEvents.TapToPlay -= OnTapToPlay;
    }

    private void Start()
    {
        foreach (var canvas in FindObjectsOfType<Canvas>())
        {
            if (canvas.gameObject == gameObject) continue;
            if (canvas.renderMode == RenderMode.WorldSpace) continue;

            _objectsToHide.Add(canvas, canvas.enabled);
        }
    }
	
    private void ShowShopScene()
    {
        AInputHandler.AssignNewState(InputState.Disabled);
        ((RectTransform)shopButton.transform).DOAnchorPos(shopButtonHide.anchoredPosition, 0.5f)
            .OnStart(() => shopButton.interactable = false)
            .OnComplete(() => shopButton.interactable = true);

        backButton.image.DOColor(Color.white, 0.25f)
            .OnStart(() =>
            {
                backButton.interactable = false;
                backButton.gameObject.SetActive(true);
            })
            .OnComplete(() => backButton.interactable = true);

        foreach (var obj in _objectsToHide) obj.Key.enabled = false;
        shopScene.SetActive(true);


        if (AudioManager.instance)
        {
            AudioManager.instance.Play("Button");
        }
        Vibration.Vibrate(30);
    }

    private void HideShopScene()
    {
        ((RectTransform)shopButton.transform).DOAnchorPos(shopButtonShow.anchoredPosition, 0.5f)
            .OnStart(() => shopButton.interactable = false)
            .OnComplete(() => shopButton.interactable = true);

        backButton.image.DOColor(Color.clear, 0.25f)
            .OnStart(() => backButton.interactable = true)
            .OnComplete(() =>
            {
                backButton.interactable = false;
                backButton.gameObject.SetActive(false);
            });

        foreach (var obj in _objectsToHide)
            obj.Key.enabled = obj.Value;
        shopScene.SetActive(false);
        AInputHandler.AssignNewState(InputState.Idle);
        if (AudioManager.instance)
        {
            AudioManager.instance.Play("Button");
            
        }
        Vibration.Vibrate(30);
    }

    public void PressShopButton() => ShowShopScene();

    public void PressHideShopButton() => HideShopScene();

    private void OnTapToPlay()
    {
        // Just hide the shop button
        ((RectTransform)shopButton.transform).DOAnchorPos(shopButtonHide.anchoredPosition, 0.5f)
            .OnStart(() => shopButton.interactable = false)
            .OnComplete(() => shopButton.gameObject.SetActive(false));
    }
}