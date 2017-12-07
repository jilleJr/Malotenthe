﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameGUI
{
	[RequireComponent(typeof(SlowSlider))]
	public class Reloadbar : MonoBehaviour
	{
		[SerializeField, HideInInspector]
		private SlowSlider slider;

		public Image blinkImage;
		public float blinkTime = 0.2f;

		private float blinkTimeRemaining = 0;

		private void Awake()
		{
			slider = GetComponent<SlowSlider>();
		}

		public void Reloading(float reloadTime)
		{
			slider.sliderSpeed = 1 / reloadTime;
			slider.ValuePercentage = 1;
			slider.ValuePercentage = 0;
		}

		public void Reloaded()
		{
			slider.ValuePercentage = 1;
		}

		public void Blink()
		{
			SetBlinkAlpha(1);
			blinkTimeRemaining = blinkTime;
		}

		private void SetBlinkAlpha(float alpha)
		{
			Color color = blinkImage.color;
			color.a = alpha;
			blinkImage.color = color;
		}

		private void Update()
		{
			if (blinkTimeRemaining > 0)
			{
				blinkTimeRemaining = Mathf.Max(0, blinkTimeRemaining - Time.deltaTime);
				SetBlinkAlpha(blinkTimeRemaining/blinkTime);
			}
		}
	}

}