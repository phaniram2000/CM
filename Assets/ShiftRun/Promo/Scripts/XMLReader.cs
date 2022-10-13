using UnityEngine;
using System.Collections;
using System.Xml;
using System;
using UnityEngine.UI;
public class XMLReader : MonoBehaviour 
{
	XmlNode MainNode;
	void Start()
	{
		StartCoroutine ("LoadAllCommonXML");

	}
	IEnumerator LoadAllCommonXML()
	{
		if (AdManager.instance.isWifi_OR_Data_Availble ()) {
		//	Debug.Log ("------------------------XML Reader Start");	
			WWW xmlData = new WWW(AdManager.instance.AllCommonUrl);
			yield return xmlData;
			if (xmlData.error != null) {
				// error, do something
				Debug.LogError("---------- error DoSomething");
			} else {
				// use the code I provided here :)
				Debug.Log("implement code");
				XmlDocument xmlDoc = new XmlDocument();
				try {
					xmlDoc.LoadXml (xmlData.text);
				} catch (Exception ex) {
					Debug.LogError ("----------------Error loading " + ":\n" + ex);
				} finally {
//					Debug.Log ("------------------------ loaded");
					//Read
					MainNode = xmlDoc.SelectSingleNode ("GameData");
					if (AdManager.instance.IsPortrait) 
					{
						ReadAllCommonXmlDataPortrail ();
					} 
					else 
					{
						ReadAllCommonXmlDataLandscape ();
					}
					ReadAllCommonXmlDataCion ();

				}
			}

		}
		else
		{
			Debug.Log ("No Internet Connection to get XML data");
		}
	}


	XmlNode MoreGamesNode;
	XmlNode IconGameNode;
	XmlNode LandscapeNode;
	private void ReadAllCommonXmlDataPortrail()
	{

		Debug.Log("---------- ReadingXmlData ------------");
	/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		/// ///////////////////moregame//////////////////////////////////////////////////////////////////////////////////////////
		if (MainNode.SelectSingleNode ("moregames") != null) {
			MoreGamesNode = MainNode.SelectSingleNode ("moregames");
			AdManager.instance.MgImgLinkList.Clear ();
			for (int i = 0; i < 10; i++) {
				if (MoreGamesNode.Attributes.GetNamedItem ("mgImgLink" + (i + 1)) != null) {
					string mgLinkto = MoreGamesNode.Attributes.GetNamedItem ("mgImgLink" + (i + 1)).Value;
					AdManager.instance.MgImgLinkList.Add (mgLinkto);
				}
			}

			AdManager.instance.MgLinkToList.Clear ();
			for (int i = 0; i < AdManager.instance.MgImgLinkList.Count; i++) {
				if (MoreGamesNode.Attributes.GetNamedItem ("mgLinkto" + (i + 1)) != null) {
					string mgLinkto = MoreGamesNode.Attributes.GetNamedItem ("mgLinkto" + (i + 1)).Value;
					AdManager.instance.MgLinkToList.Add (mgLinkto);
				}
			}
			AdManager.instance.MgImgList.Clear ();
			for (int i = 0; i < AdManager.instance.MgImgLinkList.Count; i++) {
				AdManager.instance.MgImgList.Add (null);
			}
			StartCoroutine (Downloadportrait (AdManager.instance.MgImgLinkList [0], 0));
		}	
	}
	private void ReadAllCommonXmlDataCion()
	{

		Debug.Log ("---------- ReadingXmlData ------------");
		/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		/// ///////////////////moregame//////////////////////////////////////////////////////////////////////////////////////////
		if (MainNode.SelectSingleNode ("iconpromo") != null) {
			IconGameNode = MainNode.SelectSingleNode ("iconpromo");
			AdManager.instance.IconLinkList.Clear ();
			for (int i = 0; i < 10; i++) {
				if (IconGameNode.Attributes.GetNamedItem ("iconimg" + (i + 1)) != null) {
					string mgLinkto = IconGameNode.Attributes.GetNamedItem ("iconimg" + (i + 1)).Value;
					AdManager.instance.IconLinkList.Add (mgLinkto);
				}
			}

			AdManager.instance.IconToList.Clear ();
			for (int i = 0; i < AdManager.instance.IconLinkList.Count; i++) {
				if (IconGameNode.Attributes.GetNamedItem ("iconLinkto" + (i + 1)) != null) {
					string mgLinkto = IconGameNode.Attributes.GetNamedItem ("iconLinkto" + (i + 1)).Value;
					AdManager.instance.IconToList.Add (mgLinkto);
				}
			}
			AdManager.instance.Iconlist.Clear ();
			for (int i = 0; i < AdManager.instance.IconLinkList.Count; i++) {
				AdManager.instance.Iconlist.Add (null);
			}
			StartCoroutine (DownloadiConImg (AdManager.instance.IconLinkList [0], 0));
		}	
	}

