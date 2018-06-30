using Dresmor.System;
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dresmor.Gui
{
    public class ClockGui : BaseGui
    {
        private Simple hoursFinger = new Simple();
        private Simple minutesFinger = new Simple();
        private Simple secondsFinger = new Simple();
        private UDim2 hoursFingerSize = new UDim2(0.03f, 1, 0.7f / 2.0f, 0);
        private UDim2 minutesFingerSize = new UDim2(0.02f, 1, 0.8f / 2.0f, 0);
        private UDim2 secondsFingerSize = new UDim2(0.01f, 1, 0.9f / 2.0f, 0);
        private Clock ticker = new Clock();
        private float precision = 0.01f;
        private float timeClock = 14.0f;
        private float timeHours = 24.0f;
        private float timeScale = 1.0f;
        private bool requireUpdateFingers = true;

        public Simple HoursFinger => hoursFinger;
        public Simple MinutesFinger => minutesFinger;
        public Simple SecondsFinger => secondsFinger;
        public Clock Ticker => ticker;
        public UDim2 HoursFingerSize { get => hoursFingerSize; set { hoursFingerSize = value; requireUpdateFingers = true; } }
        public UDim2 MinutesFingerSize { get => minutesFingerSize; set { minutesFingerSize = value; requireUpdateFingers = true; } }
        public UDim2 SecondsFingerSize { get => secondsFingerSize; set { secondsFingerSize = value; requireUpdateFingers = true; } }
        public float Precision { get => precision; set { precision = value; requireUpdateFingers = true; } }
        public float TimeClock { get { UpdateTimeClock();  return timeClock; } set { timeClock = value; requireUpdateFingers = true; } }
        public float TimeScale { get => timeScale; set { timeScale = value; requireUpdateFingers = true; } }
        public bool RequireUpdateFigers { get => requireUpdateFingers; set => requireUpdateFingers = value; }
        public float TimeHours { get => timeHours; set { timeHours = value; requireUpdateFingers = true; } }

        // Private Methods
        private void UpdateTimeClock()
        {
            int prevTimeClock = (int) Math.Floor((timeClock * 3600.0f) / precision);
            timeClock = (timeClock + (ticker.ElapsedTime.AsSeconds() / 3600.0f) * timeScale) % timeHours;
            int nextTimeClock = (int) Math.Floor((timeClock * 3600.0f) / precision);
            if (Math.Abs(prevTimeClock - nextTimeClock) > Single.Epsilon) requireUpdateFingers = true;
            ticker.Restart();
        }

        private void UpdateFingers()
        {
            foreach (var a in new KeyValuePair<Simple, KeyValuePair<UDim2, float>>[] {
                new KeyValuePair<Simple, KeyValuePair<UDim2, float>>(hoursFinger, new KeyValuePair<UDim2, float>(hoursFingerSize,
                (
                Math.Floor(timeClock / precision) * precision).ToFloat() / timeHours
                )
                ),
                new KeyValuePair<Simple, KeyValuePair<UDim2, float>>(minutesFinger, new KeyValuePair<UDim2, float>(minutesFingerSize,

                (Math.Floor(timeClock * 60.0f / precision) * precision / 60.0f).ToFloat() % 1.0f

                )
                ),
                new KeyValuePair<Simple, KeyValuePair<UDim2, float>>(secondsFinger, new KeyValuePair<UDim2, float>(secondsFingerSize,
                ((Math.Floor(timeClock * 3600.0f / precision) * precision).ToFloat() % 60) / 60.0f
                )
                )
            })
            {
                a.Key.Size = a.Value.Key.Offset + Vec2.Multiply(a.Value.Key.Scale, Body.StrictSize);
                a.Key.Position = Body.StrictSize / 2.0f;
                a.Key.Origin = new Vector2f(a.Key.StrictSize.X / 2.0f, 0.0f);
                a.Key.Rotation = -180.0f + a.Value.Value * 360.0f;
            }
        }

        // Public Methods
        public override void Draw(RenderTarget target, RenderStates states)
        {
            UpdateTimeClock();
            base.Draw(target, states);
            if (requireUpdateFingers)
            {
                requireUpdateFingers = false;
                UpdateFingers();
            }
            states.Transform *= Transform;
            target.Draw(hoursFinger, states);
            target.Draw(minutesFinger, states);
            target.Draw(secondsFinger, states);
        }

        // Constructor
        public ClockGui()
        {
            OutlineColor = Color.Black;
            OutlineThickness = 2.0f;
            ShapeType = ShapeTypes.Circle;

            hoursFinger.FillColor = Color.Black;
            hoursFinger.ShapeType = ShapeTypes.Rectangle;
            minutesFinger.FillColor = Color.Black;
            minutesFinger.ShapeType = ShapeTypes.Rectangle;
            secondsFinger.FillColor = Color.Red;
            secondsFinger.ShapeType = ShapeTypes.Rectangle;

            AbsoluteSizeChanged += (s, e) => requireUpdateFingers = true;
        }
    }
}
