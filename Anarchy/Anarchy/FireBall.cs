using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Anarchy
{
    public class FireBall
    {
        //Fields
        private Texture2D texture;
        private Vector2 position;
        private Vector2 speed = new Vector2(6f, 0f);
        private static SoundEffect sound;
        private Rectangle destinationRect, sourceRect;
        private SpriteEffects effect;
        private double angle;
        private float maxSpeed = 12;

        //Properties
        public Vector2 Position
        {
            get
            { return this.position; } 
        }

        public static SoundEffect Sound
        {
            get
            { return sound; }
        }

        public Rectangle DestinationRect
        {
            get
            { return this.destinationRect; }
        }

        //Constructor
        public FireBall(ContentManager content, Vector2 position, int direction, MouseState ms )
        {
            this.texture = content.Load<Texture2D>(@"pictures/fireball");
            sound = content.Load<SoundEffect>(@"sound/Shotgun");
            if (direction == 1)
            {
                this.position = position + new Vector2(20f, 0f);
                Vector2 speed = this.CalculateSpeed(this.position, ms);
                this.speed = new Vector2(this.maxSpeed * speed.X, 
                                        this.maxSpeed * speed.Y);
                this.effect = SpriteEffects.None;
            }
            else
            {
                this.position = position + new Vector2(-40f, 0f);
                Vector2 speed = this.CalculateSpeed(this.position, ms);
                this.speed = new Vector2(-this.maxSpeed * speed.X,
                                        -this.maxSpeed * speed.Y);
                this.effect = SpriteEffects.FlipHorizontally;
            }
            this.destinationRect = new Rectangle((int)this.position.X, (int)this.position.Y, 50, 49);
            this.sourceRect = new Rectangle(0, 0, 50, 49);
             
        }

        //Update
        public void Update()
        {
            this.destinationRect.X += (int)this.speed.X;
            this.destinationRect.Y += (int)this.speed.Y;
        }

        //Draw
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(this.texture,
                             this.destinationRect,
                             this.sourceRect,
                             Color.FloralWhite,
                             (float)this.angle,
                             new Vector2(this.texture.Width/2, this.texture.Height/2),
                             this.effect,
                             0.0f);
        }

        private Vector2 CalculateSpeed(Vector2 position, MouseState ms)
        {
            float adjecentSide = ms.X - position.X;
            float oppositeSide = ms.Y - position.Y;
            this.angle = Math.Atan((double)(oppositeSide / adjecentSide));
            return new Vector2((float)Math.Cos(this.angle), (float)Math.Sin(this.angle)); 
        }

    }
}