	private void ReadAllCommonXmlDataLandscape()
	{

		Debug.LogError ("---------- ReadingXmlData ------------");
		/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		/// ///////////////////moregame//////////////////////////////////////////////////////////////////////////////////////////
		if (MainNode.SelectSingleNode ("landscapepromo") != null) {
			MoreGamesNode = MainNode.SelectSingleNode ("landscapepromo");
			AdManager.instance.LandLinkList.Clear ();
			for (int i = 0; i < 10; i++) {
				if (MoreGamesNode.Attributes.GetNamedItem ("landscImgLink" + (i + 1)) != null) {
					string mgLinkto = MoreGamesNode.Attributes.GetNamedItem ("landscImgLink" + (i + 1)).Value;
					AdManager.instance.LandLinkList.Add (mgLinkto);
				}
			}

			AdManager.instance.LandToList.Clear ();
			for (int i = 0; i < AdManager.instance.LandLinkList.Count; i++) {
				if (MoreGamesNode.Attributes.GetNamedItem ("landLinkto" + (i + 1)) != null) {
					string mgLinkto = MoreGamesNode.Attributes.GetNamedItem ("landLinkto" + (i + 1)).Value;
					AdManager.instance.LandToList.Add (mgLinkto);
				}
			}
			AdManager.instance.LandList.Clear ();
			for (int i = 0; i < AdManager.instance.LandLinkList.Count; i++) {
				AdManager.instance.LandList.Add (null);
			}
			StartCoroutine (Downloadlandscape (AdManager.instance.LandLinkList [0], 0));
		}	
	}

	/// <summary>
	/// T///////////////////////////////////////////////////////////////////////////////////////	/// </summary>
	int NextMGIndex=0;
	private IEnumerator Downloadportrait(string url,int Index)
	{
		int containIndex = IsFoundMGLink (Index);
		if (containIndex == -1) {
			NextMGIndex=Index+1;
			WWW menuAdView = new WWW (url);
			yield return menuAdView;
			Texture2D texture = menuAdView.texture;
			AdManager.instance.MgImgList [Index] = Sprite.Create (texture, new Rect (0, 0, texture.width, texture.height), new Vector2 (0, 0));
			if (NextMGIndex < AdManager.instance.MgLinkToList.Count) 
			{
				StartCoroutine (Downloadportrait (AdManager.instance.MgImgLinkList [NextMGIndex], NextMGIndex));
			}
			if (NextMGIndex > AdManager.instance.MgLinkToList.Count-1) {
				StartCoroutine (MenuAdPage.instance.LoadImg());
				MenuAdPage.instance.portraitloaded = true;
				StartCoroutine (RemoveCurrentApplink ());
				}
		} else {
			NextMGIndex=Index+1;
			AdManager.instance.MgImgList [Index] = AdManager.instance.MgImgList [containIndex];
			if (NextMGIndex < AdManager.instance.MgLinkToList.Count) {
				StartCoroutine (Downloadportrait (AdManager.instance.MgImgLinkList [NextMGIndex], NextMGIndex));
			}
			yield break;
		
		}
	
	}
	/// <summary>icon
	/// D////////////////////////////////////////////////////////////////////////////////////////////////	/// </summary>
	/// <returns><c>true</c> if this instance is found MG link the specified Index; otherwise, <c>false</c>.</returns>
	/// <param name="Index">Index.</param>
	/// 
	 int NextIConIndex=0;
	private IEnumerator DownloadiConImg(string url,int Index)
	{
		int containIndex = IsFoundicLink (Index);
		if (containIndex == -1) {
			NextIConIndex=Index+1;
			WWW menuAdView = new WWW (url);
			yield return menuAdView;
			Texture2D texture = menuAdView.texture;
			AdManager.instance.Iconlist [Index] = Sprite.Create (texture, new Rect (0, 0, texture.width, texture.height), new Vector2 (0, 0));
			if (NextIConIndex < AdManager.instance.IconToList.Count) 
			{
				StartCoroutine (DownloadiConImg (AdManager.instance.IconLinkList [NextIConIndex], NextIConIndex));
			}
			if (NextIConIndex > AdManager.instance.IconToList.Count-1) {
				AdManager.instance.IconLoaded = true;

			}
		} else {
			NextIConIndex=Index+1;
			AdManager.instance.Iconlist [Index] = AdManager.instance.Iconlist [containIndex];
			if (NextIConIndex < AdManager.instance.IconToList.Count) {
				StartCoroutine (DownloadiConImg (AdManager.instance.IconLinkList [NextIConIndex], NextIConIndex));
			}
			yield break;

		}

	}

