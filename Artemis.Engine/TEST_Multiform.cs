using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Artemis.Engine
{
    public abstract class Multiform : ArtemisObject
    {

        private Action renderer;

        public abstract void Construct();

        protected void SetRenderer(Action action)
        {
            renderer = action;
        }

        internal void Render()
        {
            renderer();
        }

    }
}
