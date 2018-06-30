using System;

namespace Dresmor.System
{
    public class DresmorHandler<TEventArgs>
    {
        public event EventHandler<TEventArgs> Handler;

        public void Call(object sender, TEventArgs args) => Handler?.Invoke(sender, args);
        public EventHandler<TEventArgs> Add(EventHandler<TEventArgs> stuff) { Handler += stuff; return stuff; }
        public void Remove(EventHandler<TEventArgs> stuff) => Handler -= stuff;

        static public DresmorHandler<TEventArgs> operator +(DresmorHandler<TEventArgs> a, EventHandler<TEventArgs> b)
        {
            a.Handler += b;
            return a;
        }

        static public DresmorHandler<TEventArgs> operator -(DresmorHandler<TEventArgs> a, EventHandler<TEventArgs> b)
        {
            a.Handler -= b;
            return a;
        }
    }

    public class DresmorHandler: DresmorHandler<EventArgs>
    {
        static public DresmorHandler operator +(DresmorHandler a, EventHandler<EventArgs> b)
        {
            a.Handler += b;
            return a;
        }

        static public DresmorHandler operator -(DresmorHandler a, EventHandler<EventArgs> b)
        {
            a.Handler -= b;
            return a;
        }
    }
}
