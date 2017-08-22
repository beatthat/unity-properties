﻿using UnityEngine;

namespace BeatThat
{
	public abstract class HasColor : HasValue, IHasColor
	{
		public abstract Color value { get; set; }
	}
}
