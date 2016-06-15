using System;
using System.Collections;

using UnityEngine;
using Framework;

public partial class GameManager : UnitySingleton<GameManager>
{
	public enum BestStatType
	{
		MinValue,
		MaxValue
	}

	public delegate void GameInitCallback();
	public delegate void GameResetCallback();
	public delegate void GameStartCallback();
	public delegate void GamePauseCallback();
	public delegate void GameResumeCallback();
	public delegate void GameOverCallback();
	public delegate void ScoreChangeCallback(int score, int inc);
	public delegate void BestScoreChangeCallback(int bestScore);
	public delegate void TimeChangeCallback(int time, int inc);
	public delegate void BestTimeChangeCallback(int bestTime);
	public delegate void GameSavedCallback();
	public delegate void GameLoadedCallback();

	public event GameInitCallback GameInitEvent;
	public event GameResetCallback GameResetEvent;
	public event GameStartCallback GameStartEvent;
	public event GamePauseCallback GamePauseEvent;
	public event GameResumeCallback GameResumeEvent;
	public event GameOverCallback GameOverEvent;
	public event ScoreChangeCallback ScoreChangeEvent;
	public event BestScoreChangeCallback BestScoreChangeEvent;
	public event TimeChangeCallback TimeChangeEvent;
	public event BestTimeChangeCallback BestTimeChangeEvent;
	public event GameSavedCallback GameSavedEvent;
	public event GameLoadedCallback GameLoadedEvent;

	public bool saveLastScore = false;
	public bool autoLoadLastScore = false;

	public bool autoUpdateBestScore = true;
	public bool autoUpdateBestTime = true;

	public BestStatType bestScoreType = BestStatType.MaxValue; 
	public BestStatType bestTimeType = BestStatType.MaxValue;

	public bool IsRoundRunning { get { return _isRoundRunning; } }
	public bool IsRoundPaused { get { return _isRoundPaused; } }

	private int _score;
	private int _bestScore;

	private int _time;
	private int _bestTime;

	private bool _isRoundRunning;
	private bool _isRoundPaused;

	public void GameInit()
	{
		Initialize();
		OnGameInit();

		OnScoreChange(_score, 0);
		OnBestScoreChange(_bestScore);

		OnTimeChange(_time, 0);
		OnBestTimeChange(_bestTime);
	}

	public void GameReset()
	{
		SetTime(0);

		OnGameReset();
	}

	public void GameStart()
	{
		_isRoundRunning = true;

		OnGameStart();
	}

	public void GamePause()
	{
		_isRoundPaused = true;

		OnGamePause();
	}

	public void GameResume()
	{
		_isRoundPaused = false;
		
		OnGameResume();
	}

	public void GameOver()
	{
		_isRoundRunning = false;

		OnGameOver();
	}

	public int GetBestScore()
	{
		return _bestScore;
	}

	public int GetBestTime()
	{
		return _bestTime;
	}

	public void SetBestScore(int bestScore)
	{
		_bestScore = bestScore;

		OnBestScoreChange(bestScore);
	}

	public void SetBestTime(int bestTime)
	{
		_bestTime = bestTime;

		OnBestTimeChange(bestTime);
	}

	public void UpdateBestScore()
	{
		switch(bestScoreType)
		{
			case BestStatType.MinValue:
				if(_score < _bestScore)
				{
					SetBestScore(_score);
				}
				break;

			case BestStatType.MaxValue:
				if(_score > _bestScore)
				{
					SetBestScore(_score);
				}
				break;
		}
	}

	public void UpdateBestTime()
	{
		switch(bestTimeType)
		{
		case BestStatType.MinValue:
			if(_time < _bestTime)
			{
				SetBestTime(_time);
			}
			break;
			
		case BestStatType.MaxValue:
			if(_time > _bestTime)
			{
				SetBestTime(_time);
			}
			break;
		}
	}

	public int GetScore()
	{
		return _score;
	}

	public void SetScore(int score)
	{
		_score = score;

		if(autoUpdateBestScore)
		{
			UpdateBestScore();
		}

		OnScoreChange(_score, 0);
	}

	public int AddScore(int inc)
	{
		_score += inc;

		OnScoreChange(_score, inc);

		if(autoUpdateBestScore)
		{
			UpdateBestScore();
		}

		return _score;
	}

	public int GetTime()
	{
		return _time;
	}

	public void SetTime(int time)
	{
		_time = time;

		if(autoUpdateBestScore)
		{
			UpdateBestTime();
		}
		
		OnTimeChange(_time, 0);
	}

