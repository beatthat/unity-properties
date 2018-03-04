﻿using UnityEngine.Events;
using UnityEngine;


namespace BeatThat
{
	
	public class BoolProperty : BoolProp
	{
		public bool m_value; // TODO: this shouldn't be public but good to see in Inspector. Move to editor class.

		override protected bool GetValue() { return m_value; }
		override protected void _SetValue(bool s) { m_value = s; }
	}

	public abstract class BoolProp : HasBool, IHasValueChangedEvent<bool>
	{
		public bool m_debug;
		public bool m_debugBreakOnSetValue;

		public UnityEvent<bool> onValueChanged 
		{ 
			get { return m_onValueChanged?? (m_onValueChanged = new BoolEvent()); } 
			set { m_onValueChanged = value; } 
		}
		[SerializeField]protected UnityEvent<bool> m_onValueChanged;

		override public bool value
		{ 
			get { return GetValue(); }
			set { SetValue(value); } 
		}
			
		override public object valueObj { get { return this.value; } }

		abstract protected bool GetValue();
		abstract protected void _SetValue(bool s);

		override public bool sendsValueObjChanged { get { return true; } }

		protected void SendValueChanged(bool val)
		{
			SendValueObjChanged();
			if(m_onValueChanged != null) {
				m_onValueChanged.Invoke(val);
			}
		}

		public void SetValue(bool val, PropertyEventOptions opts = PropertyEventOptions.SendOnChange)
		{
			#if BT_DEBUG_UNSTRIP || UNITY_EDITOR
			if(m_debug) {
				Debug.Log("[" + Time.frameCount + "][" + this.Path() + "] " + GetType() + "::set_value to " + val);
			}
			#endif

			if(val == GetValue() && opts != PropertyEventOptions.Force) {
				return;
			}

			_SetValue(val);

			#if UNITY_EDITOR
			if(m_debugBreakOnSetValue) {
				Debug.LogWarning("[" + Time.frameCount + "][" + this.Path() + "] " + GetType() + "::set_value to " + val + " BREAK ON SET VALUE is enabled");
				Debug.Break();
			}
			#endif

			if(opts != PropertyEventOptions.Disable) {
				SendValueChanged(val);
			}
		}
	}

}
