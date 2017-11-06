
// --功能--
// 数据存储
// 数据读取

// 4 有怪物，没有武器和装备
// 7 体力下降，移动需要CD


// --战斗系统--
// 战斗系统先不做
// 角色可以向前左右攻击，左右伤害低，从后背攻击伤害高，攻击有CD，移动有CD，攻击和移动的CD有关系，都是减体力
// 画出人物俯视图和CG图，格子内可以看到剑伸出格子
// 遇见敌人，孙女在一旁不动，爷爷战斗，以爷爷为中心展开战斗格子，怪物计算敌人自己胜率，如果大于一定值就不逃跑，
// 否则移动到边界就逃跑，每一次自己或者敌人受伤都计算一次自己的胜率
// 有恢复HP的地方

// 9 灯塔前面的剧情

// --策划原则--
// 不需要的功能坚决砍掉，尽量简化，突核心玩法

// --BUG--
// 用后背撞敌人不应该造成伤害


// --编程Tips--
// Clear和Init相互独立，Init内部不能调用Clear
// Player是一个独立于GameManager的系统
// 父级调用子级函数，但是子级只能通过时间来通知父级调用函数

//-- 剧情
// 苍山若影，银河如练
// 漆黑的夜，怒号的海浪扑灭了灰穆港唯一的灯塔——维希灯塔
// 维希灯塔坐落于临野峰峰顶，地形险峻，道路崎岖
// 但若没了灯塔，船只便难以确定方位，从而大大增加了村民行船事故发生率，众人对此头疼不已
// 此时，双目失明的姜游毛遂自荐，只身前往点亮灯塔
// 众人见状纷纷劝阻
// 姜游却声称多次探索过维希灯塔，对临野峰地形了然于胸
// 在姜游的一再坚持下，众人终于成全了姜游的心愿
// 镇民们本欲馈赠些许礼物，姜游却一一婉拒
// 次日破晓，姜游未待众人送行便踏上行途……

// 上楼
// 一阵微弱的哭声从楼下传来
// 下楼
// 你摸了摸四处的墙壁，没有发现什么
// 一阵微弱的哭声从楼下传来
// 你摸了摸四处的楼梯，没有发现什么
// 一阵微弱的哭声从楼下传来
// 你向楼梯内侧走近，听见哭声变大

// 触发角色，角色id，对话语句
// 剧情开始
// 爷爷：姜游，孙女：姜寻
// 孙女：爷爷，爷爷，你在哪？我好饿，好冷，好想回家……呜呜……呜呜呜……
// 爷爷：是寻儿！原来她竟在这里，找得我好苦啊！
// 爷爷：寻儿别害怕，我在这里，爷爷在这里，没事的！
// 孙女：爷爷！爷爷！你在哪？
// 爷爷：寻儿，我在这里，你听得到吗？
// 孙女：我看到了！爷爷！爷爷！
// 孙女扑进爷爷怀中
// 爷爷轻抚着孙女的长发
// 孙女：爷爷，爷爷你终于来了！这里好黑好可怕呜呜……
// 爷爷：没事的，我们等会儿就回去！
// 孙女：我们什么时候回去？
// 爷爷：等点亮灯塔就回去。
// 孙女：好的，爷爷我带你去！（由于点亮灯塔要走很远的路，因此先点亮，不需要很长时间）
// SystemMsg：光明模式开启


// 黑王子：真巧，碰到两个猎物。
// 孙女：你……你是谁？
// 黑王子：嘿，告诉你无妨。我是守护灯塔的黑王子，任何胆敢损坏灯塔的异类都要被清除。 // 黑王子已经统治了灯塔
// 爷爷：胡说，这个灯塔是隶属治淮镇的，不属于任何个人！
// 黑王子：我说它属于我就属于我，你们若是识相就该乖乖交出保护费，然后回去。
// 爷爷：我来此必要点亮灯塔，才可回去！
// 黑王子：灯塔是否要点亮，我说了算！
// 孙女：爷爷，我们还是……
// 爷爷：寻儿，没事的。
// 爷爷：你这恶霸，今日我偏要点亮了！
// 黑王子：敬酒不吃吃罚酒！
// 孙女：爷爷小心。

