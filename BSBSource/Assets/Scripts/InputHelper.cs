using Lean.Touch;
using UnityEngine;

namespace Assets
{
    public class InputHelper : MonoBehaviour
    {
        public static bool Up()
        {
            return  _swipeUp;
        }

        public static bool Down()
        {
            return _swipeDown;
        }

        public static bool LeftTap()
        {
            return  _leftTap;
        }

        public static bool LeftDown()
        {
            return  _leftTouch;
        }

        public static bool RightTap()
        {
            return  _rightTap;
        }

        public static bool RightDown()
        {
            return  _rightTouch;
        }

        private Vector3 _startPos;
        private Vector3 _endPos;

        private static bool _swipeUp;
        private static bool _swipeDown;

        private static bool _leftTap;
        private static bool _rightTap;

        private static bool _leftTouch;
        private static bool _rightTouch;

        private Bounds _cameraBounds;

        void Start()
        {
            float screenAspect = (float)Screen.width / Screen.height;
            float cameraHeight = Camera.main.orthographicSize * 2;
            _cameraBounds = new Bounds(
                Camera.main.transform.position,
                new Vector3(cameraHeight * screenAspect, cameraHeight, 0));

            LeanTouch.OnFingerSwipe += OnSwipe;
            LeanTouch.OnFingerTap += OnFingerTap;
            LeanTouch.OnFingerSet += OnFingerDown;
        }

        void OnFingerDown(LeanFinger finger)
        {
            if (finger.Age < 0.2f)
                return;

            _rightTouch = finger.ScreenPosition.x > Screen.width / 2f;
            _leftTouch = !_rightTouch;
        }

        void OnFingerTap(LeanFinger finger)
        {
            _rightTap = finger.ScreenPosition.x > Screen.width / 2f;
            _leftTap = !_rightTap;
        }

        void OnSwipe(LeanFinger finger)
        {
            _startPos = finger.StartScreenPosition;
            _endPos = finger.ScreenPosition;
            SwipeFunc();
        }

        void LateUpdate()
        {
            _swipeUp = false;
            _swipeDown = false;
            _leftTap = false;
            _rightTap = false;
            _rightTouch = false;
            _leftTouch = false;
        }

        void Update()
        {
            if (Input.GetKey(KeyCode.Escape))
                Application.Quit();
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
