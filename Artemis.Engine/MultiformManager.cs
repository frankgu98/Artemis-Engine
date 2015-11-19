﻿using Artemis.Engine.Utilities;
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
                        String.Format("Multiform with name '{0}' has already been constructed.", name));
                }

                if (!registered.ContainsKey(name))
                {
                    throw new MultiformManagerException(
                        String.Format("No multiform with name '{0}' exists.", name));
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
                        String.Format("Multiform with name '{0}' has not been constructed.", name));
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
        internal void RegisterMultiform(Multiform instance)
        {
            RegisteredMultiforms.Add(instance.Name, instance);
        }

        /// <summary>
        /// Register a multiform with the given type.
        /// </summary>
        /// <param name="multiformType"></param>
        internal void RegisterMultiform(Type multiformType)
        {
            if (!typeof(Multiform).IsAssignableFrom(multiformType))
            {
                throw new MultiformRegistrationException(
                    String.Format(
                        "The given multiform type {0} does not inherit from Multiform.", multiformType)
                        );
            }
            RegisterMultiform((Multiform)Activator.CreateInstance(multiformType));
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
        /// Construct the multiform with the given name.
        /// </summary>
        /// <param name="name"></param>
        public void Construct(string name)
        {
            ApplyOrQueueEvent(new ConstructEvent(name));
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
