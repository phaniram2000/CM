using System;
using System.Collections.Generic;
using UnityEngine;

namespace Meta
{
	public enum OutfitName { Karen_Default, Karen_Yellow, Karen_Salmon, Karen_SuitPant, Karen_Red, Karen_White }
	public enum MaskName
	{
		NoMask,
		Hacker,
		Pumpkin,
		Marshmello,
		Venetian,
		Disguise,
		Santa,
		Rabbit,
		Hockey,
		Squid,
		StarGlasses,
		Theatre,
		Carnival,
		HeartGlasses,
		Joker,
	}

	public enum HatName
	{ NoHat, Chief, Tiara, Crown, Headphones, Jester, Judge, PinkHat, Pirate, RomanHelmet, UncleSam, VikingHelmet, Watermelon, Witch }

	public enum CharacterName { Karen }
	[Serializable] public class Outfit
	{
		public OutfitName name;
		[SerializeField] private int bottoms, hair, shoes, top;
		[SerializeField] private Color hairColor;
		
		public void SetOutfit(ref OutfitMeshes character)
		{
			character.SetBottoms(bottoms);
			character.SetShoes(shoes);
			character.SetTop(top);
			character.SetHair(hair, hairColor);
		}
	}
	[Serializable] public class OutfitMeshes
	{
		public List<GameObject> hair, tops, bottoms, shoes;
		
		public void SetShoes(int index) => MetaCharacterCustomisation.EnableOnlyIndexInList(ref shoes, index);

		public void SetBottoms(int index) => MetaCharacterCustomisation.EnableOnlyIndexInList(ref bottoms, index);

		public void SetTop(int index) => MetaCharacterCustomisation.EnableOnlyIndexInList(ref tops, index);

		public void SetHair(int index, Color hairColor)
		{
			MetaCharacterCustomisation.EnableOnlyIndexInList(ref hair, index);
			var hairRenderer = hair[index].GetComponent<SkinnedMeshRenderer>();

			if (!hairRenderer) hairRenderer = hair[index].transform.GetChild(0).GetComponent<SkinnedMeshRenderer>();
			
			hairRenderer.GetComponent<SkinnedMeshRenderer>().sharedMaterial.color = hairColor;
		}
	}
}