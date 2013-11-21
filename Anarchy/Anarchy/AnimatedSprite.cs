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
   
    public class AnimatedSprite
    {
        //Fields (klassenvariablen. member variables), deze zijn altijd private
        private Texture2D texture;
        private Rectangle destinationRect, sourceRect;
        private float timer = 0f;
        private KeyboardState ks, oks;
        private MouseState ms, oms;
        private Keys leftKeys, rightKeys;
        private SpriteEffects effect = SpriteEffects.None;
        private FireBall fireBall;
        private ContentManager content;
        private Vector2 position;
        private List<FireBall> fireballList;
        private List<Demon> demonList;
        private int direction = 1;
        private Anarchy game;

        //Properties, zijn altijd public en voor andere klassen te gebruiken.
        public Rectangle DestinationRect
        {
            set
            { this.destinationRect = value; }
            get
            { return this.destinationRect; }
        }


        /*Constructor, wordt aangeroepen om een nieuwe object(instantie van de class) te maken
         * 
         */
        public AnimatedSprite(ContentManager content, int x, int y, Keys leftKey, Keys rightKey, Enemy enemy, Anarchy game)
        {
            this.texture = content.Load<Texture2D>(@"pictures\gb_walk");
            this.destinationRect = new Rectangle(x, y, 104, 150);
            this.sourceRect = new Rectangle(0, 301, 104, 150);
            this.leftKeys = leftKey;
            this.rightKeys = rightKey;
            this.content = content;
            this.position = new Vector2(x, y);
            this.fireballList = new List<FireBall>();
            this.demonList = enemy.DemonList;
            this.game = game;

        }

        //Update method
        public void Update()
        {
            this.ks = Keyboard.GetState();
            this.ms = Mouse.GetState();

            #region Afvuren van fireball met spatiebalk
            if (this.ks.IsKeyDown(Keys.Space) && this.oks.IsKeyUp(Keys.Space) || 
                this.ms.LeftButton == ButtonState.Pressed && this.oms.LeftButton == ButtonState.Released)
            {
                //maak een fireball object
                if ((this.ms.X - 100 > this.destinationRect.X && this.direction == 1) ||
                    (this.ms.X < this.destinationRect.X && this.direction == 0)       &&
                    (this.ms.X > 0)                                                   &&
                    (this.ms.X < 640)                                                 &&
                    (this.ms.Y > 0)                                                   &&
                    (this.ms.Y < 480))
                {
                    this.fireballList.Add(new FireBall(this.content, new Vector2((this.destinationRect.X + 50),
                                                                      (this.destinationRect.Y + 40)), this.direction, this.ms));
                    FireBall.Sound.Play(1.0f, 0.0f, 0.0f);
                }

            } 
            #endregion

            #region update fireball en verwijderen van fireball die uit scherm is
            foreach (FireBall fireball in this.fireballList)
            {
                fireball.Update();
                if (fireball.Position.X > 640 || fireball.Position.X < 0)
                {
                    this.fireballList.Remove(fireball);
                    break;
                }

            } 
            #endregion

            #region bewegen naar links, rechts of stilstaan
            //laat je naar recht lopen
            if (this.ks.IsKeyDown(this.rightKeys))
            {
                this.direction = 1;
                if (this.effect != SpriteEffects.None)
                {
                    this.effect = SpriteEffects.None;
                }
                if (this.timer > 5f / 60f)
                {
                    //Verschuif camera en zet timer op 0
                    if (this.sourceRect.X < 520)
                    {
                        this.sourceRect.Y = 0;
                        this.sourceRect.X += 104;

                    }
                    else
                    {
                        //Hoog de timer op.
                        this.sourceRect.X = 0;
                    }
                    this.destinationRect.X += 18;
                    this.timer = 0f;
                }
                else
                {
                    this.timer += 1f / 60f;
                }

            }
            //laat je naar links lopen
            if (this.ks.IsKeyDown(this.leftKeys))
            {
                this.direction = 0;
                if (this.effect != SpriteEffects.FlipHorizontally)
                {
                    this.effect = SpriteEffects.FlipHorizontally;
                }
                if (this.timer > 5f / 60f)
                {
                    //Verschuif camera en zet timer op 0
                    if (this.sourceRect.X < 520)
                    {
                        this.sourceRect.Y = 0;
                        this.sourceRect.X += 104;

                    }
                    else
                    {
                        //Hoog de timer op.
                        this.sourceRect.X = 0;
                    }
                    this.destinationRect.X -= 18;
                    this.timer = 0f;
                }
                else
                {
                    this.timer += 1f / 60f;
                }

            }

            //Kijkt of de toets ingedrukt is.
            if (ks.IsKeyUp(this.leftKeys) && this.oks.IsKeyDown(this.leftKeys))
            {
                this.sourceRect.X = 0;
                this.sourceRect.Y = 301;
            }

            //Toon het juiste plaatje zien wanneer je de left of right knop los laat(stilstaan).
            if (ks.IsKeyUp(this.rightKeys) && this.oks.IsKeyDown(this.rightKeys))
            {
                this.sourceRect.X = 0;
                this.sourceRect.Y = 301;
            }

            #endregion

            #region Collisiondetection fireball - demons
            for (int i = 0; i < this.fireballList.Count; i++)
            {
                for (int j = 0; j < this.demonList.Count; j++)
                {
                    if (this.fireballList[i].DestinationRect.Intersects(this.demonList[j].DestinationRect))
                    {
                        this.demonList[j].SoundDie.Play(1.0f, 0.0f, 0.0f);
                        this.fireballList.RemoveAt(i);
                        this.demonList.RemoveAt(j);
                        this.game.Score += 100;
                        break;
                    }

                }

            } 
            #endregion

            #region Collisiondetection demons - walkingman
            for (int i = 0; i < this.demonList.Count; i++)
            {
                if (this.destinationRect.Intersects(this.demonList[i].DestinationRect))
                {
                    this.demonList.RemoveAt(i);
                    this.game.Score -= 50;
                    break;
                }
            } 
            #endregion

                this.oks = this.ks;
                this.oms = this.ms;
        }


        //Draw method
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(this.texture,
                             this.destinationRect,
                             this.sourceRect,
                             Color.FloralWhite,
                             0f,
                             Vector2.Zero,
                             this.effect,
                             0.5f);

            
                //het tekenen van het vuurbal
                foreach (FireBall fireball in this.fireballList)
                {
                    fireball.Draw(spriteBatch);
                }
            
        }
    }
}
