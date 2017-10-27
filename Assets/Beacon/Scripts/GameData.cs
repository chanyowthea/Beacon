using UnityEngine;
using System.Collections;
using UnityEngine.UI; 
using System; 


public static class DirCode
{
	public const byte EAST = 1; 
	public const byte WEST = 2; 
	public const byte SOUTH = 4; 
	public const byte NORTH = 8; 
}

public static class MapCode
{
	public const char NOT_EXIST = 'z'; 
	public const char NONE = '0'; 
	public const char WALL = '1'; 
	public const char PIT = '2'; 
	public const char ENEMY = '3'; 
	public const char NPC_GRAND_DAUGHTER = '4'; 
	public const char NPC_DARK_PRINCE = '5'; 
	public const char PLAYER = '7'; 
	public const char BEFORE_DOWNSTAIR = '8'; 
	public const char BEFORE_UPSTAIR = '9'; 
}

public struct Pos
{
	public int _x; 
	public int _y; 
	public Pos(int x, int y)
	{
		_x = x; 
		_y = y; 
	}

	public override string ToString()
	{
		return string.Format("[Pos: x: {0}, y: {1}]", _x, _y);
	}

	public static bool operator==(Pos pos1, Pos pos2)
	{
		if (pos1._x == pos2._x && pos1._y == pos2._y)
		{
			return true; 
		}
		return false; 
	}

	public static bool operator!=(Pos pos1, Pos pos2)
	{
		if (pos1._x == pos2._x && pos1._y == pos2._y)
		{
			return false; 
		}
		return true; 
	}

	public static Pos operator/(Pos pos, float scale)
	{
		return new Pos((int)(pos._x / scale), (int)(pos._y / scale)); 
	}

	public int Length{get {return (int)Math.Sqrt(Math.Pow(_x, 2) + Math.Pow(_y, 2)); }}

	public Vector2 ToVector2()
	{
		return new Vector2(_x, _y); 
	}

	public static bool IsParalell(Pos x, Pos y)
	{
		x /= x.Length; 
		y /= y.Length; 
		return x._x == y._x && x._y == y._y; 
	}
}

public class GameData : MonoBehaviour
{
	public static GameData _Instance; 

	#region Main

	void Awake()
	{
		_Instance = this; 
	}

	public void Init()
	{
		InitPlayer(); 
		InitGame(); 
	}

	public void Reset()
	{
		ResetPlayer(); 
		ResetGame(); 
	}

	public void Clear()
	{
		ResetPlayer(); 
		ResetGame(); 
	}
	#endregion





	// MapManager
	#region Map
	[SerializeField] public Transform _wallPrefab;
	[NonSerialized] public Transform _wallParent; 
	#endregion

	#region Items

	[SerializeField] public Transform _pitPrefab; 
	#endregion

	#region Player

	[SerializeField] public Player _playerPrefab; 
	[SerializeField] public Transform _enemyPrefab;
//	[SerializeField] public Transform _npcPrefab; 
	[SerializeField] public Transform _grandDaughterPrefab; 
	[SerializeField] public Transform _darkPrincePrefab; 
	[SerializeField] public Transform _hitPointPrefab; 
	#endregion

	#region Plot
	[SerializeField] public PlotConf _plot_Meet;  
	[SerializeField] public PlotConf _plot_Battle;
	[SerializeField] public PlotConf _plot_Battle_NoGrandDaughter;
	[SerializeField] public PlotConf _plot_Start; 
	[SerializeField] public PlotConf[] _plot_Allures; 
	[SerializeField] public PlotConf _plot_DarkPrinceDie; 
	[SerializeField] public PlotConf _plot_BattleAfter; 
	[SerializeField] public PlotConf _plot_BattleAfter_NoGrandDaughter; 
	[SerializeField] public PlotConf _plot_GrandDaughterDie; 
	[SerializeField] public RoleLibrary _roleLib; 
	[SerializeField] public float _conversSpeed = 1f; 
	public static int _curMeetHint = -1; 
	#endregion



	// GameManager
	#region Level

	public static int _CurLevel = 1;
	public const int _MaxLevel = 5;



	#endregion
	void InitGame()
	{
		ResetGame(); 
	}

	void ResetGame()
	{
		_CurLevel = 1; 
	}

	// UIManager
	#region Tutorial

	public static bool _IsOpenTutorial = true;
	public static bool _IsShowedOpenDoorTutorial = true;

	#endregion

	#region Step

	public static int _Step = 0;
	#endregion


	#region Player
	public static bool _CanRotateCamera = false;
	public static bool _HasRotated = false;
	public static bool _isOpenMask = true; 
	public static bool _isLockDoor = false; 
	public static bool _isGrandDaughterRebel = false; 
	public static bool _isGrandDaughterInQueue = false;  
	void InitPlayer()
	{
		ResetPlayer(); 
	}

	void ResetPlayer()
	{
		_CanRotateCamera = false; 
		_HasRotated = false; 
		_Step = 0; 
		_isLockDoor = false; 
		_isGrandDaughterInQueue = false; 
		_isOpenMask = true; 
	}
	#endregion
}