// 战斗_无孙女
// 嗯！去哪里！
// 你是谁？莫要阻我。
// 你踩在我的地面上，你说我是谁？
// 无理取闹。
// 糟老头子，我看你是活腻了！
// 让开！
// 交出你身上的东西，让我打一顿出气，你就可以走了！
// 你要什么东西？
// 废话，当然是钱！
// 抱歉，我身无分文。
// 那就有你受的！

// 诱惑
// 黑王子：老头子，就凭你这把老骨头，还要跟我争斗，我可不想别人说我恃强凌弱。
// 爷爷：少得意了，我可不怕你。
// 孙女：爷爷小心，莫要被他分了心神。

// 黑王子：小姑娘，老头子不顾你的安危，偏要执意点亮灯塔，可见他并不在意你。
// 孙女：你……坏蛋……你住口！
// 爷爷：休要挑拨离间！

// 黑王子：我改变主意了，老头子。若是你把你孙女让给我，我就放你去点亮灯塔，如何？
// 爷爷：痴心妄想！
// 孙女：你！
// 黑王子：我真的是一番好心，这样对你几乎没有损失，反正你看重的只是灯塔而已。
// 爷爷：……


// 黑王子：我若是遇着你定会对你百般爱护，至少先让你吃好穿好休息好，看看你爷爷又是如何？
// 孙女：……
// 爷爷：你这样是没用的。寻儿，莫要听他疯言疯语。灯塔是小镇船夫的守护者，若是没有灯塔，行船事故率便会大大增加。爷爷今日肩负重任，为大我舍弃小我，希望寻儿能体谅爷爷。
// 孙女：嗯，没事的，爷爷。


// 黑王子：寻儿，我真的是挺喜欢你的，要不这样，你跟我一起生活，我放过你爷爷。
// 爷爷：寻儿，莫要被他妖言蛊惑！
// 孙女：……
// 黑王子：怎么样，寻儿？我保证对你呵护有加，也不会伤害你爷爷。
// 孙女：你真的……你说的是真的吗？
// 黑王子：当然，我对天起誓！
// 爷爷：寻儿！他说的定然都是假的！
// 孙女：……好。

// 黑王子：嘿嘿，可真是个好结局。行了，老头子，你可以走了！
// 爷爷：任务尚未完成，怎可退却！
// 孙女：爷爷，你回去吧。
// 爷爷：寻儿，快回来，我可以战胜这个家伙的。

// 黑王子：我竟然会……输……

// 爷爷：寻儿，快走吧，点亮灯塔就可以回去了！
// 孙女：嗯。



// -- 胜利 --
// 老人缓缓走上前
// 许久之后
// 老人感受到一股暖暖的气流扑面而来
// 灯塔终于摇曳起动人的光辉
// 此后
// 孤寂的身影再次前行……



// 状态机,怎么让用户设置状态,用字符串不行，用id可以
// 共用UI有三份内存，怎么解决，设置static变量是一种？
// 战斗碰撞，扇形、矩形
// UI框架
// Shader
// Lua


using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
	public static GameManager _Instance;
	[SerializeField] Camera _mainCamera;
	[SerializeField] GameObject _inputManager;
	[SerializeField] AudioManager _audioManager;
	Player _curPlayer;

	void Start()
	{
		_Instance = this; 
		Singleton.Init(); 
		Singleton._inputManager = _inputManager.GetComponent<InputManager>(); 
		Singleton._audioManager = _audioManager; 
		UIManager._Instance.Open<StartView>(EView.Start); 

//		Singleton._archiveManager.SaveFloor(); 
//		Singleton._archiveManager.LoadFloor(); 

//		#if TEST
//		InitAccelerate(); 
//		#endif
	}

	public void Init(bool isLoad = false)
	{
		UIManager._Instance.Init(); 
		GameData._Instance.Init(); 
		_inputManager.SetActive(true); 
		_mainCamera.gameObject.SetActive(false); 
		if (_curPlayer != null)
		{
			GameObject.Destroy(_curPlayer.gameObject); 
			_curPlayer = null; 
		}
		_curPlayer = GameObject.Instantiate(GameData._Instance._playerPrefab); 
		Player._Instance.Init(isLoad); 
		Player._Instance._OnReset = Reset; 
		Player._Instance._OnClear = Clear; 
	}

	public void Reset()
	{
		Player._Instance.Reset(); 
		GameData._Instance.Reset(); 
		UIManager._Instance.Reset(); 
	}

	void OnDestroy()
	{
		Clear(); 
	}

	public void Clear()
	{
		Singleton._archiveManager.DeleteTempData();

		GameData._Instance.Clear(); 
		UIManager._Instance.Clear(); 
		if (Player._Instance != null)
		{
			Player._Instance.Clear(); 
		}
		if (_mainCamera != null)
		{
			_mainCamera.gameObject.SetActive(true); 	
		}
		if (_inputManager != null)
		{
			_inputManager.SetActive(false); 
		}
		if (_curPlayer != null)
		{
			GameObject.Destroy(_curPlayer.gameObject); 
			_curPlayer = null; 
		}
	}

	#if TEST
