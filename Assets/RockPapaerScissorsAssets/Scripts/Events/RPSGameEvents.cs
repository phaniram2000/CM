using System;
using RPS;

public static partial class RPSGameEvents
	{

		public static event Action OnTapToPlay;
		public static event Action<GameMoves> MoveSelectedByPlayer;
		public static event Action CameraZoom, CameraZoomActionCompleted, CameraZoomNormalCompleted;

		public static event Action GameTie;

		public static event Action PlayerWin, PlayerLose, NpcWin, NpcLose;

		public static event Action PlayerStartGiveSlap, AllowPlayerToSlap;

		public static event Action NewRound;

		public static event Action PlayerGaveSlap, NpcGaveSlap;

		public static event Action GameLose, GameWin;

		public static event Action GameMovesTextEnable, GameMovesTextDisable;

		public static event Action PlayerHelmetEnable, PlayerHelmetFall;

		public static event Action<float> IncreasePlayerHealth;

		public static event Action PowerSlapGiven;

	}

	public static partial class RPSGameEvents
	{
		// bhai isme se shayad kafi sare events singlecast hai, unko sudharo,time ke kami ke wajah se maine ye rasta chuna,maff karo.
		
		public static void InvokeOnMoveSelectedByPlayer(GameMoves move) => MoveSelectedByPlayer?.Invoke(move);
		public static void InvokeOnCameraZoom() => CameraZoom?.Invoke();
		public static void InvokeOnCameraZoomActionCompleted() => CameraZoomActionCompleted?.Invoke();
		public static void InvokeOnCameraZoomNormalCompleted() => CameraZoomNormalCompleted?.Invoke();
		public static void InvokeOnGameTie() => GameTie?.Invoke();

		public static void InvokeOnPlayerWin() => PlayerWin?.Invoke();

		public static void InvokeOnPlayerLose() => PlayerLose?.Invoke();

		public static void InvokeOnNpcWin() => NpcWin?.Invoke();

		public static void InvokeOnNpcLose() => NpcLose?.Invoke();

		public static void InvokeOnPlayerStartGiveSlap() => PlayerStartGiveSlap?.Invoke();

		public static void InvokeOnNewRound() => NewRound?.Invoke();

		public static void InvokeOnPlayerGaveSlap() => PlayerGaveSlap?.Invoke();

		public static void InvokeOnNpcGaveSlap() => NpcGaveSlap?.Invoke();

		public static void InvokeOnAllowPlayerToSlap() => AllowPlayerToSlap?.Invoke();

		public static void InvokeOnTapToPlay() => OnTapToPlay?.Invoke();

		public static void InvokeOnGameLose() => GameLose?.Invoke();

		public static void InvokeOnGameWin() => GameWin?.Invoke();

		public static void InvokeOnGameMovesTextEnable() => GameMovesTextEnable?.Invoke();

		public static void InvokeOnGameMovesTextDisable() => GameMovesTextDisable?.Invoke();

		public static void InvokeOnPlayerHelmetEnable() => PlayerHelmetEnable?.Invoke();

		public static void InvokeOnPlayerHelmetFall() => PlayerHelmetFall?.Invoke();

		public static void InvokeOnIncreasePlayerHealth(float value) => IncreasePlayerHealth?.Invoke(value);

		public static void InvokeOnPowerSlapGiven() => PowerSlapGiven?.Invoke();
	}