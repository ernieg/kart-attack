using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace KartAttack
{
    static class Utilities
    {
        public static ContentManager content;

        public static List<float> damageHandicaps;

        public static List<Boom> booms;

        public static Texture2D ShieldTexture;
        public static Texture2D lineTexture;
        public static Texture2D energyLineTexture;
        public static Texture2D healthLineTexture;
        public static Texture2D backgroundTexture;

        public static Texture2D laserSightTexture;

        public static List<Texture2D> blueKarts;
        public static List<Texture2D> orangeKarts;
        public static List<Texture2D> greenKarts;
        public static List<Texture2D> turquoiseKarts;
        public static List<Texture2D> bullets;
        public static List<Texture2D> rockets;
        public static List<Texture2D> shotgunPellets;
        public static List<Texture2D> coinTextures;
        public static List<Texture2D> blueCoinTextures;
        public static List<Texture2D> blueMines;
        public static List<Texture2D> orangeMines;
        public static List<Texture2D> greenMines;
        public static List<Texture2D> turqoiseMines;
        public static List<Texture2D> shockwaves;
        public static List<Texture2D> plasma;
        public static List<Texture2D> stunBullets;
        public static List<Texture2D> oilSlicks;
        public static List<Texture2D> bulletBooms;
        public static List<Texture2D> blasts;
        public static List<Texture2D> poofs;
        public static List<Texture2D> dustTrails;
        public static List<Texture2D> stuns;
        public static List<Texture2D> shocks;
        public static List<Texture2D> spirits;
        public static List<Texture2D> shieldAnimationTextures;
        public static List<Texture2D> laserTextures;

        public static SpriteFont coinNumberFont;
        public static SpriteFont menuFont;
        public static SpriteFont gameFont;
        public static SpriteFont countdownFont;

        public static SoundEffect RocketSound;
        public static SoundEffect RocketExplosion;
        public static SoundEffect BulletSound;
        public static SoundEffect ShotgunPelletSound;
        public static SoundEffect CoinSound;
        public static SoundEffect SpeedBoostSound;
        public static SoundEffect MineHitSound;
        public static SoundEffect ShockwaveSound;
        public static SoundEffect PlasmaSound;
        public static SoundEffect StunSound;
        public static SoundEffect OilSound;
        public static SoundEffect ShieldAbsorbSound;
        public static SoundEffect LaserFireSound;
        public static SoundEffect LaserChargeSound;

        public static List<Song> backgroundTracks;
        public static int backgroundTrackIndex = 0;

        public static Song menuMusic;

        public static int blueMinesInPlay;
        public static int orangeMinesInPlay;
        public static int greenMinesInPlay;
        public static int turqoiseMinesInPlay;

        public static int blueOilSlicksInPlay;
        public static int orangeOilSlicksInPlay;
        public static int greenOilSlicksInPlay;
        public static int turqoiseOilSlicksInPlay;

        public static void loadContent()
        {
            gameFont = content.Load<SpriteFont>("gamefont");
            countdownFont = content.Load<SpriteFont>("countdown");
            backgroundTexture = content.Load<Texture2D>("bigBackGroundScan");
            //backgroundTexture = content.Load<Texture2D>("KartAttackBackground");
            //backgroundTexture = content.Load<Texture2D>("backgroundNoah");

            laserSightTexture = content.Load<Texture2D>("AbilityEffects/laser_sight_1");

            booms = new List<Boom>();

            blueKarts = new List<Texture2D>();
            orangeKarts = new List<Texture2D>();
            greenKarts = new List<Texture2D>();
            turquoiseKarts = new List<Texture2D>();
            bullets = new List<Texture2D>();
            rockets = new List<Texture2D>();
            shotgunPellets = new List<Texture2D>();
            coinTextures = new List<Texture2D>();
            blueCoinTextures = new List<Texture2D>();
            blueMines = new List<Texture2D>();
            orangeMines = new List<Texture2D>();
            greenMines = new List<Texture2D>();
            turqoiseMines = new List<Texture2D>();
            shockwaves = new List<Texture2D>();
            plasma = new List<Texture2D>();
            stunBullets = new List<Texture2D>();
            oilSlicks = new List<Texture2D>();
            bulletBooms = new List<Texture2D>();
            spirits = new List<Texture2D>();
            shieldAnimationTextures = new List<Texture2D>();
            blasts = new List<Texture2D>();
            stuns = new List<Texture2D>();
            shocks = new List<Texture2D>();
            poofs = new List<Texture2D>();
            dustTrails = new List<Texture2D>();
            laserTextures = new List<Texture2D>();

            backgroundTracks = new List<Song>();

            for (int i=1; i<=1; i++) // everything that only has one frame...
            {    
                bullets.Add(content.Load<Texture2D>("Projectiles/projectile_pistol_" + i.ToString()));
                rockets.Add(content.Load<Texture2D>("Projectiles/projectile_rocket_" + i.ToString()));
                shotgunPellets.Add(content.Load<Texture2D>("Projectiles/projectile_shotgun_" + i.ToString()));
                shockwaves.Add(content.Load<Texture2D>("Projectiles/projectile_shockwave_" + i.ToString()));
                plasma.Add(content.Load<Texture2D>("Projectiles/projectile_plasma_" + i.ToString()));
                stunBullets.Add(content.Load<Texture2D>("Projectiles/projectile_stun_bullet_" + i.ToString()));
                oilSlicks.Add(content.Load<Texture2D>("Projectiles/projectile_oil_slick_" + i.ToString()));
                bulletBooms.Add(content.Load<Texture2D>("Explosions/bulletBam_" + i.ToString()));
                laserTextures.Add(content.Load<Texture2D>("Projectiles/projectile_laser_" + i.ToString()));
            }

            for (int i = 1; i <= 2; i++) // everything that has two frames...
            { } // CONTINUE DOING THINGS LIKE THIS!

            for (int i = 1; i <= 3; i++) // everything that has three frames...
            { }

            for (int i = 1; i <= 4; i++) // everything that has four frames...
            {
                blueKarts.Add(content.Load<Texture2D>("Karts/kart_blue_" + i.ToString()));
                orangeKarts.Add(content.Load<Texture2D>("Karts/kart_orange_" + i.ToString()));
                greenKarts.Add(content.Load<Texture2D>("Karts/kart_green_" + i.ToString()));
                turquoiseKarts.Add(content.Load<Texture2D>("Karts/kart_turqoise_" + i.ToString()));

                blueMines.Add(content.Load<Texture2D>("Projectiles/projectile_mine_blue_" + i.ToString()));
                orangeMines.Add(content.Load<Texture2D>("Projectiles/projectile_mine_orange_" + i.ToString()));
                greenMines.Add(content.Load<Texture2D>("Projectiles/projectile_mine_green_" + i.ToString()));
                turqoiseMines.Add(content.Load<Texture2D>("Projectiles/projectile_mine_turqoise_" + i.ToString()));

                shocks.Add(content.Load<Texture2D>("Explosions/shock_" + i.ToString()));

                dustTrails.Add(content.Load<Texture2D>("Karts/dust_trail_" + i.ToString()));
            }

            for (int i = 1; i <= 5; i++)
            {
                stuns.Add(content.Load<Texture2D>("Explosions/stun_" + i.ToString()));
            }

            for (int i = 1; i <= 6; i++)
            {
                shieldAnimationTextures.Add(content.Load<Texture2D>("AbilityEffects/shield_" + i.ToString()));
            }

            for (int i = 1; i <= 7; i++)
            {
                coinTextures.Add(content.Load<Texture2D>("Coins/coin_" + i.ToString()));
                blueCoinTextures.Add(content.Load<Texture2D>("Coins/bluecoin_" + i.ToString()));
                poofs.Add(content.Load<Texture2D>("Explosions/poof_" + i.ToString()));
            }

            for (int i = 1; i <= 10; i++)
            {
                blasts.Add(content.Load<Texture2D>("Explosions/blast_" + i.ToString()));
            }

            for (int i = 1; i <= 14; i++)
            {
                //spirits.Add(content.Load<Texture2D>("Karts/star_" + i.ToString()));
            }

            for (int i = 1; i <= 16; i++)
            {
                spirits.Add(content.Load<Texture2D>("Karts/Spirit_" + i.ToString()));
            }

               //ShieldTexture = content.Load<Texture2D>("AbilityEffects/debug_shield_1");
            ShieldTexture = content.Load<Texture2D>("AbilityEffects/shield_1");

            energyLineTexture = content.Load<Texture2D>("EnergyLine");
            healthLineTexture = content.Load<Texture2D>("HealthLine");
            coinNumberFont = content.Load<SpriteFont>("menufont");

            RocketSound = content.Load<SoundEffect>("SoundEffects/rocket_launcher_1");
            RocketExplosion = content.Load<SoundEffect>("SoundEffects/rocket_hit_1");
            BulletSound = content.Load<SoundEffect>("SoundEffects/pistol_1");
            ShotgunPelletSound = content.Load<SoundEffect>("SoundEffects/shotgun_1");
            CoinSound = content.Load<SoundEffect>("SoundEffects\\coin_collide_1");
            SpeedBoostSound = content.Load<SoundEffect>("SoundEffects/vroom_1");
            MineHitSound = content.Load<SoundEffect>("SoundEffects/mine_hit_1");
            ShockwaveSound = content.Load<SoundEffect>("SoundEffects/shockwave_1");
            PlasmaSound = content.Load<SoundEffect>("SoundEffects/plasma_gun_1");
            StunSound = content.Load<SoundEffect>("SoundEffects/stun_gun_1");
            OilSound = content.Load<SoundEffect>("SoundEffects/oil_splat_1");
            ShieldAbsorbSound = content.Load<SoundEffect>("SoundEffects/shield_hit_1");
            LaserFireSound = content.Load<SoundEffect>("SoundEffects/laser_fire_1");
            LaserChargeSound = content.Load<SoundEffect>("SoundEffects/laser_charge_1");

            // add other songs here
            backgroundTracks.Add(content.Load<Song>("BackgroundMusic/bg_track_1"));
            backgroundTracks.Add(content.Load<Song>("BackgroundMusic/bg_track_2"));
            backgroundTracks.Add(content.Load<Song>("BackgroundMusic/bg_track_3"));
            backgroundTracks.Add(content.Load<Song>("BackgroundMusic/bg_track_4"));
            backgroundTracks.Add(content.Load<Song>("BackgroundMusic/bg_track_5"));
            backgroundTracks.Add(content.Load<Song>("BackgroundMusic/bg_track_6"));

            menuFont = content.Load<SpriteFont>("menufont");
        }

        public static Random random = new Random(DateTime.Now.Millisecond);

        private static List<Base> bases;

        public static void loadNewBases(List<Base> inBases)
        {
            bases = inBases;
        }

        public static Base getBase(Color color)
        {
            foreach (Base b in bases)
            {
                if (b.Team == color)
                    return b;
            }
            // Hopefully it never reaches this part of the code.
            return new Base(Vector2.Zero, Color.Black);
        }

        public static List<Coin> coins;

        public static void loadCoins(List<Coin> inCoins)
        {
            coins = inCoins;
        }

        public static void AddNewCoins(Coin coin)
        {
            coins.Add(coin);
        }

        public static Ability getAbility(string abilityName)
        {
            switch (abilityName)
            {
                case "Rocket":
                        return new RocketLauncher();
                case "Pistol":
                        return new Pistol();
                case "MiniGun":
                        return new Minigun();
                case "Mine Layer":
                        return new MineLayer();
                case "Shield":
                        return new Shield();
                case "Shockwave":
                        return new ShockwaveGenerator();
                case "Shotgun":
                        return new Shotgun();
                case "Energy Pack":
                        return new EnergyPack();
                case "Health Pack":
                        return new HealthPack();
                case "Boost":
                        return new SpeedBoost();
                case "Stun Gun":
                        return new StunGun();
                case "Plasma Gun":
                        return new PlasmaGun();
                case "OilSlick":
                        return new OilDropper();
                case "Laser":
                        return new BurstLaser();
                default:
                        return null;
            };
        }

        // asserts on failure
        // returns if the kart info is valid
        public static void checkValidKartInformation(KartInformation kart)
        {
            List<string> validWeapons = new List<string>();
            List<string> validAbilities = new List<string>();
            bool flagLeft = false;
            bool flagRight = false;
            bool flagAbility = false;

            validWeapons.Add("Rocket");
            validWeapons.Add("Pistol");
            validWeapons.Add("MiniGun");
            validWeapons.Add("Shotgun");
            validWeapons.Add("Stun Gun");
            validWeapons.Add("Plasma Gun");
            validWeapons.Add("Laser");

            validAbilities.Add("Mine Layer");
            validAbilities.Add("Shield");
            validAbilities.Add("Shockwave");
            validAbilities.Add("Energy Pack");
            validAbilities.Add("Health Pack");
            validAbilities.Add("Boost");
            validAbilities.Add("OilSlick");

            Utilities.assert(kart.Name.Length > 0);
            Utilities.assert(kart.Cost >= 0 && kart.Cost < 500);
            Utilities.assert(kart.Speed > 0.0f && kart.Speed < 0.2f);

            // check weapons
            for (int i = 0; i < validWeapons.Count; i++)
            {
                if (validWeapons[i] == kart.Weapons[0])
                {
                    flagLeft = true;
                }

                if (validWeapons[i] == kart.Weapons[1])
                {
                    flagRight = true;
                }
            }

            Utilities.assert(flagLeft && flagRight);

            // check abilities
            for (int i = 0; i < validAbilities.Count; i++)
            {
                if (validAbilities[i] == kart.Ability)
                {
                    flagAbility = true;
                }
            }
            
            Utilities.assert(flagAbility);
        }

        // I made this because the built-in C# assert() only works in debug mode
        public static void assert(bool condition)
        {
            if (condition == false)
            {
                throw new FormatException();
            }
        }

        //Game Information
        public static PlayerKartInfo[] playerKartInformation;
        public static int numRounds = 3;
        public static int timePerRound = 3;
        public static int coinsToStart = 100;
        public static int[] roundsWon;
        public static List<int> winningTeams;
        public static List<Kart> karts;
    }
}
