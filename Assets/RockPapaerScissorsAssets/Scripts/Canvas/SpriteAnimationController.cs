using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Image = UnityEngine.UI.Image;

namespace RPS
{

	public class SpriteAnimationController : MonoBehaviour
	{
		private Image _myImage;

		[SerializeField] private List<Sprite> sprites;
		[SerializeField] private float animationDuration;
		private float _myFloat = 0f;
		private int _spriteCount;
		private Tween _spritesheet;

		private void OnDestroy() => _spritesheet.Kill();

		private void Start()
		{
			_myImage = GetComponent<Image>();
			_spriteCount = sprites.Count;
			DoAnimation();
		}

		private void DoAnimation()
		{
			_spritesheet = DOTween.To(() => _myFloat, x => _myFloat = x, 1, animationDuration).OnUpdate(() =>
			{
				var i = (int)Mathf.Lerp(0, _spriteCount, _myFloat);

				if (!_myImage) _spritesheet.Kill();

				_myImage.sprite = sprites[i];
			}).SetEase(Ease.Linear).SetLoops(-1);
		}
	}
}
