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
    public class Enemy
    {
        //Fields
        private Texture2D texture;
        private Rectangle destinationRect, sourceRect;
        private SpriteEffects effect = SpriteEffects.None;
        private float timer, demonTimer;
        private int speed = -4;
        private List<Demon> demonList;
        private ContentManager content;
        private Random random;

        //Properties
        public List<Demon> DemonList
        {
            get
            { return this.demonList; }
        }

        //constructor
        public Enemy(ContentManager content, Vector2 position)
        {
            this.texture = content.Load<Texture2D>(@"pictures\Enemy");
            this.destinationRect = new Rectangle((int)position.X, (int)position.Y, 135, 165);
            this.sourceRect = new Rectangle(0,0,135,165);
            this.demonList = new List<Demon>();
            this.content = content;
            this.random = new Random();
        }

        //Update
        public void Update()
        {
            #region EnemyCode
            if (this.timer > 1f / 60f)
            {
                //schuif de camera op en zet de timer weer op nul
                if (this.sourceRect.X <= 404)
                {
                    this.sourceRect.X += 135;
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
            if (this.destinationRect.Y < 0 || this.destinationRect.Y > 315)
            {
                this.speed = this.speed * -1;

            }
            this.destinationRect.Y += this.speed; 
            #endregion

            this.random.Next(100);

            if (this.demonTimer > (30f + this.random.Next(30)) / 60f)
            {
                this.demonList.Add(new Demon(this.content, new Vector2(this.destinationRect.X - 45, this.destinationRect.Y + 20)));

                //snelheid van vuren van demons
                this.demonTimer = -1f;
            }
            else
            {
                this.demonTimer += 1f / 60f;
            }

            foreach (Demon demon in this.demonList)
            {
                demon.Update();
                if (demon.DestinationRect.X > 640 || demon.DestinationRect.X < -100)
                {
                    this.demonList.Remove(demon);
                    break;
                }
                            
            }



        }

        //Draw
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(this.texture,this.destinationRect,this.sourceRect,Color.FloralWhite, 0f, Vector2.Zero, this.effect, 0.3f);

            foreach (Demon demon in this.demonList)
            {
                demon.Draw(spriteBatch);
            }
        }
    }
}
