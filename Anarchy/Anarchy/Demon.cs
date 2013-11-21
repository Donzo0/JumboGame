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
    public class Demon
    {
        //Fields
        private Texture2D texture;
        private Rectangle destinationRect, sourceRect;
        private SpriteEffects effect = SpriteEffects.None;
        private float timer;
        private int speed = -6;
        private SoundEffect sound, soundDie;
       
        //Properties
        public Rectangle DestinationRect
        {
            get
            { return this.destinationRect; }
        }

        public SoundEffect SoundDie
        {
            get
            { return this.soundDie; }
        }

        //constructor
        public Demon(ContentManager content, Vector2 position)
        {
            this.texture = content.Load<Texture2D>(@"pictures\Demon");
            this.sound = content.Load<SoundEffect>(@"sound\roar");
            this.soundDie = content.Load<SoundEffect>(@"sound\Bullittouch");
            this.destinationRect = new Rectangle((int)position.X, (int)position.Y, 130 * 3/4, 140 * 3/4);
            this.sourceRect = new Rectangle(0, 0, 130, 140);
            this.sound.Play(1.0f, 0.0f, 0.0f);
        }

        //Update
        public void Update()
        {
            if (this.timer > 1f / 60f)
            {
                //schuif de camera op en zet de timer weer op nul
                if (this.sourceRect.X < 385)
                {
                    this.sourceRect.X += 130;
                }
                else
                {
                    this.sourceRect.X = 0;
                }
                this.timer = 0f;
            }
            else
            {
                this.timer += 1f / 60f;
            }

          
            //laat enemy heen en weer bewegen   
            this.destinationRect.X += this.speed;

        }

        //Draw
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(this.texture, this.destinationRect, this.sourceRect, Color.FloralWhite, 0f, Vector2.Zero, this.effect, 0.3f);
        }
    }
}
