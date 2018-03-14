using System.Collections.Generic;
using UnityEngine;

namespace Assets
{
    public class InputHelper : MonoBehaviour
    {
        public static bool Up()
        {
            return Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W) || _swipeUp;
        }

        public static bool Down()
        {
            return Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S) || _swipeDown;
        }

        public static bool LeftTap()
        {
            return Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A) || _leftTap;
        }

        public static bool LeftDown()
        {
            return Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A) || _leftTouch;
        }

        public static bool RightTap()
        {
            return Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D) || _rightTap;
        }

        public static bool RightDown()
        {
            return Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D) || _rightTouch;
        }

        public float MinSwipeDist;
        private Vector3 _startPos;
        private Vector3 _endPos;
        private float _swipeDistance;

        private static bool _swipeUp;
        private static bool _swipeDown;

        private static bool _leftTap;
        private static bool _rightTap;

        private static bool _leftTouch;
        private static bool _rightTouch;

        private readonly Dictionary<int, float> _touchTime = new Dictionary<int, float>();

        private Bounds _cameraBounds;

        void Start()
        {
            float screenAspect = (float)Screen.width / Screen.height;
            float cameraHeight = Camera.main.orthographicSize * 2;
            _cameraBounds = new Bounds(
                Camera.main.transform.position,
                new Vector3(cameraHeight * screenAspect, cameraHeight, 0));
        }

        void Update()
        {
            _swipeUp = false;
            _swipeDown = false;
            _leftTap = false;
            _rightTap = false;
            _leftTouch = false;
            _rightTouch = false;

            for (var i = 0; i < Input.touchCount; i++)
            {
                var touch = Input.GetTouch(i);
                if (touch.phase == TouchPhase.Began)
                {
                    _touchTime.Add(touch.fingerId, Time.time);
                    TouchStart(touch.position);
                }

                if (touch.phase == TouchPhase.Ended)
                {
                    var startTime = _touchTime[touch.fingerId];
                    if (Mathf.Abs(Time.time - startTime) < 0.5f)
                        TouchEnd(touch.position);
                    _touchTime.Remove(touch.fingerId);
                }
                    
                if (touch.phase == TouchPhase.Stationary || touch.phase == TouchPhase.Moved && (_rightTouch || _leftTouch))
                    TouchHold(touch.position);
            }

            if (Input.GetKey(KeyCode.Escape))
                Application.Quit();
        }

        private void TouchHold(Vector3 position)
        {
            var pos = Camera.main.ScreenToWorldPoint(position);
            if (_rightTouch || _leftTouch)
                return;
            _rightTouch = pos.x > _cameraBounds.max.x * 0.2f;
            _leftTouch = pos.x < _cameraBounds.min.x * 0.2f;
        }

        private void TouchStart(Vector3 position)
        {
            _startPos = Camera.main.ScreenToWorldPoint(position);
            _rightTap = _startPos.x > _cameraBounds.max.x * 0.2f;
            _leftTap = _startPos.x < _cameraBounds.min.x * 0.2f;
        }

        private void TouchEnd(Vector3 position)
        {
            _endPos = Camera.main.ScreenToWorldPoint(position);
            _swipeDistance = (_endPos - _startPos).magnitude;
            if (_swipeDistance > 2f)
                SwipeFunc();
        }

        void SwipeFunc()
        {
            Vector2 distance = _endPos - _startPos;
            if (!(Mathf.Abs(distance.y) > 0f))
                return;

            _swipeUp = distance.y > 0f;
            _swipeDown = !_swipeUp;
        }
    }
}
