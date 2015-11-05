using Artemis.Engine.Utilities;
using System;
using System.Collections.Generic;

namespace Artemis.Engine
{
    /// <summary>
    /// A class that records currently registered and active multiforms and controls
    /// updating them, rendering them, and other actions.
    /// </summary>
    public sealed class MultiformManager
    {

        /// <summary>
        /// Return the name of a multiform by it's type.
        /// </summary>
        public static string GetMultiformName(Type multiformType)
        {
            // If the class has a NamedMultiform attribute applied to it, we use
            // the supplied name. Otherwise, the name is simply the name of the class.
            string name;
            var nameAttrs = Reflection.GetAttributes<NamedMultiform>(multiformType);
            if (nameAttrs.Count == 0)
            {
                name = multiformType.Name;
            }
            else if (nameAttrs.Count == 1)
            {
                name = nameAttrs[0].Name;
            }
            else
            {
                throw new MultiformRegistrationException(
                    "Multiforms cannot have multiple NamedMultiformAttributes.");
            }
            return name;
        }

        private class ConstructEvent : MultiformPostUpdateEvent
        {
            string name;
            public ConstructEvent(string name)
            {
                this.name = name;
            }
            public override void Perform(
                Dictionary<string, Multiform> registered, 
                Dictionary<string, Multiform> active)
            {
                if (active.ContainsKey(name))
                {
                    throw new MultiformManagerException(
                        String.Format("Multiform with name {0} has already been constructed.", name));
                }

                var multiform = registered[name];
                multiform.Construct();
                active.Add(name, multiform);
            }
        }

        private class CloseEvent : MultiformPostUpdateEvent
        {
            string name;
            public CloseEvent(string name)
            {
                this.name = name;
            }
            public override void Perform(
                Dictionary<string, Multiform> registered, 
                Dictionary<string, Multiform> active)
            {
                if (!active.ContainsKey(name))
                {
                    throw new MultiformManagerException(
                        String.Format("Multiform with name {0} is not constructed.", name));
                }
                active.Remove(name);
            }
        }

        /// <summary>
        /// The dictionary of all registered multiform instances by name.
        /// </summary>
        private Dictionary<string, Multiform> RegisteredMultiforms = new Dictionary<string, Multiform>();

        /// <summary>
        /// The dictionary of currently active multiforms.
        /// </summary>
        private Dictionary<string, Multiform> ActiveMultiforms = new Dictionary<string, Multiform>();

        /// <summary>
        /// Whether or we are in the middle of updating the currently active multiforms.
        /// </summary>
        private bool Updating = false;
        
        /// <summary>
        /// Whether or not we are in the middle of applying PostUpdateEvents.
        /// </summary>
        private bool ApplyingPostUpdateEvents = false;

        /// <summary>
        /// The list of PostUpdateEvents aggregated during the Update loop. These are
        /// events that in some way or another alter the dictionary of currently active
        /// multiforms, and thus have to be performed after the main update loop.
        /// </summary>
        private List<MultiformPostUpdateEvent> PostUpdateEvents = new List<MultiformPostUpdateEvent>();

        /// <summary>
        /// The list of PostUpdateEvents that have to be queued for the next call to
        /// Update. The reason we keep this list as well is because some PostUpdateEvents
        /// can alter the list of PostUpdateEvents whilst iterating through it, which would
        /// cause a ConcurrentModificationException to get thrown. Thus, when iterating through
        /// said list, we instead aggregate a list of newly added PostUpdateEvents and wait for
        /// the next call to Update to apply them.
        /// </summary>
        private List<MultiformPostUpdateEvent> PostUpdateEventQueue = new List<MultiformPostUpdateEvent>();

        public MultiformManager() { }

        /// <summary>
        /// Register a multiform instance with the given name.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="instance"></param>
        internal void RegisterMultiform(string name, Multiform instance)
        {
            RegisteredMultiforms.Add(name, instance);
        }

        private void ApplyOrQueueEvent(MultiformPostUpdateEvent evt)
        {
            if (Updating)
            {
                PostUpdateEvents.Add(evt);
            }
            else if (ApplyingPostUpdateEvents)
            {
                PostUpdateEventQueue.Add(evt);
            }
            else
            {
                // Perform it immediately if we're not updating or applying PostUpdateEvents.
                evt.Perform(RegisteredMultiforms, ActiveMultiforms);
            }
        }

        /// <summary>
        /// Construct the multiform of the given type.
        /// </summary>
        /// <param name="multiformType"></param>
        public void Construct(Type multiformType)
        {
            Construct(GetMultiformName(multiformType));
        }

        /// <summary>
        /// Construct the multiform with the given name.
        /// </summary>
        /// <param name="name"></param>
        public void Construct(string name)
        {
            ApplyOrQueueEvent(new ConstructEvent(name));
        }

        /// <summary>
        /// Close the multiform of the given type.
        /// </summary>
        /// <param name="multiformType"></param>
        public void Close(Type multiformType)
        {
            Close(GetMultiformName(multiformType));
        }

        /// <summary>
        /// Close the multiform with the given name.
        /// </summary>
        /// <param name="name"></param>
        public void Close(string name)
        {
            ApplyOrQueueEvent(new CloseEvent(name));
        }

        /// <summary>
        /// Update all the multiforms.
        /// </summary>
        internal void Update()
        {
            Updating = true;

            foreach (var kvp in ActiveMultiforms)
            {
                kvp.Value.Update();
            }

            Updating = false;

            // Apply PostUpdateEvents.

            ApplyingPostUpdateEvents = true;

            foreach (var evt in PostUpdateEvents)
            {
                evt.Perform(RegisteredMultiforms, ActiveMultiforms);
            }
            PostUpdateEvents.Clear();

            ApplyingPostUpdateEvents = false;

            // Add the queued PostUpdate events to the list of PostUpdateEvents to be performed 
            // next time Update is called. The reason we need this is because there are certain 
            // post update events that can alter the PostUpdateEvents list whilst iterating. For 
            // example, a PostUpdateEvent can Construct a multiform, which can in turn call something 
            // that adds a PostUpdateEvent to the list.
            //
            // So to avoid 

            foreach (var queuedEvt in PostUpdateEventQueue)
            {
                PostUpdateEvents.Add(queuedEvt);
            }

            PostUpdateEventQueue.Clear();
        }

        /// <summary>
        /// Render all the multiforms.
        /// </summary>
        internal void Render()
        {
            foreach (var kvp in ActiveMultiforms)
            {
                kvp.Value.Render();
            }
        }
    }
}
