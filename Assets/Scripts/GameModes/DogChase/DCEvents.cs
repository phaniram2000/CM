using System;


public static partial class DCEvents
{

    public static event Action GirlCollidedWithObstacle;
}

public static partial class DCEvents
{
    public static void InvokeOnGirlCollidedWithObstacle() => GirlCollidedWithObstacle?.Invoke();
}
