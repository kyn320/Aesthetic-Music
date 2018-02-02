using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUIManager : MonoBehaviour
{
	public static GameUIManager instance;

	public Text comboText;

	private GameManager g;
	private EffectManager e;



	private void Awake()
	{
		instance = this;
	}

	private void Start()
	{
		g = GameManager.instance;
		e = EffectManager.instance;
	}

	private void Update()
	{
		comboText.text = g.combo.ToString();
    }
}
