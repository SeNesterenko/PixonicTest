using Cinemachine;
using Events;
using UnityEngine;

namespace PlayerManagement
{
    public class PlayerScroller : MonoBehaviour
    {
        [SerializeField] private float _scrollSpeed = 20f;
        [SerializeField] private CinemachineVirtualCamera _playerCamera;
        [SerializeField] private float _minFieldView = 5f;
        [SerializeField] private float _maxFieldView = 10000f;
        [SerializeField] private float _distanceCameraForSpecialMode = 30f;

        private bool _isSpecialMode;

        public void Scroll(float mouseScroll)
        {
            if (mouseScroll == 0
                || mouseScroll < 0 && _playerCamera.m_Lens.OrthographicSize >= _maxFieldView
                || mouseScroll > 0 && _playerCamera.m_Lens.OrthographicSize <= _minFieldView)
            {
                return;
            }

            _playerCamera.m_Lens.OrthographicSize += -mouseScroll * _scrollSpeed;

            if (_playerCamera.m_Lens.OrthographicSize * 2 >= _distanceCameraForSpecialMode && !_isSpecialMode)
            {
                _isSpecialMode = true;
            }
            else if (_playerCamera.m_Lens.OrthographicSize * 2 < _distanceCameraForSpecialMode && _isSpecialMode)
            {
                _isSpecialMode = false;
            }

            if (_isSpecialMode)
            {
                EventStreams.Game.Publish(new SpecialModeActivatedEvent());
            }
            else
            {
                EventStreams.Game.Publish(new NormalModeActivatedEvent());
            }
        }
        
        private void Start()
        {
            _minFieldView /= 2;
            _maxFieldView /= 2;
        }
    }
}