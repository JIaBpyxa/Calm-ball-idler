using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using Gyroscope = UnityEngine.InputSystem.Gyroscope;

namespace Vorval.CalmBall.Service
{
    public class GyroService : MonoBehaviour
    {
        [SerializeField] private bool _isDebug = false;
        [SerializeField] private TextMeshProUGUI _debugText;

        public Vector3 AngularVelocity { get; private set; } = Vector3.zero;
        public Vector3 Acceleration { get; private set; } = Vector3.down;
        public Vector3 Attitude { get; private set; } = new(0, 90f, 90f);
        public Vector3 Gravity { get; private set; } = Vector3.down;

        private bool _isGyroscopeEnabled;
        private bool _isAccelerometerEnabled;
        private bool _isAttitudeEnabled;
        private bool _isGravityEnabled;

        private void Start()
        {
#if !UNITY_EDITOR
            InputSystem.EnableDevice(Gyroscope.current);
            InputSystem.EnableDevice(Accelerometer.current);
            InputSystem.EnableDevice(AttitudeSensor.current);
            InputSystem.EnableDevice(GravitySensor.current);

            _isGyroscopeEnabled = Gyroscope.current.enabled;
            _isAccelerometerEnabled = Accelerometer.current.enabled;
            _isAttitudeEnabled = AttitudeSensor.current.enabled;
            _isGravityEnabled = GravitySensor.current.enabled;
#endif

            _debugText.gameObject.SetActive(_isDebug);
        }

        private void Update()
        {
            //if (_isGyroscopeEnabled) AngularVelocity = Gyroscope.current.angularVelocity.ReadValue();
            //if (_isAccelerometerEnabled) Acceleration = Accelerometer.current.acceleration.ReadValue();
            //if (_isAttitudeEnabled) Attitude = AttitudeSensor.current.attitude.ReadValue().eulerAngles;
            if (_isGravityEnabled) Gravity = GravitySensor.current.gravity.ReadValue();

            if (_isDebug)
            {
                _debugText.text =
                    $"Ang vel {AngularVelocity} \nAccel {Acceleration} \n Attitude {Attitude} \n Gravity {Gravity}";
            }
        }
    }
}