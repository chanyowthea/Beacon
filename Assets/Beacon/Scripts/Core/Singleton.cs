using UnityEngine;
using System.Collections;

public static class Singleton
{
	public static LevelManager _levelManager; 
	public static NPCManager _npcManager; 
	public static PathFindSystem _pathFindystem; 
	public static ArchiveManager _archiveManager; 
	public static InputManager _inputManager; 

	public static void Init()
	{
		_levelManager = new LevelManager(); 
		_npcManager = new NPCManager(); 
		_pathFindystem = new PathFindSystem(); 
		_archiveManager = new ArchiveManager(); 
	}

	public static void Clear()
	{
		_npcManager = null; 
		_levelManager = null; 
		_pathFindystem = null; 
		_archiveManager = null; 
	}
}
