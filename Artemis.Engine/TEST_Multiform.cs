using Artemis.Engine.Utilities;
using System;

namespace Artemis.Engine
{
    public abstract class Multiform : ArtemisObject
    {

        /// <summary>
        /// The name of the multiform instance.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// The current renderer for the multiform.
        /// </summary>
        private Action renderer;

        public Multiform()
        {
            var thisType = this.GetType();
            var nameAttributes = Reflection.GetAttributes<NamedMultiform>(thisType);
            if (nameAttributes.Count != 1)
            {
                throw new MultiformException(
                    String.Format(
                        "Anonymous Multiforms may only have one NamedMultiformAttribute. " + 
                        "Multiform with type {0} has {1}.", thisType, nameAttributes.Count)
                        );
            }
            Name = nameAttributes[0].Name;
        }

        public Multiform(string name)
        {
            Name = name;
        }

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
