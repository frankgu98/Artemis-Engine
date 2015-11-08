using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Artemis.Engine
{
    /// <summary>
    /// RenderPipeline Class that draws using the Monogame tools. 
    /// Having this allows us to keep the user away from Monogame 
    /// and simplify some common drawing procedures
    /// </summary>
    public class RenderPipeline
    {

        /// <summary>
        /// GraphicsDevice that will be drawn to.
        /// </summary>
        public GraphicsDevice GraphicsDevice { get; private set; }

        /// <summary>
        /// Mostly useless.
        /// </summary>
        public GraphicsDeviceManager GraphicsDeviceManager { get; private set; }

        /// <summary>
        /// If the user has started a RenderCycle.
        /// </summary>
        internal bool BegunRenderCycle { get; set; }

        /// <summary>
        /// If the spriteBatch has been begun.
        /// </summary>
        private bool spriteBatchBegun { get; set; }

        internal SpriteBatch SpriteBatch; // SpriteBatch that draws everything.

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
        /// Begin the render cycle, in which rendering can take place.
        /// </summary>
        internal void BeginRenderCycle()
        {
            BegunRenderCycle = true;
            spriteBatchBegun = true;
            SpriteBatch.Begin();
        }

        /// <summary>
        /// End the render cycle. This ends the spriteBatch as well.
        /// </summary>
        internal void EndRenderCycle()
        {
            BegunRenderCycle = false;
            spriteBatchBegun = false;
            SpriteBatch.End();
        }

        /// <summary>
        /// Directly render a texture to the screen with the given parameters.
        /// </summary>
        /// <param name="texture"></param>
        /// <param name="position"></param>
        /// <param name="sourceRectangle"></param>
        /// <param name="colour"></param>
        /// <param name="rotation"></param>
        /// <param name="origin"></param>
        /// <param name="scale"></param>
        /// <param name="effects"></param>
        /// <param name="layerDepth"></param>
        public void Render(Texture2D texture
                          , Vector2 position
                          , Rectangle? sourceRectangle = null
                          , Color? colour              = null
                          , float rotation             = 0
                          , Vector2? origin            = null
                          , Vector2? scale             = null
                          , SpriteEffects effects      = SpriteEffects.None
                          , float layerDepth           = 0)
        {
            //consider catching exceptions instead
            if (BegunRenderCycle)
            {
                var _colour = colour.HasValue ? colour.Value : Color.White;
                var _origin = origin.HasValue ? origin.Value : Vector2.Zero;
                var _scale  = scale.HasValue  ? scale.Value  : Vector2.One;

                SpriteBatch.Draw(
                    texture, position, sourceRectangle, _colour, rotation,
                    _origin, _scale, effects, layerDepth
                    );
            }
            else
            {
                throw new RenderPipelineException(
                    "Rendering must occur in the render cycle.");
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
        public void SetRenderProperties( SpriteSortMode ssm    = SpriteSortMode.Deferred
                                       , BlendState bs         = null
                                       , SamplerState ss       = null
                                       , DepthStencilState dss = null
                                       , RasterizerState rs    = null
                                       , Effect e              = null
                                       , Matrix? m             = null)
        {
            if (spriteBatchBegun)
            {
                SpriteBatch.End();
            }
            SpriteBatch.Begin(ssm, bs, ss, dss, rs, e, m);
            spriteBatchBegun = true;
        }

        /// <summary>
        /// How the user ends how they've been drawing things so far
        /// </summary>
        public void ClearRenderProperties()
        {
            if (spriteBatchBegun)
            {
                SpriteBatch.End();
            }
            SpriteBatch.Begin();
        }
    }
}


