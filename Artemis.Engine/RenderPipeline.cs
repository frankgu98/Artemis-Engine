using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;



namespace Artemis.Engine
{
    /// <summary>
    /// RenderPipeline Class that draws using the Monogame tools.
    /// Having this allows us to keep the user away from Monogame and simplify some common drawing procedures
    /// </summary>
    public class RenderPipeline
    {
        public GraphicsDevice GraphicsDevice { get; private set; } // GraphicsDevice that will be drawn on
        public GraphicsDeviceManager GraphicsDeviceManager { get; private set; } // Mostly useless
        internal bool BegunRenderCycle { get; set; } // If the user has started a RenderCycle
        private bool setRenderProperties { get; set; } //If the user has setRenderProperties (ie begun a SpriteBatch)

        private SpriteBatch SpriteBatch; //SpriteBatch that draws everything

        /// <summary>
        /// Constructs a RenderPipeline with everything a user might need to draw things
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="gd"></param>
        /// <param name="gdm"></param>
        public RenderPipeline(SpriteBatch sb, GraphicsDevice gd, GraphicsDeviceManager gdm)
        {
            SpriteBatch = sb;
            GraphicsDevice = gd;
            GraphicsDeviceManager = gdm;
        }

        /// <summary>
        /// The main rendering function the user will use to make graphics appear onscreen
        /// </summary>
        /// <param name="texture"></param>
        /// <param name="position"></param>
        /// <param name="sourceRectangle"></param>
        /// <param name="color"></param>
        /// <param name="rotation"></param>
        /// <param name="origin"></param>
        /// <param name="scale"></param>
        /// <param name="effects"></param>
        /// <param name="layerDepth"></param>
        public void Render(Texture2D texture,
                           Vector2 position,
                           Rectangle? sourceRectangle = null,
                           Color? color = null,
                           float rotation = 0,
                           Vector2? origin = null,
                           Vector2? scale = null,
                           SpriteEffects effects = SpriteEffects.None,
                           float layerDepth = 0)
        {
            //consider catching exceptions instead
            if (BegunRenderCycle)
            {
                if (setRenderProperties)
                {
                    SpriteBatch.Draw(texture,
                                     position,
                                     sourceRectangle,
                                     color == null ? Color.White : (Color)color,
                                     rotation,
                                     origin == null ? Vector2.Zero : (Vector2)origin,
                                     scale == null ? Vector2.Zero : (Vector2)scale,
                                     effects,
                                     layerDepth);
                }
                else
                {
                    throw new RenderPropertiesNotSetException("You must set the RenderProperties before attempting to draw");
                }
            }
            else
            {
                throw new RenderCycleNotBegunException("You must begin the RenderCycle before attempting to draw");
            }
        }

        /// <summary>
        /// How the user defines how they want things to be rendered
        /// </summary>
        /// <param name="ssm"></param>
        /// <param name="bs"></param>
        /// <param name="ss"></param>
        /// <param name="dss"></param>
        /// <param name="rs"></param>
        /// <param name="e"></param>
        /// <param name="m"></param>
        public void SetRenderProperties(SpriteSortMode ssm = SpriteSortMode.Deferred,
                                        BlendState bs = null,
                                        SamplerState ss = null,
                                        DepthStencilState dss = null,
                                        RasterizerState rs = null,
                                        Effect e = null,
                                        Matrix? m = null)
        {
            if (setRenderProperties)
            {
                throw new RenderPropertiesAlreadySet("You must clear the RenderProperties before attempting to set them again");
            }
            else
            {
                SpriteBatch.Begin(ssm, bs, ss, dss, rs, e, m);
                setRenderProperties = true;
            }
            
 
        }

        /// <summary>
        /// How the user ends how they've been drawing things so far
        /// </summary>
        public void ClearRenderProperties()
        {
            if (setRenderProperties)
            {
                SpriteBatch.End();
                setRenderProperties = false;
            }
            else
            {
                throw new RenderPropertiesNotSetException("You must set the RenderProperties before attempting to clear them");
            }
            
        }

    }

    /// <summary>
    /// Thrown when the user tries to do something outside the RenderCycle that should be done within the RenderCycle
    /// </summary>
    public class RenderCycleNotBegunException : Exception
    {
        public RenderCycleNotBegunException()
        {
        }

        public RenderCycleNotBegunException(string message)
            : base(message)
        {
        }

        public RenderCycleNotBegunException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }

    /// <summary>
    /// Thrown when the user tries to do something that requires RenderProperties wthout having chosen any RenderProperties
    /// </summary>
    public class RenderPropertiesNotSetException : Exception
    {
        public RenderPropertiesNotSetException()
        {
        }

        public RenderPropertiesNotSetException(string message)
            : base(message)
        {
        }

        public RenderPropertiesNotSetException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }

    /// <summary>
    /// Thrown when the user tries to set RenderProperties again without having cleared them
    /// </summary>
    public class RenderPropertiesAlreadySet : Exception
    {
        public RenderPropertiesAlreadySet()
        {
        }

        public RenderPropertiesAlreadySet(string message)
            : base(message)
        {
        }

        public RenderPropertiesAlreadySet(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}