	int NextLandIndex=0;
	private IEnumerator Downloadlandscape(string url,int Index)
	{
		int containIndex = IsFoundlaLink (Index);
		if (containIndex == -1) {
			NextLandIndex=Index+1;
			WWW menuAdView = new WWW (url);
			yield return menuAdView;
			Texture2D texture = menuAdView.texture;
			AdManager.instance.LandList [Index] = Sprite.Create (texture, new Rect (0, 0, texture.width, texture.height), new Vector2 (0, 0));
			if (NextLandIndex < AdManager.instance.LandToList.Count) 
			{
				StartCoroutine (Downloadlandscape (AdManager.instance.LandLinkList [NextLandIndex], NextLandIndex));
			}
			if (NextLandIndex > AdManager.instance.LandToList.Count-1) {
				StartCoroutine (MenuAdPage.instance.Loadland());
				MenuAdPage.instance.LandscapeLoaded = true;
			}
		} else {
			NextLandIndex=Index+1;
			AdManager.instance.LandList [Index] = AdManager.instance.LandList [containIndex];
			if (NextLandIndex < AdManager.instance.LandToList.Count) {
				StartCoroutine (Downloadlandscape (AdManager.instance.LandLinkList [NextLandIndex], NextLandIndex));
			}
			yield break;

		}

	}

	/// <summary>
	/// D	/// </summary> portrait
	/// <returns><c>true</c> if this instance is found MG link the specified Index; otherwise, <c>false</c>.</returns>
	/// <param name="Index">Index.</param>
	private int IsFoundMGLink(int Index)
	{
		for (int i = 0; i < AdManager.instance.MgImgLinkList.Count; i++) {
			if (Index!=i && AdManager.instance.MgImgLinkList [Index] == AdManager.instance.MgImgLinkList [i] && AdManager.instance.MgImgList [i]!=null) {
				return i;
			}			
		}
		return -1;
	}
	/// <summary> icon
	/// Determines whether this instance is found MG link the specified Index.
	/// </summary>
	/// <returns><c>true</c> if this instance is found MG link the specified Index; otherwise, <c>false</c>.</returns>
	/// <param name="Index">Index.</param>
	private int IsFoundicLink(int Index)
	{
		for (int i = 0; i < AdManager.instance.IconLinkList.Count; i++) {
			if (Index!=i && AdManager.instance.IconLinkList [Index] == AdManager.instance.IconLinkList [i] && AdManager.instance.Iconlist [i]!=null) {
				return i;
			}			
		}
		return -1;
	}
	/// <summary>landscape
	/// D/	/// </summary>
	/// <returns><c>true</c> if this instance is found MG link the specified Index; otherwise, <c>false</c>.</returns>
	/// <param name="Index">Index.</param>
	private int IsFoundlaLink(int Index)
	{
		for (int i = 0; i < AdManager.instance.LandLinkList.Count; i++) {
			if (Index!=i && AdManager.instance.LandLinkList [Index] == AdManager.instance.LandLinkList [i] && AdManager.instance.LandList [i]!=null) {
				return i;
			}			
		}
		return -1;
	}

	IEnumerator RemoveCurrentApplink()
	{
		
		yield return new WaitForSeconds(5);
		for(int i =0 ;i >AdManager.instance.MgLinkToList.Count;i++)
		{
			Debug.Log ("+++++++++++++++++++++++");
			Debug.Log ("checking");
			if(AdManager.instance.MgLinkToList[i].Contains("com.FridayBoxGames.AxeHit"))
			{
				AdManager.instance.MgImgLinkList.RemoveAt (i);
				AdManager.instance.MgImgList.RemoveAt (i);
				AdManager.instance.MgImgLinkList.RemoveAt (i);
			}
		}


	

	}
}