//	public float accelerometerUpdateInterval = 1.0f / 60.0f;
//	public float lowPassKernelWidthInSeconds = 1.0f;
//	public float shakeDetectionThreshold = 4.0f;
//	Vector3 _acceleration; 
//	float lowPassFilterFactor;
//	Vector3 lowPassValue;
//	Vector3 acceleration;
//	Vector3 deltaAcceleration;    
//
//	void InitAccelerate()
//	{
//		lowPassFilterFactor = accelerometerUpdateInterval / lowPassKernelWidthInSeconds;
//		lowPassValue = Input.acceleration;
//	}
//
//	void UpdateAccelerate()
//	{
//		_acceleration = Input.acceleration;
//		lowPassValue = Vector3.Lerp(lowPassValue, acceleration, lowPassFilterFactor);
//		deltaAcceleration = acceleration - lowPassValue;
//		if (deltaAcceleration.sqrMagnitude >= shakeDetectionThreshold)
//		{
//			// Perform your "shaking actions" here, with suitable guards in the if check above, 
//			// if necessary to not, to not fire again if they're already being performed.
//			UIManager._Instance.SetMaskEnable(!GameData._isOpenMask); 
//			var view = UIManager._Instance.Open<MessageView>(EView.Message); 
//			view.message = "设置背景蒙板！"; 
//		}
//
//		ShowTime(); 
////		Invoke("ShowTime", 0.5f); 
//	}
//
//	void ShowTime()
//	{
//		UIManager._Instance.SetDebugInfo("_acceleration=" + _acceleration + ", lowPassValue=" + lowPassValue + 
//			", deltaAcceleration=" + deltaAcceleration + ", deltaAcceleration.sqrMagnitude=" + deltaAcceleration.sqrMagnitude +
//			", shakeDetectionThreshold=" + shakeDetectionThreshold); 
//	}

	void Update()
	{
//		UpdateAccelerate(); 

		if (Input.GetKey(KeyCode.Escape))
		{
			Application.Quit(); 
		}
		if (Input.GetKeyDown(KeyCode.R))
		{
			Player._Instance.transform.eulerAngles = Vector3.zero;
			GameData._CanRotateCamera = false; 
		}
		if (Input.GetKeyDown(KeyCode.M))
		{
			UIManager._Instance.SetMaskEnable(!GameData._isOpenMask); 
		}
		if (Input.GetKeyDown(KeyCode.C))
		{
			UIManager._Instance.Open(EView.End); 
		}
		if (Input.GetKeyDown(KeyCode.V))
		{
			UIManager._Instance.Open(EView.Win); 
		}

		if (Input.GetKeyDown(KeyCode.Delete))
		{
			PlayerPrefs.DeleteAll(); 
			Debug.LogError("Delete all"); 
//			var playerPos = Player._Instance.GetPos(); 
//			Singleton._archiveManager.SaveObj(999, Player._Instance._playerHurt._curHP, MapManager.CurIndex(playerPos._x, playerPos._y), GameData._CurLevel); 
		}


		if (Input.GetKeyDown(KeyCode.P))
		{
			// 存储玩家信息
			var playerPos = Player._Instance.GetPos(); 
			Singleton._archiveManager.SavePlayer(ConstValue._playerId, GameData._CurLevel, Player._Instance._playerHurt._curHP, MapManager.CurIndex(playerPos._x, playerPos._y)); 
		}

		if (Input.GetKeyDown(KeyCode.F))
		{	
			Singleton._archiveManager.SaveFloor(); 
		}
	}
	#endif
}
