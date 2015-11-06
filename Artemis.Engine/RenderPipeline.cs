using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Artemis.Engine
{
    public class RenderPipeline
    {
        public GraphicsDevice GraphicsDevice { get; private set; }
        public GraphicsDeviceManager GraphicsDeviceManager { get; private set; }
        internal bool BegunRenderCycle { get; set; }

        private SpriteBatch SpriteBatch;

        public RenderPipeline(SpriteBatch sb, GraphicsDevice gd, GraphicsDeviceManager gdm)
        {
            SpriteBatch = sb;
            GraphicsDevice = gd;
            GraphicsDeviceManager = gdm;
        }

        public void Render(Texture2D texture, Vector2 position, Rectangle? sourceRectangle, Color color, float rotation, 
            Vector2 origin, Vector2 scale, SpriteEffects effects, float layerDepth)
        {
            if (BegunRenderCycle)
            {
                SpriteBatch.Draw(texture, position, sourceRectangle, color, rotation, origin, scale, effects, layerDepth);
            }
        }


        public void SetRenderProperties(SpriteSortMode ssm, BlendState bs, SamplerState ss, DepthStencilState dss, RasterizerState rs, Effect e, Matrix m)
        {
            SpriteBatch.Begin(ssm,bs,ss,dss,rs,e,m);
        }

        public void ClearRenderProperties()
        {
            SpriteBatch.End();
        }

    }
}
