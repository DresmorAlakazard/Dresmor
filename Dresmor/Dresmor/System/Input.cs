using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dresmor.System
{
    public class Input
    {
        public enum Types
        {
            Mouse,
            Keyboard,
            Touch,
            Joystick,
            Unkwown
        }

        public virtual Types Type => Types.Unkwown;
    }
}
