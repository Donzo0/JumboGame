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
    public enum GameState 
    { Start, Play, GameOver, Finished };

    public class Anarchy : Microsoft.Xna.Framework.Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private Texture2D background, animatedSprite;
        private Vector2 position;
        private Rectangle backgroundDestinationRect,backgroundSourceRect;
        private AnimatedSprite walkingMan;
        private Enemy enemy;
        private SpriteFont arial;
        private int score = 0;
        private GameState state = GameState.Start;

        //control
        private KeyboardState keyboardState, oldKeyboardState;

        //song
        private Song backsong;
        private bool songstart = false;

       //properties
        public int Score
        {
            get
            { return this.score; }
            set
            { this.score = value; }
        }

        public Anarchy()
        {
            //Dit is de constructor van de class. Een constructor heeft dezelfde als de class en heeft geen access-modifier
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            //Dit maakt de muis zichtbaar in het scherm van het spel
            IsMouseVisible = true;

            //Verandert de title in het gamescherm.
            Window.Title = "Anarchy Beta 1.001.001";

            //Hier voor de de groote van het speelscherm gewijzigd
            graphics.PreferredBackBufferWidth = 640;
            graphics.PreferredBackBufferHeight = 480;
            //past de wijziging toe
            graphics.ApplyChanges();
        }

        protected override void Initialize()
        {
            

            base.Initialize();
        }

       
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            this.background = Content.Load<Texture2D>(@"Pictures\Art_Anarchy");
            this.animatedSprite = Content.Load<Texture2D>(@"pictures\gb_walk");
            this.position = new Vector2(0f, 0f);
            this.backgroundDestinationRect = new Rectangle((int)this.position.X, 
                                                (int)this.position.Y,
                                                this.background.Width,
                                                this.background.Height);
            this.backgroundSourceRect = new Rectangle(0, 0, 640, 480);
            
            //We roepen de contructor aan van de class
            this.enemy = new Enemy(Content, new Vector2(480f, 300f));
            this.walkingMan = new AnimatedSprite(Content, 0, 325, Keys.N, Keys.M, this.enemy, this);
            this.arial = Content.Load<SpriteFont>(@"fonts\Arial");
           
            //loading song
            backsong = Content.Load<Song>(@"Songs\backsong");
            MediaPlayer.IsRepeating = true;

           
        }
        protected override void UnloadContent()
        {
           
        }

        protected override void Update(GameTime gameTime)
        {
                        
            //kijkt iedere update of het toetsenbord is gebruikt
            this.keyboardState = Keyboard.GetState();

            if (this.keyboardState.IsKeyDown(Keys.Escape))
            {
                this.Exit();
            }
                    
        
            switch (this.state)
            { 
                case GameState.Start:
                    if (this.keyboardState.IsKeyDown(Keys.B))
                    {
                        this.state = GameState.Play;
                    }
                    break;
                case GameState.Play:
                    //play song    
                    if (this.score < 0)
                    {
                        this.state = GameState.GameOver;
                    }
                    if (this.score >= 2500)
                    {
                        this.state = GameState.Finished;
                    }
                    //Update de walkingMan instantie
                    this.walkingMan.Update();
                    //Update de enemy instantie
                    this.enemy.Update();
                    
                    break;

                case GameState.GameOver:
                    if (this.keyboardState.IsKeyDown(Keys.R))
                    {
                        this.state = GameState.Play;
                        this.score = 0;
                    }
                    this.LoadContent();
                    break;
                case GameState.Finished:
                    if (this.keyboardState.IsKeyDown(Keys.R))
                    {
                        this.state = GameState.Play;
                        this.score = 0;
                    }
                    this.LoadContent();


                    break;

            }

            //Voor edgedecetion, zet de huidige keyboardstate in oude keyboardstate.
            this.oldKeyboardState = this.keyboardState;
            base.Update(gameTime);
        }

   
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            this.spriteBatch.Begin();

            switch (this.state)
            {
                case GameState.Start:
                    this.spriteBatch.Draw(this.background,
                                  this.backgroundDestinationRect,
                                  this.backgroundSourceRect,
                                  Color.FloralWhite,
                                  0f,
                                  new Vector2(0f, 0f),
                                  SpriteEffects.None,
                                  1f);
                    this.spriteBatch.DrawString(this.arial, "Druk op B om het spel te starten. ", new Vector2(20f, 10f), Color.FloralWhite);
                    break;
                case GameState.Play:
                    this.spriteBatch.Draw(this.background, 
                                  this.backgroundDestinationRect, 
                                  this.backgroundSourceRect,
                                  Color.FloralWhite,
                                  0f,
                                  new Vector2(0f, 0f),
                                  SpriteEffects.None,
                                  1f);
                        //Het tekenen van het lopende mannetje
                        this.walkingMan.Draw(this.spriteBatch);
                        //Het tekene 
                        this.enemy.Draw(this.spriteBatch);
                        //score
                        this.spriteBatch.DrawString(this.arial, "Score: " + this.score, new Vector2(20f, 10f),Color.FloralWhite);
                        break;

                case GameState.GameOver:
                    this.spriteBatch.Draw(this.background,
                                  this.backgroundDestinationRect,
                                  this.backgroundSourceRect,
                                  Color.FloralWhite,
                                  0f,
                                  new Vector2(0f, 0f),
                                  SpriteEffects.None,
                                  1f);
                    this.spriteBatch.DrawString(this.arial,
                                                "Druk op Escape om het spel te stoppen. \n " + "Druk op R om opnieuw te beginnen. ", 
                                                new Vector2(20f, 10f), 
                                                Color.FloralWhite);
                    break;
                case GameState.Finished:
                    this.spriteBatch.Draw(this.background,
                                          this.backgroundDestinationRect,
                                          this.backgroundSourceRect,
                                          Color.FloralWhite,
                                          0f,
                                          new Vector2(0f, 0f),
                                          SpriteEffects.None,
                                          1f);
                    this.spriteBatch.DrawString(this.arial,
                                               "Gefeliciteerd je hebt het spel gewonnen \n \n Druk op Escape om het spel te stoppen. \n " + "Druk op R om opnieuw te beginnen.",
                                               new Vector2(20f, 10f),
                                               Color.FloralWhite);
                    break;

            }       

            this.spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
