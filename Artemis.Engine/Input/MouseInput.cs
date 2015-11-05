using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Linq;

namespace Artemis.Engine.Input
{
    /// <summary>
    /// The main mouse input provider class.
    /// </summary>
    internal sealed class MouseInput
    {

        /// <summary>
        /// The number of frames passed since a mouse button was initially pressed, 
        /// or zero if the button is released.
        /// </summary>
        public int[] FramesSinceButtonPressed { get; private set; }

        /// <summary>
        /// The number of frames passed since a mouse button was initially released, 
        /// or zero if the button is pressed.
        /// </summary>
        public int[] FramesSinceButtonReleased { get; private set; }

        /// <summary>
        /// The number of frames the mouse has been still for.
        /// </summary>
        public int FramesMouseStill { get; private set; }

        /// <summary>
        /// A list of the previous mouse positions (stored for 60 frames).
        /// </summary>
        public readonly List<Point> PreviousPositions = new List<Point>();

        /// <summary>
        /// The previous mouse position.
        /// </summary>
        public Point PreviousPosition { get { return PreviousPositions.Last(); } }

        /// <summary>
        /// The previous mouse position as a Vector2 instance.
        /// </summary>
        public Vector2 PreviousPositionVector
        {
            get
            {
                var last = PreviousPosition;
                return new Vector2(last.X, last.Y);
            }
        }

        /// <summary>
        /// The position of the mouse.
        /// </summary>
        public Point Position { get; private set; }

        /// <summary>
        /// The position of the mouse as a Vector2 instance.
        /// </summary>
        public Vector2 PositionVector { get { return new Vector2(Position.X, Position.Y); } }

        /// <summary>
        /// The difference in mouse position between this frame and the previous frame.
        /// </summary>
        public Vector2 DeltaPosition
        {
            get
            {
                return PositionVector - PreviousPositionVector;
            }
        }

        /// <summary>
        /// The velocity of the mouse's movement.
        /// </summary>
        public float MouseVelocity
        {
            get
            {
                return DeltaPosition.Length();
            }
        }

        public MouseInput()
        {
            FramesSinceButtonPressed  = new int[] { 0, 0, 0 };
            FramesSinceButtonReleased = new int[] { 0, 0, 0 };
        }

        /// <summary>
        /// Update the internal state of the mouse.
        /// </summary>
        internal void Update()
        {
            PreviousPositions.Add(Position);
            if (PreviousPositions.Count > 60)
            {
                PreviousPositions.RemoveAt(0);
            }

            var state = Mouse.GetState();
            Position = state.Position;

            UpdateButton(state.LeftButton, MouseButton.Left);
            UpdateButton(state.MiddleButton, MouseButton.Middle);
            UpdateButton(state.RightButton, MouseButton.Right);

            if (DeltaPosition == Vector2.Zero)
            {
                FramesMouseStill++;
            }
            else
            {
                FramesMouseStill = 0;
            }
        }

        private void UpdateButton(ButtonState state, MouseButton button)
        {
            var index = (int)button;
            if (state == ButtonState.Pressed)
            {
                FramesSinceButtonPressed[index]++;
                FramesSinceButtonReleased[index] = 0;
            }
            else
            {
                FramesSinceButtonPressed[index] = 0;
                FramesSinceButtonReleased[index]++;
            }
        }

        /// <summary>
        /// Check if a button is clicked (i.e. pressed for exactly 1 frame).
        /// </summary>
        /// <param name="button"></param>
        /// <returns></returns>
        public bool IsClicked(MouseButton button)
        {
            return FramesSinceButtonPressed[(int)button] == 1;
        }

        /// <summary>
        /// Check if a button is being held down.
        /// </summary>
        /// <param name="button"></param>
        /// <returns></returns>
        public bool IsHeld(MouseButton button)
        {
            return FramesSinceButtonPressed[(int)button] > 0;
        }

        /// <summary>
        /// Check if a button has been held down for exactly the given number of frames.
        /// </summary>
        /// <param name="button"></param>
        /// <param name="frames"></param>
        /// <returns></returns>
        public bool IsHeldFor(MouseButton button, int frames)
        {
            return FramesSinceButtonPressed[(int)button] == frames;
        }

        /// <summary>
        /// Check if a button has been held down for at least the given number of frames.
        /// </summary>
        public bool IsHeldForAtleast(MouseButton button, int frames)
        {
            return FramesSinceButtonPressed[(int)button] >= frames;
        }

        /// <summary>
        /// Check if a button has been released (i.e. unpressed for exactly 1 frame).
        /// </summary>
        /// <param name="button"></param>
        /// <returns></returns>
        public bool IsReleased(MouseButton button)
        {
            return FramesSinceButtonReleased[(int)button] == 1;
        }

        /// <summary>
        /// Check if a button is not being held down.
        /// </summary>
        /// <param name="button"></param>
        /// <returns></returns>
        public bool IsUnheld(MouseButton button)
        {
            return FramesSinceButtonReleased[(int)button] > 0;
        }

        /// <summary>
        /// Check if a button has been left unpressed for exactly the given number of frames.
        /// </summary>
        /// <param name="button"></param>
        /// <param name="frames"></param>
        /// <returns></returns>
        public bool IsUnheldFor(MouseButton button, int frames)
        {
            return FramesSinceButtonReleased[(int)button] == frames;
        }

        /// <summary>
        /// Check if a button has been left unpressed for at least the given number of frames.
        /// </summary>
        /// <param name="button"></param>
        /// <param name="frames"></param>
        /// <returns></returns>
        public bool IsUnheldForAtleast(MouseButton button, int frames)
        {
            return FramesSinceButtonReleased[(int)button] >= frames;
        }

        /// <summary>
        /// Check if the mouse is still.
        /// </summary>
        /// <returns></returns>
        public bool IsMouseStill()
        {
            return DeltaPosition == Vector2.Zero;
        }

        /// <summary>
        /// Check if the mouse has been still for exactly the given number of frames.
        /// </summary>
        /// <param name="frames"></param>
        /// <returns></returns>
        public bool IsMouseStillFor(int frames)
        {
            return FramesMouseStill == frames;
        }

        /// <summary>
        /// Check if the mouse has been still for at least the given number of frames.
        /// </summary>
        /// <param name="frames"></param>
        /// <returns></returns>
        public bool IsMouseStillForAtleast(int frames)
        {
            return FramesMouseStill >= frames;
        }
    }
}
