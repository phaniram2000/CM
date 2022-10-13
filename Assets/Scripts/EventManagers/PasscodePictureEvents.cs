using System;

public static partial class PasscodePictureEvents
{
	public static event Action<bool> PicturesShared; 
}

public static partial class PasscodePictureEvents
{
	public static void InvokePicturesShared(bool areAllPicturesControversial) => PicturesShared?.Invoke(areAllPicturesControversial);
}