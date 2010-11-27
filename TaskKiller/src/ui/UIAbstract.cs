using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace View
{
    public abstract class UIAbstract
    {
        public abstract void manUpdate(object sender);
        public abstract void onTaskChange(object sender, int state);
        public abstract void enable();
        public abstract void disable();
    }
}
