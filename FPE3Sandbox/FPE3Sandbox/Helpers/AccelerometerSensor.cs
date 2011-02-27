using System;
using Microsoft.Devices.Sensors;
using Microsoft.Xna.Framework;

namespace FPE3Sandbox.Helpers
{
    struct AccelerometerReading
    {
        public Vector3 Position{ get; private set;}
        public DateTimeOffset TimeStamp { get; private set; }

        public AccelerometerReading(Vector3 position, DateTimeOffset timeStamp) : this()
        {
            Position = position;
            TimeStamp = timeStamp;
        }
    }
    static class AccelerometerSensor
    {
        private static Accelerometer accelerometer;
        private static Vector3 state;
        private static DateTimeOffset timeStamp;

        public static float MaxValue { get; private set; }
        public static float MinValue { get; private set; }

        static AccelerometerSensor()
        {
            MaxValue = 0.6f;
            MinValue = -MaxValue;
            accelerometer = new Accelerometer();
            accelerometer.ReadingChanged += accelerometer_ReadingChanged;
            accelerometer.Start();
        }

        static void accelerometer_ReadingChanged(object sender, AccelerometerReadingEventArgs e)
        {
            state.X = (float)e.X;
            state.Y = (float)e.Y;
            state.Z = (float)e.Z;
            timeStamp = e.Timestamp;
        }

        public static void Start()
        {
            accelerometer.Start();
        }

        public static void Stop()
        {
            accelerometer.Stop();
        }

        public static AccelerometerReading GetReading()
        {
            return GetReading(DisplayOrientation.Default);
        }
        public static AccelerometerReading GetReading(DisplayOrientation orientation)
        {
            if (orientation == DisplayOrientation.Portrait)
            {
                return new AccelerometerReading(state,timeStamp);
            }
            var landLeft = new Vector3(-state.Y, state.X, state.Z);
            if (orientation == DisplayOrientation.LandscapeLeft || orientation == DisplayOrientation.Default)
            {
                return new AccelerometerReading(landLeft, timeStamp);
            }
            return new AccelerometerReading(landLeft,timeStamp );
        }
    }
}
