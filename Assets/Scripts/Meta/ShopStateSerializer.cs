using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace Meta
{
	public class ShopStateSerializer
	{
		private readonly string _savePath;

		public ShopStateSerializer(string savePath) => _savePath = savePath;

		private static ShopState InitialiseEmptyState()
		{
			var categories = Enum.GetNames(typeof(ShopCategory));
			
			var states = new List<ShopCategoryState>();
			for (var i = 0; i < categories.Length; i++)
			{
				var empty = new Dictionary<int, ShopItemState> { { 0, ShopItemState.Selected } };
				
				for(var j = 1; j < ShopStateHelpers.GetCategoryItemCount(i); j++)
					empty.Add(j, ShopItemState.Locked);
					
				var x = new ShopCategoryState
				{
					ItemStates = empty
				};

				Enum.TryParse(categories[i], out x.Category);
				states.Add(x);
			}

			return new ShopState(0, 4000, 1,states);
		}

		public void SaveCurrentState()
		{
			var currentShopState = ShopStateController.CurrentState.GetState();
			var save = new ShopState(currentShopState.BankBalance, currentShopState.RichRank, currentShopState.LoaderIndex,currentShopState.CategoryStates);

			var binaryFormatter = new BinaryFormatter();
			using (var fileStream = File.Create(_savePath)) 
				binaryFormatter.Serialize(fileStream, save);

			Debug.Log("Data Saved");
		}

		public ShopState LoadSavedState()
		{
			//if you are here to solve multiple calls at the same time,
			//here is what i suggest: set a delayed call to allow loading.
			// if someone comes to load state in that window, give them a current state
			if (!File.Exists(_savePath))
			{
				MonoBehaviour.print(_savePath);
				Debug.LogWarning("Save file doesn't exist. Initialising a blank one.");
				return InitialiseEmptyState();
			}
		
			ShopState state;

			var binaryFormatter = new BinaryFormatter();
			try
			{
				using var fileStream = File.Open(_savePath, FileMode.Open);
				state = (ShopState)binaryFormatter.Deserialize(fileStream);
			}
			catch (Exception e)
			{
				Console.WriteLine($"Broken save file data structure. Initialising new empty save game.\n{e}.");
				return InitialiseEmptyState();
			}

			Debug.Log("Data Loaded");
			return state;
		}

		//cant call from context menu because path isn't initialised then
		public void DeleteSavedState()
		{
			if (!File.Exists(_savePath))
			{
				MonoBehaviour.print("Data does not Exist at path");
				return;
			}

			MonoBehaviour.print("Data Deleted");
			File.Delete(_savePath);
		}
	}
}