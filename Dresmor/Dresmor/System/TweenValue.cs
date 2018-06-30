using Dresmor.Gui;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dresmor.System
{
    public class TweenValue
    {
        private UDim2 to;
        private UDim2 from;
        private Clock ticker;
        private bool overridable;
        private float seconds;

        public Action<BaseGui, TweenValue> Callback;
        public UDim2 To => to;
        public UDim2 From { get => from; }
        public Clock Ticker => ticker;
        public bool Overridable => overridable;
        public float Seconds => seconds;
        public bool Done => ticker?.ElapsedTime.AsSeconds() >= seconds;
        public bool Ready => ticker != null;
        public UDim2 Position => from + (to - from) * Math.Max(0, Math.Min(1, ticker.ElapsedTime.AsSeconds() / seconds));

        public TweenValue Setup(UDim2 from)
        {
            if (ticker != null) return this;
            this.from = from;
            ticker = new Clock();
            return this;
        }

        public TweenValue(UDim2 to, float seconds, bool overridable, Action<BaseGui, TweenValue> callback)
        {
            this.to = to;
            this.seconds = seconds;
            this.overridable = overridable;
            Callback = callback;
        }
    }
}
