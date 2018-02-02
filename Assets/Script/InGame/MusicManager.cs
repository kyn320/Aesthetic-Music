using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
	public static MusicManager instance;

	[HideInInspector]
	public float musicLength = 0f;
	public float musicTime
	{
		get
		{
			return musicSource.time;
		}
		set
		{
			musicSource.time = value;
		}
	}
	public float musicTimeS
	{
		get
		{
			return (float)musicSource.timeSamples / musicSource.clip.frequency;
		}
	}

	private AudioSource musicSource;
	private AudioSource seSource;
	private GameManager g;



	private void Awake()
	{
		instance = this;

		AudioSource[] sources = this.GetComponents<AudioSource>();
		musicSource = sources[0];
		seSource = sources[1];
	}

	private void Start()
	{
		g = GameManager.instance;
	}

	private void Update()
	{
	}

	public void LoadMusic(AudioClip _music)
	{
		musicSource.clip = _music;
		musicLength = _music.length;
	}

	public void Play()
	{
		musicSource.Play();
	}

	public void Stop()
	{
		musicSource.Stop();
	}

	public void Pause()
	{
		musicSource.Pause();
	}

	public void UnPause()
	{
		musicSource.UnPause();
	}
}
