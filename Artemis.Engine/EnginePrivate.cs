﻿using Artemis.Engine.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Artemis.Engine
{
    public sealed partial class ArtemisEngine
    {
        // Private instance fields

        internal RenderPipeline   _RenderPipeline   { get; private set; }
        internal MultiformManager _MultiformManager { get; private set; }
        internal GameProperties   _GameProperties   { get; private set; }
        internal GlobalTimer      _GameTimer        { get; private set; }
        internal GlobalUpdater    _GameUpdater      { get; private set; }
        internal MouseInput       _Mouse            { get; private set; }
        internal KeyboardInput    _Keyboard         { get; private set; }

        private GameKernel gameKernel;

        private Action initializer;

        internal bool Initialized { get; private set; }

        private ArtemisEngine(GameProperties properties, Action initializer) : base()
        {
            this.initializer = initializer;
            Initialized = false;

            _GameProperties = properties;
            gameKernel = new GameKernel(this);

            _MultiformManager = new MultiformManager();
            _GameTimer        = new GlobalTimer();
            _GameUpdater      = new GlobalUpdater();

            _Mouse            = new MouseInput();
            _Keyboard         = new KeyboardInput();
        }

        internal void InitializeRenderPipeline(SpriteBatch sb, GraphicsDevice gd, GraphicsDeviceManager gdm)
        {
            _RenderPipeline = new RenderPipeline(sb, gd, gdm);
        }

        internal void Initialize()
        {
            initializer();
            Initialized = true;
        }

        private void Run()
        {
            using (gameKernel)
            {
                gameKernel.Run();
            }
        }

        /// <summary>
        /// The main game loop.
        /// </summary>
        /// <param name="gameTime"></param>
        internal void Update(GameTime gameTime)
        {
            // Update the time first...
            _GameTimer.UpdateTime(gameTime);

            // Then the input...
            _Mouse.Update();
            _Keyboard.Update();

            // Then the multiforms...
            _MultiformManager.Update();

            // And finally all remaining ArtemisObjects.
            _GameUpdater.FinalizeUpdate();
        }

        /// <summary>
        /// The main rendering loop, which gets called after Update.
        /// </summary>
        /// <param name="gameTime"></param>
        internal void Render()
        {
            _RenderPipeline.BeginRenderCycle();

            _MultiformManager.Render();

            _RenderPipeline.EndRenderCycle();
        }

        /// <summary>
        /// Register Multiform classes to the engine's MultiformManager.
        /// </summary>
        /// <param name="multiforms"></param>
        private void _RegisterMultiforms(object[] multiforms)
        {
            var i = 0;
            foreach (var multiform in multiforms)
            {
                if (multiform is Multiform)
                {
                    MultiformManager.RegisterMultiform((Multiform)multiform);
                }
                else if (multiform is Type)
                {
                    MultiformManager.RegisterMultiform((Type)multiform);
                }
                else
                {
                    throw new MultiformRegistrationException(
                        String.Format(
                            "When registering multiforms, the given objects must either be a multiform " +
                            "instance or a multiform type. The object at index {0} was neither (received {1}).",
                            i, multiform));
                }
                i++;
            }
        }
    }
}
