using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace Particle_Sim_2D
{
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Random r = new Random();
        int modus = 1;

        SpriteFont font;
        public static GraphicsDevice Device;
        Texture2D instructions, overlay;
        Texture2D[] metaballTextures;
        Color[] glowColors;
        RenderTarget2D metaballTarget;
        AlphaTestEffect effect;
        List<Metaball> balls = new List<Metaball>();
        bool showOverlay = true;
        int currentColor = 0;
        Random rand;
        KeyboardState state, lastState;
        public const int MetaballRadius = 16;
        public const float MetaballScale = 1f;
        const int NumMetaballs = 10000;




        const int ballmenge = 10000;
        const int meng = 6;
        const int partgros = 8;
        int sss = partgros - 2;
        int ausgewähltematerie = 1;
        float temperaturverbreitung = 20;
        int gestartet = 0;
        int jjj = 0;
        float[] balltemperatur = new float[ballmenge];
        int[] particelexplodiert = new int[ballmenge];
        int[] particelexplodiert2 = new int[ballmenge];
        int[] particelart = new int[ballmenge];
        int[,,] felder = new int[120, 120, 40];
        Vector2[] pos = new Vector2[ballmenge];
        Vector2[] fixeparticelpos = new Vector2[ballmenge];
        Vector2[] speed = new Vector2[ballmenge];
        int[] particel = new int[ballmenge];
        int[] particelcollidiert = new int[ballmenge];
        int[] particel2 = new int[ballmenge];
        int[] particelfix = new int[ballmenge];
        int[] particelfixtimer = new int[ballmenge];
        int[,,] particelfeldbesetzt = new int[153, 98, meng];
        int[,,] particelfeldnummer = new int[153, 98, meng];
        int thread1gestartet = 0;
        int thread2gestartet = 0;
        int thread3gestartet = 0;
        int thread4gestartet = 0;
        float xgrav;
        float ygrav = 0.05f;
        int grenzhöhe = 350;
        int pause = 0;
        int pausegdrückt;
        int aaaa;
        Texture2D wassertex, festesparticeltex, explosivetex, firetex, steamtex;
        SpriteFont b;
        int aaa;
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            base.Initialize();
            graphics.PreferredBackBufferHeight = 1080;
            graphics.PreferredBackBufferWidth = 1920;
            graphics.IsFullScreen = false;
            IsFixedTimeStep = false;
            //this.TargetElapsedTime = TimeSpan.FromTicks(69445);
            graphics.SynchronizeWithVerticalRetrace = true;
            this.IsMouseVisible = true;
            rand = new Random();
            for (int i = 0; i < NumMetaballs; i++)
            {
                var ball = new Metaball();
                //ball.Position = new Vector2(rand.Next(GraphicsDevice.Viewport.Width), rand.Next(GraphicsDevice.Viewport.Height)) - new Vector2(128);
                ball.Texture = metaballTextures[0];
                ball.Glow = glowColors[0];
                //ball.Velocity = new Vector2(((float)rand.Next(-50, 50)) / 20, ((float)rand.Next(-50, 50)) / 20);
                balls.Add(ball);
            }
            graphics.ApplyChanges();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            wassertex = Content.Load<Texture2D>("bestwasserparticel");
            festesparticeltex = Content.Load<Texture2D>("festesparticel");
            b = Content.Load<SpriteFont>("SpriteFont1");
            explosivetex = Content.Load<Texture2D>("explosions");
            steamtex = Content.Load<Texture2D>("steam");
            firetex = Content.Load<Texture2D>("fire");
            Device = GraphicsDevice;
            spriteBatch = new SpriteBatch(GraphicsDevice);
            font = Content.Load<SpriteFont>("Font");
            graphics.PreferredBackBufferWidth = GraphicsDevice.DisplayMode.Width;
            graphics.PreferredBackBufferHeight = GraphicsDevice.DisplayMode.Height;
            graphics.ApplyChanges();
            // generate some differently coloured metaball textures. You could tint them with SpriteBatch instead, 
            // but then you couldn't have a two colour gradient.
            metaballTextures = new Texture2D[5];
            Color abc = Color.Blue;
            metaballTextures[0] = Metaball.GenerateTexture(MetaballRadius, Metaball.CreateTwoColorPicker(Color.Blue, Color.Blue));
            metaballTextures[1] = Metaball.GenerateTexture(MetaballRadius, Metaball.CreateTwoColorPicker(Color.Red, Color.Red));
            metaballTextures[2] = Metaball.GenerateTexture(MetaballRadius, Metaball.CreateTwoColorPicker(Color.DarkOrange, Color.DarkOrange));
            metaballTextures[3] = Metaball.GenerateTexture(MetaballRadius, Metaball.CreateTwoColorPicker(Color.LightBlue, Color.LightBlue));
            metaballTextures[4] = Metaball.GenerateTexture(MetaballRadius, Metaball.CreateTwoColorPicker(Color.Gray, Color.Gray));

            glowColors = new Color[] { Color.Red, Color.Blue, Color.Lime, Color.Magenta, Color.Red };

            // initialize the alpha test effect.
            effect = new AlphaTestEffect(GraphicsDevice);
            var viewport = GraphicsDevice.Viewport;
            effect.Projection = Matrix.CreateTranslation(-0.5f, -0.5f, 0) * Matrix.CreateOrthographicOffCenter(0, viewport.Width, viewport.Height, 0, 0, 1);
            effect.ReferenceAlpha = 128;

            metaballTarget = new RenderTarget2D(GraphicsDevice, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }
        public void linksoben()
        {
            for (; ; )
            {
                collisionserkennung(0);
                System.Threading.Thread.Sleep(1 / 10);
                jjj++;
            }
        }
        public void rechtsoben()
        {
            for (; ; )
            {
                collisionserkennung(1);
                System.Threading.Thread.Sleep(1 / 10);
            }

        }
        public void linksunten()
        {
            for (; ; )
            {
                collisionserkennung(2);
                System.Threading.Thread.Sleep(1 / 10);
            }
        }
        public void rechtsunten()
        {
            for (; ; )
            {
                collisionserkennung(3);
                System.Threading.Thread.Sleep(1 / 10);
            }
        }
        public void collisionserkennung(int t)
        {
            int xan = 0;
            int xend = 0;
            int yan = 0;
            int yend = 0;
            if (t == 0)
            {
                xan = 0;
                xend = 76;
                yan = 0;
                yend = 49;
            }
            if (t == 1)
            {
                xan = 76;
                xend = 153;
                yan = 0;
                yend = 49;
            }
            if (t == 2)
            {
                xan = 0;
                xend = 76;
                yan = 49;
                yend = 98;
            }
            if (t == 3)
            {
                xan = 76;
                xend = 153;
                yan = 49;
                yend = 98;
            }
            for (int i = xan; i < xend; i++)
            {
                for (int j = yan; j < yend; j++)
                {
                    for (int x = 0; x < meng; x++)
                    {
                        if (particelfeldbesetzt[i, j, x] == 1)
                        {
                            int zz = particelfeldnummer[i, j, x];
                            if (particel[zz] == 1)
                            {
                                if (pos[zz].Y > grenzhöhe)
                                {
                                    speed[zz].Y = 0;
                                    pos[zz].Y = grenzhöhe;
                                }
                                if (pos[zz].Y < 10)
                                {
                                    speed[zz].Y = 0;
                                    pos[zz].Y = 10;
                                }
                                if (pos[zz].X < 10)
                                {
                                    speed[zz].X = 0;
                                    pos[zz].X = 10;
                                }
                                if (pos[zz].X > 1200)
                                {
                                    speed[zz].X = 0;
                                    pos[zz].X = 1200;
                                }
                                if (particelcollidiert[zz] == 0)
                                {
                                    if (particelart[zz] != 5)
                                    {
                                        speed[zz].Y += ygrav;
                                        speed[zz].X += xgrav;
                                    }
                                    else
                                    {
                                        speed[zz].Y -= (-100 + balltemperatur[zz]) * (ygrav / 10.0f);
                                        speed[zz].X -= (-100 + balltemperatur[zz]) * (xgrav / 10.0f);
                                    }
                                }
                                pos[zz].Y += speed[zz].Y;
                                pos[zz].X += speed[zz].X;
                            }
                        }
                    }
                }
            }
            for (int i = xan; i < xend; i++)
            {
                for (int j = yan; j < yend; j++)
                {
                    for (int x = 0; x < meng; x++)
                    {
                        particelfeldbesetzt[i, j, x] = 0;
                        particelfeldnummer[i, j, x] = 0;
                    }
                }
            }
            for (int i = 0; i < ballmenge; i++)
            {
                if (particel[i] == 1)
                {

                    particelexplodiert[i] = 0;
                    particelcollidiert[i] = 0;
                    for (int j = 0; j < meng; j++)
                    {
                        if ((int)pos[i].X / 10 >= xan && (int)pos[i].X / 10 < xend && (int)pos[i].Y / 10 >= yan && (int)pos[i].Y / 10 < yend)
                        {
                            if (particelfeldbesetzt[(int)pos[i].X / 10, (int)pos[i].Y / 10, j] == 0)
                            {
                                particelfeldbesetzt[(int)pos[i].X / 10, (int)pos[i].Y / 10, j] = 1;
                                particelfeldnummer[(int)pos[i].X / 10, (int)pos[i].Y / 10, j] = i;
                                break;
                            }
                        }
                    }
                }
            }

            for (int i = xan; i < xend; i++)
            {
                for (int j = yan; j < yend; j++)
                {
                    for (int x = 0; x < meng; x++)
                    {
                        if (particelfeldbesetzt[i, j, x] == 1)
                        {
                            for (int y = 0; y < meng; y++)
                            {
                                if (particelfeldbesetzt[i, j, y] == 1)
                                {
                                    int i2 = particelfeldnummer[i, j, x];
                                    int j2 = particelfeldnummer[i, j, y];
                                    Vector2 delta = pos[i2] - pos[j2];
                                    float d = (float)Math.Sqrt(((delta.X) * (delta.X)) + ((delta.Y) * (delta.Y)));
                                    if (d < partgros && d > 0)
                                    {
                                        float zz = balltemperatur[i2];
                                        float zz2 = balltemperatur[j2];
                                        balltemperatur[i2] += (zz2 - zz) / temperaturverbreitung;
                                        balltemperatur[j2] += (zz - zz2) / temperaturverbreitung;
                                        if (particelart[i2] == 3 && particelart[j2] == 4)
                                        {
                                            particelexplodiert[i2] = 1;
                                        }
                                        if (particelart[j2] == 3 && particelart[i2] == 4)
                                        {
                                            particelexplodiert[j2] = 1;
                                        }
                                        Vector2 mtd = delta * (((partgros) - d) / d);
                                        float im1 = 1;
                                        float im2 = 1;
                                        pos[i2].X = pos[i2].X + (mtd.X * (im1 / (im1 + im2)));
                                        pos[i2].Y = pos[i2].Y + (mtd.Y * (im1 / (im1 + im2)));
                                        pos[j2].X = pos[j2].X - (mtd.X * (im2 / (im1 + im2)));
                                        pos[j2].Y = pos[j2].Y - (mtd.Y * (im2 / (im1 + im2)));
                                        Vector2 v = speed[i2] - speed[j2];
                                        float f = 2f / (im1 + im2);
                                        Vector2 impulse = mtd * f;
                                        speed[i2] = speed[i2] + ((impulse * im1) / 10);
                                        speed[j2] = speed[j2] - ((impulse * im2) / 10);
                                        if (d < sss)
                                        {
                                            particelcollidiert[i2] = 1;
                                            particelcollidiert[j2] = 1;
                                        }
                                    }
                                }
                            }
                            for (int y = 0; y < meng; y++)
                            {
                                if (i + 1 < 153)
                                {
                                    if (particelfeldbesetzt[i + 1, j, y] == 1)
                                    {
                                        int i2 = particelfeldnummer[i, j, x];
                                        int j2 = particelfeldnummer[i + 1, j, y];
                                        Vector2 delta = pos[i2] - pos[j2];
                                        float d = (float)Math.Sqrt(((delta.X) * (delta.X)) + ((delta.Y) * (delta.Y)));
                                        if (d < partgros && d > 0)
                                        {
                                            float zz = balltemperatur[i2];
                                            float zz2 = balltemperatur[j2];
                                            balltemperatur[i2] += (zz2 - zz) / temperaturverbreitung;
                                            balltemperatur[j2] += (zz - zz2) / temperaturverbreitung;
                                            if (particelart[i2] == 3 && particelart[j2] == 4)
                                            {
                                                particelexplodiert[i2] = 1;
                                            }
                                            if (particelart[j2] == 3 && particelart[i2] == 4)
                                            {
                                                particelexplodiert[j2] = 1;
                                            }
                                            Vector2 mtd = delta * (((partgros) - d) / d);
                                            float im1 = 1;
                                            float im2 = 1;
                                            pos[i2].X = pos[i2].X + (mtd.X * (im1 / (im1 + im2)));
                                            pos[i2].Y = pos[i2].Y + (mtd.Y * (im1 / (im1 + im2)));
                                            pos[j2].X = pos[j2].X - (mtd.X * (im2 / (im1 + im2)));
                                            pos[j2].Y = pos[j2].Y - (mtd.Y * (im2 / (im1 + im2)));
                                            Vector2 v = speed[i2] - speed[j2];
                                            float f = 2f / (im1 + im2);
                                            Vector2 impulse = mtd * f;
                                            speed[i2] = speed[i2] + ((impulse * im1) / 10);
                                            speed[j2] = speed[j2] - ((impulse * im2) / 10);
                                            if (d < sss)
                                            {
                                                particelcollidiert[i2] = 1;
                                                particelcollidiert[j2] = 1;
                                            }
                                        }
                                    }
                                }
                            }
                            for (int y = 0; y < meng; y++)
                            {
                                if (j + 1 < 98)
                                {
                                    if (particelfeldbesetzt[i, j + 1, y] == 1)
                                    {
                                        int i2 = particelfeldnummer[i, j, x];
                                        int j2 = particelfeldnummer[i, j + 1, y];
                                        Vector2 delta = pos[i2] - pos[j2];
                                        float d = (float)Math.Sqrt(((delta.X) * (delta.X)) + ((delta.Y) * (delta.Y)));
                                        if (d < partgros && d > 0)
                                        {
                                            float zz = balltemperatur[i2];
                                            float zz2 = balltemperatur[j2];
                                            balltemperatur[i2] += (zz2 - zz) / temperaturverbreitung;
                                            balltemperatur[j2] += (zz - zz2) / temperaturverbreitung;
                                            if (particelart[i2] == 3 && particelart[j2] == 4)
                                            {
                                                particelexplodiert[i2] = 1;
                                            }
                                            if (particelart[j2] == 3 && particelart[i2] == 4)
                                            {
                                                particelexplodiert[j2] = 1;
                                            }
                                            Vector2 mtd = delta * (((partgros) - d) / d);
                                            float im1 = 1;
                                            float im2 = 1;
                                            pos[i2].X = pos[i2].X + (mtd.X * (im1 / (im1 + im2)));
                                            pos[i2].Y = pos[i2].Y + (mtd.Y * (im1 / (im1 + im2)));
                                            pos[j2].X = pos[j2].X - (mtd.X * (im2 / (im1 + im2)));
                                            pos[j2].Y = pos[j2].Y - (mtd.Y * (im2 / (im1 + im2)));
                                            Vector2 v = speed[i2] - speed[j2];
                                            float f = 2f / (im1 + im2);
                                            Vector2 impulse = mtd * f;
                                            speed[i2] = speed[i2] + ((impulse * im1) / 10);
                                            speed[j2] = speed[j2] - ((impulse * im2) / 10);
                                            if (d < sss)
                                            {
                                                particelcollidiert[i2] = 1;
                                                particelcollidiert[j2] = 1;
                                            }
                                        }
                                    }
                                }
                            }
                            for (int y = 0; y < meng; y++)
                            {
                                if (i + 1 < 153 && j + 1 < 98)
                                {
                                    if (particelfeldbesetzt[i + 1, j + 1, y] == 1)
                                    {
                                        int i2 = particelfeldnummer[i, j, x];
                                        int j2 = particelfeldnummer[i + 1, j + 1, y];
                                        Vector2 delta = pos[i2] - pos[j2];
                                        float d = (float)Math.Sqrt(((delta.X) * (delta.X)) + ((delta.Y) * (delta.Y)));
                                        if (d < partgros && d > 0)
                                        {
                                            float zz = balltemperatur[i2];
                                            float zz2 = balltemperatur[j2];
                                            balltemperatur[i2] += (zz2 - zz) / temperaturverbreitung;
                                            balltemperatur[j2] += (zz - zz2) / temperaturverbreitung;
                                            if (particelart[i2] == 3 && particelart[j2] == 4)
                                            {
                                                particelexplodiert[i2] = 1;
                                            }
                                            if (particelart[j2] == 3 && particelart[i2] == 4)
                                            {
                                                particelexplodiert[j2] = 1;
                                            }
                                            Vector2 mtd = delta * (((partgros) - d) / d);
                                            float im1 = 1;
                                            float im2 = 1;
                                            pos[i2].X = pos[i2].X + (mtd.X * (im1 / (im1 + im2)));
                                            pos[i2].Y = pos[i2].Y + (mtd.Y * (im1 / (im1 + im2)));
                                            pos[j2].X = pos[j2].X - (mtd.X * (im2 / (im1 + im2)));
                                            pos[j2].Y = pos[j2].Y - (mtd.Y * (im2 / (im1 + im2)));
                                            Vector2 v = speed[i2] - speed[j2];
                                            float f = 2f / (im1 + im2);
                                            Vector2 impulse = mtd * f;
                                            speed[i2] = speed[i2] + ((impulse * im1) / 10);
                                            speed[j2] = speed[j2] - ((impulse * im2) / 10);
                                            if (d < sss)
                                            {
                                                particelcollidiert[i2] = 1;
                                                particelcollidiert[j2] = 1;
                                            }
                                        }
                                    }
                                }
                            }
                            for (int y = 0; y < meng; y++)
                            {
                                if (i - 1 >= 0 && j + 1 < 98)
                                {
                                    if (particelfeldbesetzt[i - 1, j + 1, y] == 1)
                                    {
                                        int i2 = particelfeldnummer[i, j, x];
                                        int j2 = particelfeldnummer[i - 1, j + 1, y];
                                        Vector2 delta = pos[i2] - pos[j2];
                                        float d = (float)Math.Sqrt(((delta.X) * (delta.X)) + ((delta.Y) * (delta.Y)));
                                        if (d < partgros && d > 0)
                                        {
                                            float zz = balltemperatur[i2];
                                            float zz2 = balltemperatur[j2];
                                            balltemperatur[i2] += (zz2 - zz) / temperaturverbreitung;
                                            balltemperatur[j2] += (zz - zz2) / temperaturverbreitung;
                                            if (particelart[i2] == 3 && particelart[j2] == 4)
                                            {
                                                particelexplodiert[i2] = 1;
                                            }
                                            if (particelart[j2] == 3 && particelart[i2] == 4)
                                            {
                                                particelexplodiert[j2] = 1;
                                            }
                                            Vector2 mtd = delta * (((partgros) - d) / d);
                                            float im1 = 1;
                                            float im2 = 1;
                                            pos[i2].X = pos[i2].X + (mtd.X * (im1 / (im1 + im2)));
                                            pos[i2].Y = pos[i2].Y + (mtd.Y * (im1 / (im1 + im2)));
                                            pos[j2].X = pos[j2].X - (mtd.X * (im2 / (im1 + im2)));
                                            pos[j2].Y = pos[j2].Y - (mtd.Y * (im2 / (im1 + im2)));
                                            Vector2 v = speed[i2] - speed[j2];
                                            float f = 2f / (im1 + im2);
                                            Vector2 impulse = mtd * f;
                                            speed[i2] = speed[i2] + ((impulse * im1) / 10);
                                            speed[j2] = speed[j2] - ((impulse * im2) / 10);
                                            if (d < sss)
                                            {
                                                particelcollidiert[i2] = 1;
                                                particelcollidiert[j2] = 1;
                                            }
                                        }
                                    }
                                }
                            }
                            for (int y = 0; y < meng; y++)
                            {
                                if (i - 1 >= 0)
                                {
                                    if (particelfeldbesetzt[i - 1, j, y] == 1)
                                    {
                                        int i2 = particelfeldnummer[i, j, x];
                                        int j2 = particelfeldnummer[i - 1, j, y];
                                        Vector2 delta = pos[i2] - pos[j2];
                                        float d = (float)Math.Sqrt(((delta.X) * (delta.X)) + ((delta.Y) * (delta.Y)));
                                        if (d < partgros && d > 0)
                                        {
                                            float zz = balltemperatur[i2];
                                            float zz2 = balltemperatur[j2];
                                            balltemperatur[i2] += (zz2 - zz) / temperaturverbreitung;
                                            balltemperatur[j2] += (zz - zz2) / temperaturverbreitung;
                                            if (particelart[i2] == 3 && particelart[j2] == 4)
                                            {
                                                particelexplodiert[i2] = 1;
                                            }
                                            if (particelart[j2] == 3 && particelart[i2] == 4)
                                            {
                                                particelexplodiert[j2] = 1;
                                            }
                                            Vector2 mtd = delta * (((partgros) - d) / d);
                                            float im1 = 1;
                                            float im2 = 1;
                                            pos[i2].X = pos[i2].X + (mtd.X * (im1 / (im1 + im2)));
                                            pos[i2].Y = pos[i2].Y + (mtd.Y * (im1 / (im1 + im2)));
                                            pos[j2].X = pos[j2].X - (mtd.X * (im2 / (im1 + im2)));
                                            pos[j2].Y = pos[j2].Y - (mtd.Y * (im2 / (im1 + im2)));
                                            Vector2 v = speed[i2] - speed[j2];
                                            float f = 2f / (im1 + im2);
                                            Vector2 impulse = mtd * f;
                                            speed[i2] = speed[i2] + ((impulse * im1) / 10);
                                            speed[j2] = speed[j2] - ((impulse * im2) / 10);
                                            if (d < sss)
                                            {
                                                particelcollidiert[i2] = 1;
                                                particelcollidiert[j2] = 1;
                                            }
                                        }
                                    }
                                }
                            }
                            for (int y = 0; y < meng; y++)
                            {
                                if (i - 1 >= 0 && j - 1 >= 0)
                                {
                                    if (particelfeldbesetzt[i - 1, j - 1, y] == 1)
                                    {
                                        int i2 = particelfeldnummer[i, j, x];
                                        int j2 = particelfeldnummer[i - 1, j - 1, y];
                                        Vector2 delta = pos[i2] - pos[j2];
                                        float d = (float)Math.Sqrt(((delta.X) * (delta.X)) + ((delta.Y) * (delta.Y)));
                                        if (d < partgros && d > 0)
                                        {
                                            float zz = balltemperatur[i2];
                                            float zz2 = balltemperatur[j2];
                                            balltemperatur[i2] += (zz2 - zz) / temperaturverbreitung;
                                            balltemperatur[j2] += (zz - zz2) / temperaturverbreitung;
                                            if (particelart[i2] == 3 && particelart[j2] == 4)
                                            {
                                                particelexplodiert[i2] = 1;
                                            }
                                            if (particelart[j2] == 3 && particelart[i2] == 4)
                                            {
                                                particelexplodiert[j2] = 1;
                                            }
                                            Vector2 mtd = delta * (((partgros) - d) / d);
                                            float im1 = 1;
                                            float im2 = 1;
                                            pos[i2].X = pos[i2].X + (mtd.X * (im1 / (im1 + im2)));
                                            pos[i2].Y = pos[i2].Y + (mtd.Y * (im1 / (im1 + im2)));
                                            pos[j2].X = pos[j2].X - (mtd.X * (im2 / (im1 + im2)));
                                            pos[j2].Y = pos[j2].Y - (mtd.Y * (im2 / (im1 + im2)));
                                            Vector2 v = speed[i2] - speed[j2];
                                            float f = 2f / (im1 + im2);
                                            Vector2 impulse = mtd * f;
                                            speed[i2] = speed[i2] + ((impulse * im1) / 10);
                                            speed[j2] = speed[j2] - ((impulse * im2) / 10);
                                            if (d < sss)
                                            {
                                                particelcollidiert[i2] = 1;
                                                particelcollidiert[j2] = 1;
                                            }
                                        }
                                    }
                                }
                            }
                            for (int y = 0; y < meng; y++)
                            {
                                if (j - 1 >= 0)
                                {
                                    if (particelfeldbesetzt[i, j - 1, y] == 1)
                                    {
                                        int i2 = particelfeldnummer[i, j, x];
                                        int j2 = particelfeldnummer[i, j - 1, y];
                                        Vector2 delta = pos[i2] - pos[j2];
                                        float d = (float)Math.Sqrt(((delta.X) * (delta.X)) + ((delta.Y) * (delta.Y)));
                                        if (d < partgros && d > 0)
                                        {
                                            float zz = balltemperatur[i2];
                                            float zz2 = balltemperatur[j2];
                                            balltemperatur[i2] += (zz2 - zz) / temperaturverbreitung;
                                            balltemperatur[j2] += (zz - zz2) / temperaturverbreitung;
                                            if (particelart[i2] == 3 && particelart[j2] == 4)
                                            {
                                                particelexplodiert[i2] = 1;
                                            }
                                            if (particelart[j2] == 3 && particelart[i2] == 4)
                                            {
                                                particelexplodiert[j2] = 1;
                                            }
                                            Vector2 mtd = delta * (((partgros) - d) / d);
                                            float im1 = 1;
                                            float im2 = 1;
                                            pos[i2].X = pos[i2].X + (mtd.X * (im1 / (im1 + im2)));
                                            pos[i2].Y = pos[i2].Y + (mtd.Y * (im1 / (im1 + im2)));
                                            pos[j2].X = pos[j2].X - (mtd.X * (im2 / (im1 + im2)));
                                            pos[j2].Y = pos[j2].Y - (mtd.Y * (im2 / (im1 + im2)));
                                            Vector2 v = speed[i2] - speed[j2];
                                            float f = 2f / (im1 + im2);
                                            Vector2 impulse = mtd * f;
                                            speed[i2] = speed[i2] + ((impulse * im1) / 10);
                                            speed[j2] = speed[j2] - ((impulse * im2) / 10);
                                            if (d < sss)
                                            {
                                                particelcollidiert[i2] = 1;
                                                particelcollidiert[j2] = 1;
                                            }
                                        }
                                    }
                                }
                            }
                            for (int y = 0; y < meng; y++)
                            {
                                if (i + 1 < 153 && j - 1 >= 0)
                                {
                                    if (particelfeldbesetzt[i + 1, j - 1, y] == 1)
                                    {
                                        int i2 = particelfeldnummer[i, j, x];
                                        int j2 = particelfeldnummer[i + 1, j - 1, y];
                                        Vector2 delta = pos[i2] - pos[j2];
                                        float d = (float)Math.Sqrt(((delta.X) * (delta.X)) + ((delta.Y) * (delta.Y)));
                                        if (d < partgros && d > 0)
                                        {
                                            float zz = balltemperatur[i2];
                                            float zz2 = balltemperatur[j2];
                                            balltemperatur[i2] += (zz2 - zz) / temperaturverbreitung;
                                            balltemperatur[j2] += (zz - zz2) / temperaturverbreitung;
                                            if (particelart[i2] == 3 && particelart[j2] == 4)
                                            {
                                                particelexplodiert[i2] = 1;
                                            }
                                            if (particelart[j2] == 3 && particelart[i2] == 4)
                                            {
                                                particelexplodiert[j2] = 1;
                                            }
                                            Vector2 mtd = delta * (((partgros) - d) / d);
                                            float im1 = 1;
                                            float im2 = 1;
                                            pos[i2].X = pos[i2].X + (mtd.X * (im1 / (im1 + im2)));
                                            pos[i2].Y = pos[i2].Y + (mtd.Y * (im1 / (im1 + im2)));
                                            pos[j2].X = pos[j2].X - (mtd.X * (im2 / (im1 + im2)));
                                            pos[j2].Y = pos[j2].Y - (mtd.Y * (im2 / (im1 + im2)));
                                            Vector2 v = speed[i2] - speed[j2];
                                            float f = 2f / (im1 + im2);
                                            Vector2 impulse = mtd * f;
                                            speed[i2] = speed[i2] + ((impulse * im1) / 10);
                                            speed[j2] = speed[j2] - ((impulse * im2) / 10);
                                            if (d < sss)
                                            {
                                                particelcollidiert[i2] = 1;
                                                particelcollidiert[j2] = 1;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            for (int i = 0; i < ballmenge; i++)
            {
                if (particel[i] == 1)
                {
                    if (particelexplodiert2[i] == 1 && particelart[i] == 3)
                    {
                        particelexplodiert[i] = 1;
                        particelexplodiert2[i] = 0;
                    }
                }
            }
            aaa = 0;
            for (int i = 0; i < ballmenge; i++)
            {
                if (particel[i] == 1)
                {
                    aaa++;
                    if (particelart[i] == 1 && balltemperatur[i] >= 100)
                    {
                        particelart[i] = 5;
                    }
                    if (particelart[i] == 5 && balltemperatur[i] < 100)
                    {
                        particelart[i] = 1;
                    }
                    if (particelart[i] == 4 && balltemperatur[i] < 100)
                    {
                        particelexplodiert[i] = 0;
                        particelexplodiert2[i] = 0;
                        particel[i] = 0;
                        balltemperatur[i] = 22;
                        speed[i].X = 0;
                        speed[i].Y = 0;
                    }
                    if (particelart[i] == 2)
                    {
                        pos[i] = fixeparticelpos[i];
                        balltemperatur[i] = 22;
                        speed[i].X = 0;
                        speed[i].Y = 0;
                    }
                    if (particelexplodiert[i] == 1)
                    {
                        for (int j = 0; j < ballmenge; j++)
                        {
                            if (particel[j] == 1)
                            {

                                Vector2 delta = pos[i] - pos[j];
                                float d = (float)Math.Sqrt(((delta.X) * (delta.X)) + ((delta.Y) * (delta.Y)));
                                if (d < 50)
                                {
                                    if (d < 30 && particelart[j] == 3)
                                    {
                                        particelexplodiert2[j] = 1;
                                    }
                                    speed[j].X += ((pos[j].X - pos[i].X) / 500) * (50 - d);
                                    speed[j].Y += ((pos[j].Y - pos[i].Y) / 500) * (50 - d);
                                }
                            }
                        }
                        particelexplodiert[i] = 0;
                        particelexplodiert2[i] = 0;
                        particel[i] = 0;
                        speed[i].X = 0;
                        speed[i].Y = 0;
                    }
                }
            }
        }
        private void HandleInputOnPC()
        {
            // allow user to toggle fullscreen mode
            if (state.IsKeyDown(Keys.LeftShift) && state.IsKeyDown(Keys.Enter))
            {
                if (graphics.IsFullScreen)
                {
                    graphics.PreferredBackBufferWidth = 1920;
                    graphics.PreferredBackBufferHeight = 1080;
                }
                else
                {
                    graphics.PreferredBackBufferWidth = GraphicsDevice.DisplayMode.Width;
                    graphics.PreferredBackBufferHeight = GraphicsDevice.DisplayMode.Height;
                }
                graphics.ToggleFullScreen();
                metaballTarget.Dispose();
                metaballTarget = new RenderTarget2D(GraphicsDevice, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
                var viewport = GraphicsDevice.Viewport;
                effect.Projection = Matrix.CreateTranslation(-0.5f, -0.5f, 0) * Matrix.CreateOrthographicOffCenter(0, viewport.Width, viewport.Height, 0, 0, 1);
            }

            // pressing the up or down arrows changes the alpha threshold
            if (state.IsKeyDown(Keys.Up))
                effect.ReferenceAlpha = (int)MathHelper.Clamp(effect.ReferenceAlpha + 1, 0, 256);
            if (state.IsKeyDown(Keys.Down))
                effect.ReferenceAlpha = (int)MathHelper.Clamp(effect.ReferenceAlpha - 1, 0, 256);

            // the plus and minus keys add and remove metaballs
            if (state.IsKeyDown(Keys.OemPlus))
            {
                int index = currentColor;
                if (currentColor == metaballTextures.Length)
                    index = rand.Next(metaballTextures.Length);

                balls.Add(new Metaball()
                {
                    Glow = glowColors[index],
                    Texture = metaballTextures[index],
                    Position = new Vector2(rand.Next(GraphicsDevice.Viewport.Width), rand.Next(GraphicsDevice.Viewport.Height)),
                });
            }
            if (state.IsKeyDown(Keys.OemMinus) && balls.Count > 0)
                balls.RemoveAt(rand.Next(balls.Count));

            if (state.IsKeyDown(Keys.F10))
                showOverlay = !showOverlay;
        }
        protected override void Update(GameTime gameTime)
        {
            lastState = state;
            state = Keyboard.GetState();
            // Allows the game to exit
            var mouseState = Mouse.GetState();
            var mousePosition = new Point(mouseState.X, mouseState.Y);
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();
            aaaa = 0;
            HandleInputOnPC();
            if (Keyboard.GetState().IsKeyDown(Keys.D1))
            {
                ausgewähltematerie = 1;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.D2))
            {
                ausgewähltematerie = 3;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.D3))
            {
                ausgewähltematerie = 4;
            }
            if (Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                for (int i = 0; i < ballmenge; i++)
                {
                    if (particel[i] == 0)
                    {
                        for (int j = 0; j < ballmenge; j++)
                        {
                            if (particel[j] == 1)
                            {
                                if (mousePosition.X > pos[j].X - 10 && mousePosition.X < pos[j].X + 10 && mousePosition.Y > pos[j].Y - 10 && mousePosition.Y < pos[j].Y + 10)
                                {
                                    aaaa = 1;
                                }
                            }
                        }
                        if (aaaa == 0)
                        {
                            //felder[(int)(mousePosition.X / 5), (int)(mousePosition.Y / 5), a] = 1;
                            particel[i] = 1;
                            particel2[i] = 1;
                            aaa++;
                            pos[i].X = mousePosition.X;
                            pos[i].Y = mousePosition.Y;
                            if (ausgewähltematerie == 1)
                            {
                                particelart[i] = 1;
                                balltemperatur[i] = 22;
                            }
                            if (ausgewähltematerie == 3)
                            {
                                particelart[i] = 3;
                                balltemperatur[i] = 22;
                            }
                            if (ausgewähltematerie == 4)
                            {
                                particelart[i] = 4;
                                balltemperatur[i] = 200;
                            }
                        }
                        break;
                    }
                }
            }
            if (Mouse.GetState().RightButton == ButtonState.Pressed)
            {
                for (int i = 0; i < ballmenge; i++)
                {
                    if (particel[i] == 0)
                    {
                        for (int j = 0; j < ballmenge; j++)
                        {
                            if (particel[j] == 1)
                            {
                                if (mousePosition.X > pos[j].X - 7 && mousePosition.X < pos[j].X + 7 && mousePosition.Y > pos[j].Y - 7 && mousePosition.Y < pos[j].Y + 7)
                                {
                                    aaaa = 1;
                                }
                            }
                        }
                        if (aaaa == 0)
                        {
                            particel[i] = 1;
                            particel2[i] = 1;
                            aaa++;
                            pos[i].X = mousePosition.X;
                            pos[i].Y = mousePosition.Y;
                            particelart[i] = 2;
                            //balltemperatur[i] = 20;
                            fixeparticelpos[i] = pos[i];
                        }
                        break;
                    }
                }
            }
            if (Keyboard.GetState().IsKeyUp(Keys.Space))
            {
                pausegdrückt = 0;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Space) && pausegdrückt == 0)
            {
                pausegdrückt = 1;
                if (pause == 0)
                {
                    pause = 1;
                }
                else if (pause == 1)
                {
                    pause = 0;
                }
            }
            if (Keyboard.GetState().IsKeyUp(Keys.Up))
            {
                grenzhöhe += 5;
            }
            if (Keyboard.GetState().IsKeyUp(Keys.Down))
            {
                grenzhöhe -= 5;
            }
            if (Keyboard.GetState().IsKeyUp(Keys.W))
            {
                ygrav += 0.001f;
            }
            if (Keyboard.GetState().IsKeyUp(Keys.A))
            {
                xgrav += 0.001f;
            }
            if (Keyboard.GetState().IsKeyUp(Keys.S))
            {
                ygrav -= 0.001f;
            }
            if (Keyboard.GetState().IsKeyUp(Keys.D))
            {
                xgrav -= 0.001f;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.I))
            {
                modus = 0;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.O))
            {
                modus = 1;
            }
            if (pause == 0)
            {
                for (int i = 0; i < ballmenge; i++)
                {
                    balls[i].besetzt = 0;
                    if (particel[i] == 1)
                    {
                        balls[i].besetzt = 1;
                        if (pos[i].Y > grenzhöhe)
                        {
                            speed[i].Y = 0;
                            pos[i].Y = grenzhöhe;
                        }
                        if (pos[i].Y < 10)
                        {
                            speed[i].Y = 0;
                            pos[i].Y = 10;
                        }
                        if (pos[i].X < 10)
                        {
                            speed[i].X = 0;
                            pos[i].X = 10;
                        }
                        if (pos[i].X > 1200)
                        {
                            speed[i].X = 0;
                            pos[i].X = 1200;
                        }
                        if (particelcollidiert[i] == 0)
                        {
                            if (particelart[i] != 5)
                            {
                                speed[i].Y += ygrav;
                                speed[i].X += xgrav;
                            }
                            else
                            {
                                speed[i].Y -= (-100 + balltemperatur[i]) * (ygrav / 10.0f);
                                speed[i].X -= (-100 + balltemperatur[i]) * (xgrav / 10.0f);
                            }
                        }
                        pos[i].Y += speed[i].Y;
                        pos[i].X += speed[i].X;
                        balls[i].Position.X = pos[i].X - 16;
                        balls[i].Position.Y = pos[i].Y - 16;
                        balls[i].type = particelart[i];
                    }
                }

                for (int z = 0; z < 1; z++)
                {
                    for (int i = 0; i < 122; i++)
                    {
                        for (int j = 0; j < 78; j++)
                        {
                            for (int x = 0; x < meng; x++)
                            {
                                particelfeldbesetzt[i, j, x] = 0;
                                particelfeldnummer[i, j, x] = 0;
                            }
                        }
                    }
                    for (int i = 0; i < ballmenge; i++)
                    {
                        if (particel[i] == 1)
                        {
                            particelexplodiert[i] = 0;
                            particelcollidiert[i] = 0;
                            for (int j = 0; j < meng; j++)
                            {
                                if ((int)pos[i].X / 10 >= 0 && (int)pos[i].X / 10 < 122 && (int)pos[i].Y / 10 >= 0 && (int)pos[i].Y / 10 < 78)
                                {
                                    if (particelfeldbesetzt[(int)pos[i].X / 10, (int)pos[i].Y / 10, j] == 0)
                                    {
                                        particelfeldbesetzt[(int)pos[i].X / 10, (int)pos[i].Y / 10, j] = 1;
                                        particelfeldnummer[(int)pos[i].X / 10, (int)pos[i].Y / 10, j] = i;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                    for (int i = 0; i < 122; i++)
                    {
                        for (int j = 77; j >= 0; j--)
                        {
                            for (int x = 0; x < meng; x++)
                            {
                                if (particelfeldbesetzt[i, j, x] == 1)
                                {
                                    for (int y = 0; y < meng; y++)
                                    {
                                        if (particelfeldbesetzt[i, j, y] == 1)
                                        {
                                            int i2 = particelfeldnummer[i, j, x];
                                            int j2 = particelfeldnummer[i, j, y];
                                            Vector2 delta = pos[i2] - pos[j2];
                                            float d = (float)Math.Sqrt(((delta.X) * (delta.X)) + ((delta.Y) * (delta.Y)));
                                            if (d < partgros && d > 0)
                                            {
                                                float zz = balltemperatur[i2];
                                                float zz2 = balltemperatur[j2];
                                                balltemperatur[i2] += (zz2 - zz) / temperaturverbreitung;
                                                balltemperatur[j2] += (zz - zz2) / temperaturverbreitung;
                                                if (particelart[i2] == 3 && particelart[j2] == 4)
                                                {
                                                    particelexplodiert[i2] = 1;
                                                }
                                                if (particelart[j2] == 3 && particelart[i2] == 4)
                                                {
                                                    particelexplodiert[j2] = 1;
                                                }
                                                Vector2 mtd = delta * (((partgros) - d) / d);
                                                float im1 = 1;
                                                float im2 = 1;
                                                pos[i2].X = pos[i2].X + (mtd.X * (im1 / (im1 + im2)));
                                                pos[i2].Y = pos[i2].Y + (mtd.Y * (im1 / (im1 + im2)));
                                                pos[j2].X = pos[j2].X - (mtd.X * (im2 / (im1 + im2)));
                                                pos[j2].Y = pos[j2].Y - (mtd.Y * (im2 / (im1 + im2)));
                                                Vector2 v = speed[i2] - speed[j2];
                                                float f = 2f / (im1 + im2);
                                                Vector2 impulse = mtd * f;
                                                speed[i2] = speed[i2] + ((impulse * im1) / 10);
                                                speed[j2] = speed[j2] - ((impulse * im2) / 10);
                                                if (d < sss)
                                                {
                                                    particelcollidiert[i2] = 1;
                                                    particelcollidiert[j2] = 1;
                                                }
                                            }
                                        }
                                    }
                                    for (int y = 0; y < meng; y++)
                                    {
                                        if (i + 1 < 122)
                                        {
                                            if (particelfeldbesetzt[i + 1, j, y] == 1)
                                            {
                                                int i2 = particelfeldnummer[i, j, x];
                                                int j2 = particelfeldnummer[i + 1, j, y];
                                                Vector2 delta = pos[i2] - pos[j2];
                                                float d = (float)Math.Sqrt(((delta.X) * (delta.X)) + ((delta.Y) * (delta.Y)));
                                                if (d < partgros && d > 0)
                                                {
                                                    float zz = balltemperatur[i2];
                                                    float zz2 = balltemperatur[j2];
                                                    balltemperatur[i2] += (zz2 - zz) / temperaturverbreitung;
                                                    balltemperatur[j2] += (zz - zz2) / temperaturverbreitung;
                                                    if (particelart[i2] == 3 && particelart[j2] == 4)
                                                    {
                                                        particelexplodiert[i2] = 1;
                                                    }
                                                    if (particelart[j2] == 3 && particelart[i2] == 4)
                                                    {
                                                        particelexplodiert[j2] = 1;
                                                    }
                                                    Vector2 mtd = delta * (((partgros) - d) / d);
                                                    float im1 = 1;
                                                    float im2 = 1;
                                                    pos[i2].X = pos[i2].X + (mtd.X * (im1 / (im1 + im2)));
                                                    pos[i2].Y = pos[i2].Y + (mtd.Y * (im1 / (im1 + im2)));
                                                    pos[j2].X = pos[j2].X - (mtd.X * (im2 / (im1 + im2)));
                                                    pos[j2].Y = pos[j2].Y - (mtd.Y * (im2 / (im1 + im2)));
                                                    Vector2 v = speed[i2] - speed[j2];
                                                    float f = 2f / (im1 + im2);
                                                    Vector2 impulse = mtd * f;
                                                    speed[i2] = speed[i2] + ((impulse * im1) / 10);
                                                    speed[j2] = speed[j2] - ((impulse * im2) / 10);
                                                    if (d < sss)
                                                    {
                                                        particelcollidiert[i2] = 1;
                                                        particelcollidiert[j2] = 1;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    for (int y = 0; y < meng; y++)
                                    {
                                        if (j + 1 < 78)
                                        {
                                            if (particelfeldbesetzt[i, j + 1, y] == 1)
                                            {
                                                int i2 = particelfeldnummer[i, j, x];
                                                int j2 = particelfeldnummer[i, j + 1, y];
                                                Vector2 delta = pos[i2] - pos[j2];
                                                float d = (float)Math.Sqrt(((delta.X) * (delta.X)) + ((delta.Y) * (delta.Y)));
                                                if (d < partgros && d > 0)
                                                {
                                                    float zz = balltemperatur[i2];
                                                    float zz2 = balltemperatur[j2];
                                                    balltemperatur[i2] += (zz2 - zz) / temperaturverbreitung;
                                                    balltemperatur[j2] += (zz - zz2) / temperaturverbreitung;
                                                    if (particelart[i2] == 3 && particelart[j2] == 4)
                                                    {
                                                        particelexplodiert[i2] = 1;
                                                    }
                                                    if (particelart[j2] == 3 && particelart[i2] == 4)
                                                    {
                                                        particelexplodiert[j2] = 1;
                                                    }
                                                    Vector2 mtd = delta * (((partgros) - d) / d);
                                                    float im1 = 1;
                                                    float im2 = 1;
                                                    pos[i2].X = pos[i2].X + (mtd.X * (im1 / (im1 + im2)));
                                                    pos[i2].Y = pos[i2].Y + (mtd.Y * (im1 / (im1 + im2)));
                                                    pos[j2].X = pos[j2].X - (mtd.X * (im2 / (im1 + im2)));
                                                    pos[j2].Y = pos[j2].Y - (mtd.Y * (im2 / (im1 + im2)));
                                                    Vector2 v = speed[i2] - speed[j2];
                                                    float f = 2f / (im1 + im2);
                                                    Vector2 impulse = mtd * f;
                                                    speed[i2] = speed[i2] + ((impulse * im1) / 10);
                                                    speed[j2] = speed[j2] - ((impulse * im2) / 10);
                                                    if (d < sss)
                                                    {
                                                        particelcollidiert[i2] = 1;
                                                        particelcollidiert[j2] = 1;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    for (int y = 0; y < meng; y++)
                                    {
                                        if (i + 1 < 122 && j + 1 < 78)
                                        {
                                            if (particelfeldbesetzt[i + 1, j + 1, y] == 1)
                                            {
                                                int i2 = particelfeldnummer[i, j, x];
                                                int j2 = particelfeldnummer[i + 1, j + 1, y];
                                                Vector2 delta = pos[i2] - pos[j2];
                                                float d = (float)Math.Sqrt(((delta.X) * (delta.X)) + ((delta.Y) * (delta.Y)));
                                                if (d < partgros && d > 0)
                                                {
                                                    float zz = balltemperatur[i2];
                                                    float zz2 = balltemperatur[j2];
                                                    balltemperatur[i2] += (zz2 - zz) / temperaturverbreitung;
                                                    balltemperatur[j2] += (zz - zz2) / temperaturverbreitung;
                                                    if (particelart[i2] == 3 && particelart[j2] == 4)
                                                    {
                                                        particelexplodiert[i2] = 1;
                                                    }
                                                    if (particelart[j2] == 3 && particelart[i2] == 4)
                                                    {
                                                        particelexplodiert[j2] = 1;
                                                    }
                                                    Vector2 mtd = delta * (((partgros) - d) / d);
                                                    float im1 = 1;
                                                    float im2 = 1;
                                                    pos[i2].X = pos[i2].X + (mtd.X * (im1 / (im1 + im2)));
                                                    pos[i2].Y = pos[i2].Y + (mtd.Y * (im1 / (im1 + im2)));
                                                    pos[j2].X = pos[j2].X - (mtd.X * (im2 / (im1 + im2)));
                                                    pos[j2].Y = pos[j2].Y - (mtd.Y * (im2 / (im1 + im2)));
                                                    Vector2 v = speed[i2] - speed[j2];
                                                    float f = 2f / (im1 + im2);
                                                    Vector2 impulse = mtd * f;
                                                    speed[i2] = speed[i2] + ((impulse * im1) / 10);
                                                    speed[j2] = speed[j2] - ((impulse * im2) / 10);
                                                    if (d < sss)
                                                    {
                                                        particelcollidiert[i2] = 1;
                                                        particelcollidiert[j2] = 1;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    for (int y = 0; y < meng; y++)
                                    {
                                        if (i - 1 >= 0 && j + 1 < 78)
                                        {
                                            if (particelfeldbesetzt[i - 1, j + 1, y] == 1)
                                            {
                                                int i2 = particelfeldnummer[i, j, x];
                                                int j2 = particelfeldnummer[i - 1, j + 1, y];
                                                Vector2 delta = pos[i2] - pos[j2];
                                                float d = (float)Math.Sqrt(((delta.X) * (delta.X)) + ((delta.Y) * (delta.Y)));
                                                if (d < partgros && d > 0)
                                                {
                                                    float zz = balltemperatur[i2];
                                                    float zz2 = balltemperatur[j2];
                                                    balltemperatur[i2] += (zz2 - zz) / temperaturverbreitung;
                                                    balltemperatur[j2] += (zz - zz2) / temperaturverbreitung;
                                                    if (particelart[i2] == 3 && particelart[j2] == 4)
                                                    {
                                                        particelexplodiert[i2] = 1;
                                                    }
                                                    if (particelart[j2] == 3 && particelart[i2] == 4)
                                                    {
                                                        particelexplodiert[j2] = 1;
                                                    }
                                                    Vector2 mtd = delta * (((partgros) - d) / d);
                                                    float im1 = 1;
                                                    float im2 = 1;
                                                    pos[i2].X = pos[i2].X + (mtd.X * (im1 / (im1 + im2)));
                                                    pos[i2].Y = pos[i2].Y + (mtd.Y * (im1 / (im1 + im2)));
                                                    pos[j2].X = pos[j2].X - (mtd.X * (im2 / (im1 + im2)));
                                                    pos[j2].Y = pos[j2].Y - (mtd.Y * (im2 / (im1 + im2)));
                                                    Vector2 v = speed[i2] - speed[j2];
                                                    float f = 2f / (im1 + im2);
                                                    Vector2 impulse = mtd * f;
                                                    speed[i2] = speed[i2] + ((impulse * im1) / 10);
                                                    speed[j2] = speed[j2] - ((impulse * im2) / 10);
                                                    if (d < sss)
                                                    {
                                                        particelcollidiert[i2] = 1;
                                                        particelcollidiert[j2] = 1;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    for (int y = 0; y < meng; y++)
                                    {
                                        if (i - 1 >= 0)
                                        {
                                            if (particelfeldbesetzt[i - 1, j, y] == 1)
                                            {
                                                int i2 = particelfeldnummer[i, j, x];
                                                int j2 = particelfeldnummer[i - 1, j, y];
                                                Vector2 delta = pos[i2] - pos[j2];
                                                float d = (float)Math.Sqrt(((delta.X) * (delta.X)) + ((delta.Y) * (delta.Y)));
                                                if (d < partgros && d > 0)
                                                {
                                                    float zz = balltemperatur[i2];
                                                    float zz2 = balltemperatur[j2];
                                                    balltemperatur[i2] += (zz2 - zz) / temperaturverbreitung;
                                                    balltemperatur[j2] += (zz - zz2) / temperaturverbreitung;
                                                    if (particelart[i2] == 3 && particelart[j2] == 4)
                                                    {
                                                        particelexplodiert[i2] = 1;
                                                    }
                                                    if (particelart[j2] == 3 && particelart[i2] == 4)
                                                    {
                                                        particelexplodiert[j2] = 1;
                                                    }
                                                    Vector2 mtd = delta * (((partgros) - d) / d);
                                                    float im1 = 1;
                                                    float im2 = 1;
                                                    pos[i2].X = pos[i2].X + (mtd.X * (im1 / (im1 + im2)));
                                                    pos[i2].Y = pos[i2].Y + (mtd.Y * (im1 / (im1 + im2)));
                                                    pos[j2].X = pos[j2].X - (mtd.X * (im2 / (im1 + im2)));
                                                    pos[j2].Y = pos[j2].Y - (mtd.Y * (im2 / (im1 + im2)));
                                                    Vector2 v = speed[i2] - speed[j2];
                                                    float f = 2f / (im1 + im2);
                                                    Vector2 impulse = mtd * f;
                                                    speed[i2] = speed[i2] + ((impulse * im1) / 10);
                                                    speed[j2] = speed[j2] - ((impulse * im2) / 10);
                                                    if (d < sss)
                                                    {
                                                        particelcollidiert[i2] = 1;
                                                        particelcollidiert[j2] = 1;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    for (int y = 0; y < meng; y++)
                                    {
                                        if (i - 1 >= 0 && j - 1 >= 0)
                                        {
                                            if (particelfeldbesetzt[i - 1, j - 1, y] == 1)
                                            {
                                                int i2 = particelfeldnummer[i, j, x];
                                                int j2 = particelfeldnummer[i - 1, j - 1, y];
                                                Vector2 delta = pos[i2] - pos[j2];
                                                float d = (float)Math.Sqrt(((delta.X) * (delta.X)) + ((delta.Y) * (delta.Y)));
                                                if (d < partgros && d > 0)
                                                {
                                                    float zz = balltemperatur[i2];
                                                    float zz2 = balltemperatur[j2];
                                                    balltemperatur[i2] += (zz2 - zz) / temperaturverbreitung;
                                                    balltemperatur[j2] += (zz - zz2) / temperaturverbreitung;
                                                    if (particelart[i2] == 3 && particelart[j2] == 4)
                                                    {
                                                        particelexplodiert[i2] = 1;
                                                    }
                                                    if (particelart[j2] == 3 && particelart[i2] == 4)
                                                    {
                                                        particelexplodiert[j2] = 1;
                                                    }
                                                    Vector2 mtd = delta * (((partgros) - d) / d);
                                                    float im1 = 1;
                                                    float im2 = 1;
                                                    pos[i2].X = pos[i2].X + (mtd.X * (im1 / (im1 + im2)));
                                                    pos[i2].Y = pos[i2].Y + (mtd.Y * (im1 / (im1 + im2)));
                                                    pos[j2].X = pos[j2].X - (mtd.X * (im2 / (im1 + im2)));
                                                    pos[j2].Y = pos[j2].Y - (mtd.Y * (im2 / (im1 + im2)));
                                                    Vector2 v = speed[i2] - speed[j2];
                                                    float f = 2f / (im1 + im2);
                                                    Vector2 impulse = mtd * f;
                                                    speed[i2] = speed[i2] + ((impulse * im1) / 10);
                                                    speed[j2] = speed[j2] - ((impulse * im2) / 10);
                                                    if (d < sss)
                                                    {
                                                        particelcollidiert[i2] = 1;
                                                        particelcollidiert[j2] = 1;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    for (int y = 0; y < meng; y++)
                                    {
                                        if (j - 1 >= 0)
                                        {
                                            if (particelfeldbesetzt[i, j - 1, y] == 1)
                                            {
                                                int i2 = particelfeldnummer[i, j, x];
                                                int j2 = particelfeldnummer[i, j - 1, y];
                                                Vector2 delta = pos[i2] - pos[j2];
                                                float d = (float)Math.Sqrt(((delta.X) * (delta.X)) + ((delta.Y) * (delta.Y)));
                                                if (d < partgros && d > 0)
                                                {
                                                    float zz = balltemperatur[i2];
                                                    float zz2 = balltemperatur[j2];
                                                    balltemperatur[i2] += (zz2 - zz) / temperaturverbreitung;
                                                    balltemperatur[j2] += (zz - zz2) / temperaturverbreitung;
                                                    if (particelart[i2] == 3 && particelart[j2] == 4)
                                                    {
                                                        particelexplodiert[i2] = 1;
                                                    }
                                                    if (particelart[j2] == 3 && particelart[i2] == 4)
                                                    {
                                                        particelexplodiert[j2] = 1;
                                                    }
                                                    Vector2 mtd = delta * (((partgros) - d) / d);
                                                    float im1 = 1;
                                                    float im2 = 1;
                                                    pos[i2].X = pos[i2].X + (mtd.X * (im1 / (im1 + im2)));
                                                    pos[i2].Y = pos[i2].Y + (mtd.Y * (im1 / (im1 + im2)));
                                                    pos[j2].X = pos[j2].X - (mtd.X * (im2 / (im1 + im2)));
                                                    pos[j2].Y = pos[j2].Y - (mtd.Y * (im2 / (im1 + im2)));
                                                    Vector2 v = speed[i2] - speed[j2];
                                                    float f = 2f / (im1 + im2);
                                                    Vector2 impulse = mtd * f;
                                                    speed[i2] = speed[i2] + ((impulse * im1) / 10);
                                                    speed[j2] = speed[j2] - ((impulse * im2) / 10);
                                                    if (d < sss)
                                                    {
                                                        particelcollidiert[i2] = 1;
                                                        particelcollidiert[j2] = 1;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    for (int y = 0; y < meng; y++)
                                    {
                                        if (i + 1 < 122 && j - 1 >= 0)
                                        {
                                            if (particelfeldbesetzt[i + 1, j - 1, y] == 1)
                                            {
                                                int i2 = particelfeldnummer[i, j, x];
                                                int j2 = particelfeldnummer[i + 1, j - 1, y];
                                                Vector2 delta = pos[i2] - pos[j2];
                                                float d = (float)Math.Sqrt(((delta.X) * (delta.X)) + ((delta.Y) * (delta.Y)));
                                                if (d < partgros && d > 0)
                                                {
                                                    float zz = balltemperatur[i2];
                                                    float zz2 = balltemperatur[j2];
                                                    balltemperatur[i2] += (zz2 - zz) / temperaturverbreitung;
                                                    balltemperatur[j2] += (zz - zz2) / temperaturverbreitung;
                                                    if (particelart[i2] == 3 && particelart[j2] == 4)
                                                    {
                                                        particelexplodiert[i2] = 1;
                                                    }
                                                    if (particelart[j2] == 3 && particelart[i2] == 4)
                                                    {
                                                        particelexplodiert[j2] = 1;
                                                    }
                                                    Vector2 mtd = delta * (((partgros) - d) / d);
                                                    float im1 = 1;
                                                    float im2 = 1;
                                                    pos[i2].X = pos[i2].X + (mtd.X * (im1 / (im1 + im2)));
                                                    pos[i2].Y = pos[i2].Y + (mtd.Y * (im1 / (im1 + im2)));
                                                    pos[j2].X = pos[j2].X - (mtd.X * (im2 / (im1 + im2)));
                                                    pos[j2].Y = pos[j2].Y - (mtd.Y * (im2 / (im1 + im2)));
                                                    Vector2 v = speed[i2] - speed[j2];
                                                    float f = 2f / (im1 + im2);
                                                    Vector2 impulse = mtd * f;
                                                    speed[i2] = speed[i2] + ((impulse * im1) / 10);
                                                    speed[j2] = speed[j2] - ((impulse * im2) / 10);
                                                    if (d < sss)
                                                    {
                                                        particelcollidiert[i2] = 1;
                                                        particelcollidiert[j2] = 1;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    for (int i = 0; i < ballmenge; i++)
                    {
                        if (particel[i] == 1)
                        {
                            if (particelexplodiert2[i] == 1 && particelart[i] == 3)
                            {
                                particelexplodiert[i] = 1;
                                particelexplodiert2[i] = 0;
                            }
                        }
                    }
                    aaa = 0;
                    for (int i = 0; i < ballmenge; i++)
                    {
                        if (particel[i] == 1)
                        {
                            aaa++;
                            if (particelart[i] == 1 && balltemperatur[i] >= 100)
                            {
                                particelart[i] = 5;
                            }
                            if (particelart[i] == 5 && balltemperatur[i] < 100)
                            {
                                particelart[i] = 1;
                            }
                            if (particelart[i] == 4 && balltemperatur[i] < 100)
                            {
                                particelexplodiert[i] = 0;
                                particelexplodiert2[i] = 0;
                                particel[i] = 0;
                                balltemperatur[i] = 22;
                                speed[i].X = 0;
                                speed[i].Y = 0;
                            }
                            if (particelart[i] == 2)
                            {
                                pos[i] = fixeparticelpos[i];
                                //balltemperatur[i] = 2200;
                                speed[i].X = 0;
                                speed[i].Y = 0;
                            }
                            if (particelexplodiert[i] == 1)
                            {
                                for (int j = 0; j < ballmenge; j++)
                                {
                                    if (particel[j] == 1)
                                    {

                                        Vector2 delta = pos[i] - pos[j];
                                        float d = (float)Math.Sqrt(((delta.X) * (delta.X)) + ((delta.Y) * (delta.Y)));
                                        if (d < 50)
                                        {
                                            if (d < 30 && particelart[j] == 3)
                                            {
                                                particelexplodiert2[j] = 1;
                                            }
                                            speed[j].X += ((pos[j].X - pos[i].X) / 500) * (50 - d);
                                            speed[j].Y += ((pos[j].Y - pos[i].Y) / 500) * (50 - d);
                                        }
                                    }
                                }
                                particelexplodiert[i] = 0;
                                particelexplodiert2[i] = 0;
                                particel[i] = 0;
                                speed[i].X = 0;
                                speed[i].Y = 0;
                            }
                        }
                    }
                }
            }
            foreach (var ball in balls)
                ball.Update();
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);
            GraphicsDevice.SetRenderTarget(metaballTarget);
            GraphicsDevice.Clear(Color.Transparent);

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive);
            for (int i = 0; i < ballmenge; i++)
            {
                if (balls[i].besetzt == 1)
                {

                    if (particelart[i] == 1)
                    {
                        spriteBatch.Draw(metaballTextures[0], balls[i].Position, null, Color.White, 0f, Vector2.Zero, MetaballScale, SpriteEffects.None, 0f);
                    }
                    if (particelart[i] == 2)
                    {
                        spriteBatch.Draw(metaballTextures[4], balls[i].Position, null, Color.White, 0f, Vector2.Zero, MetaballScale, SpriteEffects.None, 0f);
                    }
                    if (particelart[i] == 3)
                    {
                        spriteBatch.Draw(metaballTextures[1], balls[i].Position, null, Color.White, 0f, Vector2.Zero, MetaballScale, SpriteEffects.None, 0f);
                    }
                    if (particelart[i] == 4)
                    {
                        spriteBatch.Draw(metaballTextures[2], balls[i].Position, null, Color.White, 0f, Vector2.Zero, MetaballScale, SpriteEffects.None, 0f);
                    }
                    if (particelart[i] == 5)
                    {
                        spriteBatch.Draw(metaballTextures[3], balls[i].Position, null, Color.White, 0f, Vector2.Zero, MetaballScale, SpriteEffects.None, 0f);
                    }
                }
            }
            spriteBatch.End();

            GraphicsDevice.SetRenderTarget(null);
            GraphicsDevice.Clear(Color.Black);

            // draw a faint glow behind the metaballs. We accomplish this by rendering the 
            // metaball texture without threshholding it. This is purely aesthetic.
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive);
            foreach (var ball in balls)
            {
                Color tint = Color.LightBlue;
                //spriteBatch.Draw(ball.Texture, ball.Position, null, tint, 0f, Vector2.Zero, MetaballScale, SpriteEffects.None, 0f);
            }
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Opaque, null, null, null, effect);
            if (modus == 1)
            {
                spriteBatch.Draw(metaballTarget, Vector2.Zero, Color.White);
            }
            spriteBatch.End();





            spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, Matrix.CreateTranslation(new Vector3(0, 0, 0f)));
            if (modus == 0)
            {
                for (int i = 0; i < ballmenge; i++)
                {
                    if (particel[i] == 1)
                    {
                        if (particelart[i] == 1)
                        {
                            spriteBatch.Draw(wassertex, new Vector2(pos[i].X - partgros / 2 - 3, pos[i].Y - partgros / 2 - 3), Color.White);
                        }
                        if (particelart[i] == 2)
                        {
                            spriteBatch.Draw(festesparticeltex, new Vector2(pos[i].X - partgros / 2 - 3, pos[i].Y - partgros / 2 - 3), Color.White);
                        }
                        if (particelart[i] == 3)
                        {
                            spriteBatch.Draw(explosivetex, new Vector2(pos[i].X - partgros / 2 - 3, pos[i].Y - partgros / 2 - 3), Color.White);
                        }
                        if (particelart[i] == 4)
                        {
                            spriteBatch.Draw(firetex, new Vector2(pos[i].X - partgros / 2 - 3, pos[i].Y - partgros / 2 - 3), Color.White);
                        }
                        if (particelart[i] == 5)
                        {
                            spriteBatch.Draw(steamtex, new Vector2(pos[i].X - partgros / 2 - 3, pos[i].Y - partgros / 2 - 3), Color.White);
                        }
                    }
                }
            }

            spriteBatch.DrawString(b, xgrav.ToString(), new Vector2(300, 300), Color.Red);
            spriteBatch.DrawString(b, ygrav.ToString(), new Vector2(300, 320), Color.Red);
            spriteBatch.DrawString(b, aaa.ToString(), new Vector2(300, 340), Color.Red);
            spriteBatch.DrawString(b, jjj.ToString(), new Vector2(300, 360), Color.Red);
            spriteBatch.End();
            // TODO: Add your drawing code here
            base.Draw(gameTime);
        }
    }
}
