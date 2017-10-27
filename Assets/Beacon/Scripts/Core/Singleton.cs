using UnityEngine;
using System.Collections;

public static class Singleton
{
	public static LevelManager _levelManager; 
	public static NPCManager _npcManager; 
	public static PathFindSystem _pathFindystem; 

	public static void Init()
	{
		_levelManager = new LevelManager(); 
		_npcManager = new NPCManager(); 
		_pathFindystem = new PathFindSystem(); 
	}

	public static void Clear()
	{
		_npcManager = null; 
		_levelManager = null; 
	}
}