	public int AddTime(int inc)
	{
		_time += inc;

		OnTimeChange(_time, inc);
		
		if(autoUpdateBestTime)
		{
			UpdateBestTime();
		}
		
		return _time;
	}

	public void SaveStats()
	{
		if(saveLastScore)
		{
			PlayerPrefs.SetInt(FrameworkConstants.KEY_SCORE, _score);
			PlayerPrefs.SetInt(FrameworkConstants.KEY_TIME, _time);
		}

		PlayerPrefs.SetInt(FrameworkConstants.KEY_BESTSCORE, _bestScore);
		PlayerPrefs.SetInt(FrameworkConstants.KEY_BESTTIME, _bestTime);

		PlayerPrefs.Save();
	}

	public void SaveGame(string gameData)
	{
		PlayerPrefs.SetString(FrameworkConstants.KEY_GAMESAVE, gameData);
		PlayerPrefs.Save();

		OnGameSaved();
	}

	public void LoadGame()
	{
		string gameData = PlayerPrefs.GetString(FrameworkConstants.KEY_GAMESAVE, null);

		// TODO: Load Game

		OnGameLoaded();
	}

	private void Initialize()
	{
		if(autoLoadLastScore)
		{
			_score = PlayerPrefs.GetInt(FrameworkConstants.KEY_SCORE, 0);
			_time = PlayerPrefs.GetInt(FrameworkConstants.KEY_TIME, 0);
		}

		_bestScore = PlayerPrefs.GetInt(FrameworkConstants.KEY_BESTSCORE, 0);
		_bestTime = PlayerPrefs.GetInt(FrameworkConstants.KEY_BESTTIME, 0);
	}

	#region Events

	private void OnGameInit()
	{
		if(GameInitEvent != null)
		{
			try
			{
				GameInitEvent();
			}
			catch(Exception e)
			{
				Debug.LogException(e);
			}	
		}
	}

	private void OnGameReset()
	{
		if(GameResetEvent != null)
		{
			try
			{
				GameResetEvent();
			}
			catch(Exception e)
			{
				Debug.LogException(e);
			}	
		}
	}

	private void OnGameStart()
	{
		if(GameStartEvent != null)
		{
			try
			{
				GameStartEvent();
			}
			catch(Exception e)
			{
				Debug.LogException(e);
			}	
		}
	}

	private void OnGamePause()
	{
		if(GamePauseEvent != null)
		{
			try
			{
				GamePauseEvent();
			}
			catch(Exception e)
			{
				Debug.LogException(e);
			}
		}
	}

	private void OnGameResume()
	{
		if(GameResumeEvent != null)
		{
			try
			{
				GameResumeEvent();
			}
			catch(Exception e)
			{
				Debug.LogException(e);
			}
		}
	}

	private void OnGameOver()
	{
		if(GameOverEvent != null)
		{
			try
			{
				GameOverEvent();
			}
			catch(Exception e)
			{
				Debug.LogException(e);
			}
		}
	}

	private void OnScoreChange(int score, int inc)
	{
		if(ScoreChangeEvent != null)
		{
			try
			{
				ScoreChangeEvent(score, inc);
			}
			catch(Exception e)
			{
				Debug.LogException(e);
			}	
		}
	}

	private void OnBestScoreChange(int bestScore)
	{
		if(BestScoreChangeEvent != null)
		{
			try
			{
				BestScoreChangeEvent(bestScore);
			}
			catch(Exception e)
			{
				Debug.LogException(e);
			}	
		}
	}

	private void OnTimeChange(int time, int inc)
	{
		if(TimeChangeEvent != null)
		{
			try
			{
				TimeChangeEvent(time, inc);
			}
			catch(Exception e)
			{
				Debug.LogException(e);
			}	
		}
	}

	private void OnBestTimeChange(int bestTime)
	{
		if(BestTimeChangeEvent != null)
		{
			try
			{
				BestTimeChangeEvent(bestTime);
			}
			catch(Exception e)
			{
				Debug.LogException(e);
			}	
		}
	}

	private void OnGameSaved()
	{
		if(GameSavedEvent != null)
		{
			try
			{
				GameSavedEvent();
			}
			catch(Exception e)
			{
				Debug.LogException(e);
			}	
		}
	}

	private void OnGameLoaded()
	{
		if(GameLoadedEvent != null)
		{
			try
			{
				GameLoadedEvent();
			}
			catch(Exception e)
			{
				Debug.LogException(e);
			}	
		}
	}

	#endregion
}