using System;
using System.Collections;

using UnityEngine;

namespace Framework
{
	[RequireComponent(typeof(BoxCollider2D))]
	public class TouchZone : MonoBehaviour 
	{
		public delegate void TouchDownCallback(Vector3 worldPos);
		public delegate void TouchUpCallback(Vector3 worldPos);
		public delegate void TouchDragCallback(Vector3 worldPos, Vector3 worldDelta);
		public delegate void TouchingCallback(Vector3 worldPos);
		
		public event TouchDownCallback TouchDownEvent;
		public event TouchUpCallback TouchUpEvent;
		public event TouchingCallback TouchingEvent;
		public event TouchDragCallback TouchDragEvent;
		
		public Camera camera;
		
		public Vector2 deadzone;
		
		public bool IsTouchDown { get { return _isTouchDown; } }
		
		private bool _isTouchDown;
		private Vector2 _lastWorldPos;
		
		private const float epsilon = 0.001f;
		
		// Use this for initialization
		void Start () 
		{
		}
		
		void Update()
		{
			if(camera == null) { return; }
			
			#if UNITY_EDITOR
			bool isAnyTouch = Input.GetMouseButton(0);
			#else
			bool isAnyTouch = (Input.touchCount > 0);
			#endif
			
			if(isAnyTouch)
			{
				#if UNITY_EDITOR
				Vector2 tp = camera.ScreenToWorldPoint(Input.mousePosition);
				#else
				Touch touch = Input.GetTouch(0);
				Vector2 tp = camera.ScreenToWorldPoint(touch.position);
				#endif
				
				//Debug.Log("TouchZone: " + tp);
				
				if(this.GetComponent<Collider2D>().OverlapPoint(tp))
				{
					if(!_isTouchDown)
					{
						_isTouchDown = true;
						OnTouchDown(_lastWorldPos);
					}
					
					Vector2 delta = tp - _lastWorldPos;
					
					if(Vector2.Distance(tp, _lastWorldPos) > epsilon)
					{
						if(Mathf.Abs(delta.x) > deadzone.x || Mathf.Abs(delta.y) > deadzone.y)
						{
							_lastWorldPos = tp;
							OnTouchDrag(_lastWorldPos, delta);
						}
					}
				}
			}
			else
			{
				if(_isTouchDown)
				{
					_isTouchDown = false;
					OnTouchUp(_lastWorldPos);
				}
			}
			
			if(_isTouchDown)
			{
				OnTouching(_lastWorldPos);
			}
		}
		
		// Update is called once per frame
		/*
		void Update ()
		{
			if (UICamera.IsPressed(this.gameObject))
			{
				if (!_isTouchOn)
				{
					_isTouchOn = true;

					_touchDownCallback(UICamera.lastWorldPosition);
				}
				else
				{
					_touchMoveCallback(UICamera.lastWorldPosition);
				}
			}
			else if (_isTouchOn)
			{
				_isTouchOn = false;
			}
		}
		*/
		
		/*
		void OnMouseDown()
		{
			_isTouchDown = true;

			Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			_lastWorldPos = worldPos;

			Debug.Log("OnMouseDown screen position: " + Input.mousePosition);
			Debug.Log("OnMouseDown game space position: " + worldPos);

			OnTouchDown(worldPos);
		}

		void OnMouseUp()
		{
			_isTouchDown = false;
			
			OnTouchDown(_lastWorldPos);
		}

		void OnMouseDrag()
		{
			Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			Vector3 worldDelta = _lastWorldPos - worldPos;

			Debug.Log("OnMouseDown screen position: " + Input.mousePosition);
			Debug.Log("OnMouseDown game space position: " + worldPos);

			OnTouchDrag(worldPos, worldDelta);

			_lastWorldPos = worldPos;
		}
		*/
		
		#region Events
		
		private void OnTouchDown(Vector3 worldPos)
		{
			if(TouchDownEvent != null)
			{
				try
				{
					TouchDownEvent(worldPos);
				}
				catch(Exception e)
				{
					Debug.LogException(e);
				}
			}
		}
		
		private void OnTouchUp(Vector3 worldPos)
		{
			if(TouchUpEvent != null)
			{
				try
				{
					TouchUpEvent(worldPos);
				}
				catch(Exception e)
				{
					Debug.LogException(e);
				}
			}
		}
		
		private void OnTouching(Vector3 worldPos)
		{
			if(TouchingEvent != null)
			{
				try
				{
					TouchingEvent(worldPos);
				}
				catch(Exception e)
				{
					Debug.LogException(e);
				}
			}
		}
		
		private void OnTouchDrag(Vector3 worldPos, Vector3 worldDelta)
		{
			if(TouchDragEvent != null)
			{
				try
				{
					TouchDragEvent(worldPos, worldDelta);
				}
				catch(Exception e)
				{
					Debug.LogException(e);
				}
			}
		}
		
		#endregion
	}
}
