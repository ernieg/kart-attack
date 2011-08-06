using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;

namespace KartAttack
{
    // which side of the kart we want to attach abilities
    enum KartSides { FRONT = 0, LEFT, REAR, RIGHT };

    class Kart : GameObject
    {
        public List<Ability> abilities { get; protected set; } // front -> 0, left -> 1, rear -> 2, right -> 3
        List<Attribute> attributes;
        public int NumCoins { get; set; }
        public float Health { get; set; }
        public float Energy { get; set; }
        public Color Team { get; protected set; }
        public float Weight { get; protected set; } // use 0.07 for slowest and 0.17 for the fastest

        // where we spawn the bullets on each side of kart
        public List<Vector2> ProjectileSpawnPositions { get; protected set; }

        private float timePassedSinceCollision;

        // are the shields on or not?
        public bool shields_active { get; set; }

        private List<Texture2D> WeaponTextures;

        private float maxHealth;
        private float maxEnergy;

        private bool dead = true;
        public int ID;
        public int[] kills;
        public int[] deaths;
        public int totalCoins;

        public bool drawLaserSight;

        public bool wasColliding;
        private bool collided;
        private bool hitByAnother;
        private float timeSinceCollided;
        private float timeToVibrate;
        private bool hitByProjectile;

        private bool firstSpawn = true;


        // in milliseconds
        // one counter for each side, indexed like above
        // this is used to control the firing rate of full-auto weapons and the cooldown for semi-auto weapons
        private List<int> timePassedForShooting;
        private int timePassedForShields;

        private int shieldAnimationFrameNum;
        private int timePassedForShieldAnimation;

        private int timePassedForDustTrail;

        #region Controller Input
        // Controler input
        private bool forward;
        private bool backward;
        private bool a;
        private bool b;
        private bool x;
        private bool y;
        private bool rightTrigger;
        private bool leftTrigger;
        private bool previous_y;
        private bool previous_x;
        private bool previous_a;
        private bool previous_b;
        private bool previous_leftTrigger;
        private bool previous_rightTrigger;
        private bool stickPressed;
        private Vector2 targetDirection;
        private Vector2 movement;
        #endregion

        public Kart(Color color, int id, float weight, float health = 100, float energy = 100, float angle = (float)Math.PI/2.0f)
            : base(Utilities.getBase(color).Bounds.Center - new Vector2(14, 23), 28, 45, angle, 4)
        {
            Team = color;
            Health = health;
            Energy = energy;
            maxHealth = health;
            maxEnergy = energy;
            Weight = weight;
            NumCoins = 0;
            timePassedSinceCollision = 3000;
            abilities = new List<Ability>(4); // for the four sides of the kart
            attributes = new List<Attribute>();
            ID = id;
            kills = new int[4];
            deaths = new int[4];
            for (int i = 0; i < 4; i++)
            {
                kills[i] = 0;
                deaths[i] = 0;
            }
            totalCoins = 0;

            // Controller defaults
            forward = false;
            backward = false;
            a = false;
            b = false;
            x = false;
            y = false;
            leftTrigger = false;
            rightTrigger = false;
            previous_y = false;
            previous_x = false;
            previous_a = false;
            previous_b = false;
            previous_leftTrigger = false;
            previous_rightTrigger = false;
            targetDirection = new Vector2(1, 0);

            drawLaserSight = false;

            shields_active = false;
            WeaponTextures = new List<Texture2D>(4);
            
            timePassedForShooting = new List<int>(4);
            timePassedForShooting.Add(30000);
            timePassedForShooting.Add(30000);
            timePassedForShooting.Add(30000);
            timePassedForShooting.Add(30000);

            timePassedForShields = 0;
            shieldAnimationFrameNum = 0;
            timePassedForShieldAnimation = 0;

            timePassedForDustTrail = 0;

            ProjectileSpawnPositions = new List<Vector2>(4);
            ProjectileSpawnPositions.Add(Bounds.UpperLeftCorner());
            ProjectileSpawnPositions.Add(Bounds.UpperLeftCorner());
            ProjectileSpawnPositions.Add(Bounds.UpperLeftCorner());
            ProjectileSpawnPositions.Add(Bounds.UpperLeftCorner());

            // add nothing for the front, left, rear, and right sides
            abilities.Add(null);
            abilities.Add(null);
            abilities.Add(null);
            abilities.Add(null);

            // always attach a pistol to each side of the kart for now
            AttachAbility(new RocketLauncher(), KartSides.FRONT);
            AttachAbility(new PlasmaGun(), KartSides.LEFT);
            AttachAbility(new ShockwaveGenerator(), KartSides.REAR);
            AttachAbility(new Minigun(), KartSides.RIGHT);

            wasColliding = false;
            collided = false;
            timeSinceCollided = 0;
            timeToVibrate = 100;
            hitByAnother = false;
            hitByProjectile = false;
        }

        public void LoadContent(ContentManager contentManager, GraphicsDevice graphicsDevice)
        {
            Utilities.lineTexture = new Texture2D(graphicsDevice, 1, 1, false, SurfaceFormat.Color);
            Utilities.lineTexture.SetData(new[] { Color.White });

            // front side
            WeaponTextures.Add(contentManager.Load<Texture2D>(abilities[(int)KartSides.FRONT].GetAssetName()));
            // left
            WeaponTextures.Add(contentManager.Load<Texture2D>(abilities[(int)KartSides.LEFT].GetAssetName()));
            // rear
            WeaponTextures.Add(contentManager.Load<Texture2D>(abilities[(int)KartSides.REAR].GetAssetName()));
            // right
            WeaponTextures.Add(contentManager.Load<Texture2D>(abilities[(int)KartSides.RIGHT].GetAssetName()));

            if (Team == Color.Blue)
                AnimationTextures = Utilities.blueKarts;
            else if (Team == Color.Orange)
                AnimationTextures = Utilities.orangeKarts;
            else if (Team == Color.Green)
                AnimationTextures = Utilities.greenKarts;
            else // (Team == Color.Turquoise)
                AnimationTextures = Utilities.turquoiseKarts;

        }

        public override bool Collide(GameObject o)
        {
            if (dead)
                return false;
            if (base.Collide(o))
            {
                if (o.GetType() == typeof(Bullet))
                {
                    if (((Projectile)o).Team == Team)
                        return false;
                    timePassedSinceCollision = 70.0f;
                    HitByProjectile((Projectile)o);
                }
                else if (o.GetType() == typeof(ShotgunPellet))
                {
                    if (((Projectile)o).Team == Team)
                        return false;
                    timePassedSinceCollision = 50.0f;
                    HitByProjectile((Projectile)o);
                }
                else if (o.GetType() == typeof(Rocket))
                {
                    if (((Projectile)o).Team == Team)
                        return false;
                    timePassedSinceCollision = -600.0f;
                    HitByProjectile((Projectile)o);
                }
                else if (o.GetType() == typeof(Mine))
                {
                    if (((Projectile)o).Team == Team)
                        return false;
                    timePassedSinceCollision = -600.0f;
                    HitByProjectile((Projectile)o);
                }
                else if (o.GetType() == typeof(Shockwave))
                {
                    if (((Projectile)o).Team == Team)
                        return false;
                    timePassedSinceCollision = -900.0f;
                    HitByProjectile((Projectile)o);
                }
                else if (o.GetType() == typeof(Plasma))
                {
                    if (((Projectile)o).Team == Team)
                        return false;
                    timePassedSinceCollision = 70.0f;
                    HitByProjectile((Projectile)o);
                }
                else if (o.GetType() == typeof(StunBullet))
                {
                    if (((Projectile)o).Team == Team)
                        return false;
                    timePassedSinceCollision = -900.0f;
                    HitByProjectile((Projectile)o);
                }
                else if (o.GetType() == typeof(OilSlick))
                {
                    if (((Projectile)o).Team == Team)
                        return false;
                    timePassedSinceCollision = -1400.0f;
                    HitByProjectile((Projectile)o);
                }
                else if (o.GetType() == typeof(LaserBeam))
                {
                    if (((Projectile)o).Team == Team)
                        return false;
                    timePassedSinceCollision = 0.0f;
                    HitByProjectile((Projectile)o);
                }
                // else if ()   PUT THE OTHER TYPES OF PROJECTILES HERE ... unfortunately it looks like you have to check for same team for each type
                // NOTE: the "timePassedSinceCollision" is my hack to put in stans.  Players can't move until their timePassedSinceCollision is above 100, and it's measured in milliseconds
                //       so if you want a 1/2 second stun, make timePassedSinceCollision -400.
                return true;
            }
            return false;
        }

        private float dist2(Vector2 v1, Vector2 v2)
        {
            return Math.Abs(v1.X - v2.X) + Math.Abs(v1.Y - v2.Y);
        }

        public void Move(float timeDelta, List<GameObject> stuff)
        {
            collided = false;
            if (timePassedSinceCollision < 10000.0f)
                timePassedSinceCollision += timeDelta;
            if (dead)
            {
                if (timePassedSinceCollision < 3000.0f)
                    Move(timeDelta * Velocity);
                else
                {
                    bool dontSpawn = false;
                    foreach (GameObject o in stuff)
                    {
                        if (o.GetType() == typeof(Kart))
                            if (o != this && base.Collide(o) && !((Kart)o).dead)
                            {
                                if (((Kart)o).Team == this.Team)
                                    dontSpawn = true; // team mate there, don't spawn
                                else
                                    ((Kart)o).TakeDamage(10001.0f, this); // spawn kill
                            } 
                    }
                    if (dontSpawn)
                        return;
                    //ResetPosition(Utilities.getBase(Team).Bounds.Center - Bounds.Origin);
                    Velocity = Vector2.Zero;
                    dead = false;
                    Health = maxHealth;
                    Energy = maxEnergy;
                    Utilities.booms.Add(new Boom(Utilities.poofs, 200, new Vector2(Utilities.getBase(Team).Bounds.Center.X - 40, Utilities.getBase(Team).Bounds.Center.Y - 40), 80, 80, 7, 200/7));
                }
                return;
            }

            if (base.Collide(Utilities.getBase(Team)))
            {
                Health += timeDelta * 0.05f;
                Energy += timeDelta * 0.05f;
                if (Health > maxHealth)
                    Health = maxHealth;
                if (Energy > maxEnergy)
                    Energy = maxEnergy;
            }

            //movement = Vector2.Zero;
            if (stickPressed)
                movement = new Vector2((float)Math.Sin(Angle), (float)-Math.Cos(Angle));
            else
                movement = Vector2.Zero;         
            
            // weight is 0.12 is the normal fast speed we were testing with
            if (timePassedSinceCollision > 100.0f)
            {
                Velocity += Weight * movement * timeDelta;                    // acceleration based on controller // 0.15 was original
                Velocity *= (float)Math.Pow(0.06, timeDelta / 1000.0);       // apply the friction
            }
            // 0.03 was medium, 0.01 was slow, 0.05 was fast

            // This will limit the max speed of karts.  Hopefully eliminating really bad collisions
            const float MAX_SPEED = 50.0f;
            if (Velocity.X > MAX_SPEED)
                Velocity = new Vector2(MAX_SPEED, Velocity.Y);
            else if (Velocity.Y > MAX_SPEED)
                Velocity = new Vector2(Velocity.X, MAX_SPEED);
            if (Velocity.X < -MAX_SPEED)
                Velocity = new Vector2(-MAX_SPEED, Velocity.Y);
            else if (Velocity.Y < -MAX_SPEED)
                Velocity = new Vector2(Velocity.X, -MAX_SPEED);

            const float COLLISION_DAMPER = 0.95f;

            Vector2 v = Velocity;
            v.Y = 0.0f;
            Move(timeDelta * 0.012f * v);                       // try moving car in X direction
            foreach (GameObject o in stuff)
            {
                if (o == this) continue;
                if (o.GetType() == typeof(Kart))
                    if (((Kart)o).dead)
                        continue;
                if (base.Collide(o))
                {
                    collided = true;
                    if (timePassedSinceCollision > 0.0f)
                        timePassedSinceCollision = 0.0f;
                    if (o.GetType() == typeof(Kart))
                    {
                        if (((Kart)o).timePassedSinceCollision > 0.0f)
                            ((Kart)o).timePassedSinceCollision = 0.0f;
                        ((Kart)o).hitByAnother = true;
                        o.Velocity = new Vector2(0.6f * Velocity.X, Velocity.Y);
                        Velocity = new Vector2(-0.6f * Velocity.X, Velocity.Y);

                    }
                    else // the general case (hitting a wall or something unmoveable)
                        Velocity = new Vector2(-COLLISION_DAMPER * Velocity.X, Velocity.Y);
                    Move(timeDelta * 0.012f * -v); // move the car back
                    break;
                }
            }

            v = Velocity;
            v.X = 0.0f;
            Move(timeDelta * 0.012f * v);                       // try moving car in Y direction
            foreach (GameObject o in stuff)
            {
                if (o == this) continue;
                if (o.GetType() == typeof(Kart))
                    if (((Kart)o).dead)
                        continue;
                if (base.Collide(o)) // if (o.GetType() == typeof(Kart))
                {
                    collided = true;
                    if (timePassedSinceCollision > 0.0f)
                        timePassedSinceCollision = 0.0f;
                    if (o.GetType() == typeof(Kart))
                    {
                        if (((Kart)o).timePassedSinceCollision > 0.0f) 
                            ((Kart)o).timePassedSinceCollision = 0.0f;
                        ((Kart)o).hitByAnother = true;
                        o.Velocity = new Vector2(Velocity.X, 0.6f * Velocity.Y);
                        Velocity = new Vector2(Velocity.X, -0.6f * Velocity.Y);

                    }
                    else // the general case (hitting a wall or something unmoveable)
                        Velocity = new Vector2(Velocity.X, -COLLISION_DAMPER * Velocity.Y);
                    Move(timeDelta * 0.012f * -v); // move the car back
                }
            }


            // calculate direction to turn
            float pi = (float)Math.PI;
            float targetAngle = (float)Math.Atan2(targetDirection.X, -targetDirection.Y); // Should calculate the angle the controller is facing
            if (targetAngle < 0)
                targetAngle += 2 * (float)Math.PI;
            float turnDirection;
            if (targetAngle > Angle)
            {
                if (targetAngle - Angle < pi)
                    turnDirection = 1.0f;
                else
                    turnDirection = -1.0f;
            }
            else
            {
                if (Angle - targetAngle < pi)
                    turnDirection = -1.0f;
                else
                    turnDirection = 1.0f;
            }

            // try to turn car
            const float ROTATION_SPEED = 0.015f;
            float rotatedAmount;
            if (Math.Abs(Angle - targetAngle) < ROTATION_SPEED * timeDelta)
            { // We're close enough, just rotate to that angle
                rotatedAmount = targetAngle - Angle;
                Angle = targetAngle;
            }
            else
            {
                rotatedAmount = ROTATION_SPEED * turnDirection * timeDelta;
                Angle += ROTATION_SPEED * turnDirection * timeDelta;
            }
            if (Angle < 0)
                Angle += 2 * (float)Math.PI;
            else if (Angle > 2 * Math.PI)
                Angle -= 2 * (float)Math.PI;

            // check for collisions and rotate car back until it's no longer colliding
            foreach (GameObject o in stuff)
            {
                if (o == this) continue;
                if (o.GetType() == typeof(Kart))
                    if (((Kart)o).dead)
                        continue;
                int counter = 0;
                while (base.Collide(o) && counter++ < 75)
                    Angle -= 0.2f * ROTATION_SPEED * turnDirection * timeDelta;
            }
        }

        public override void Update(float timeDelta)
        {
            // drain the kart's energy if the shields are being used
            if (shields_active)
            {
                timePassedForShields += (int)timeDelta;

                // rate of energy drain is 40 units per sec or 1 unit per 25 msec
                if (timePassedForShields >= 25)
                {
                    timePassedForShields = 0;

                    Energy -= 1.0f;

                    if (Energy < 0.0f)
                        Energy = 0.0f;
                }
            }

            if (wasColliding)
            {
                if (timeSinceCollided > timeToVibrate)
                {
                    timeSinceCollided = 0;
                    wasColliding = false;
                }
                else
                {
                    timeSinceCollided += timeDelta;
                }
            }

            if (!wasColliding && (collided || hitByAnother || hitByProjectile))
            {
                hitByAnother = false;
                hitByProjectile = false;
                wasColliding = true;
                collided = false;
                timeSinceCollided += timeDelta;
            }

            // fire weapons here?
            Energy += (0.008f * timeDelta);
            if (Energy > maxEnergy)
            {
                Energy = maxEnergy;
            }
        }

        public override void Draw(SpriteBatch spriteBatch, int millisElapsed)
        {
            if (dead && !firstSpawn)
            {
                /*
                int num = (int)timePassedSinceCollision / 56;
                spriteBatch.Draw(Utilities.spirits[(num % 13) + 1], Bounds.UpperLeftCorner(), null, Color.White, Angle, Vector2.Zero, 1.0f, SpriteEffects.None, 0);
                */
                int num = (int)timePassedSinceCollision / 64;
                spriteBatch.Draw(Utilities.spirits[(num % 15) + 1], Bounds.UpperLeftCorner(), null, Color.White, Angle, Vector2.Zero, 1.0f, SpriteEffects.None, 0);
                return;
            }

            // draw the kart
            base.Draw(spriteBatch, millisElapsed);
            // spriteBatch.Draw(Texture, Bounds.UpperLeftCorner(), null, Color.White, Angle, Vector2.Zero, 1.0f, SpriteEffects.None, 0);

            // if the shields are active, draw them, too
            //if (shields_active)
            //{
                //spriteBatch.Draw(ShieldTexture, Position, null, Color.White, Angle, Bounds.Origin, 1.0f, SpriteEffects.None, 0);    <-- THIS WAS THE OLD ONE, ADJUST ACCORDINGLY
                //spriteBatch.Draw(Utilities.ShieldTexture, Bounds.UpperLeftCorner(), null, Color.White, Angle, Vector2.Zero, 1.0f, SpriteEffects.None, 0);
            //}

            //TAKE NOTE FOR FUTURE If the max health or energy of the kart is every changed due to an attachment the variables will need to be updated also
            float XEnergyDist = 45 * (Energy / maxEnergy);
            float XHealthDist = 45 * (Health / maxHealth);

            spriteBatch.Draw(Utilities.energyLineTexture, new Rectangle((int)Position.X - 8, (int)Position.Y - 10, (int)XEnergyDist, 5), Color.White);
            spriteBatch.Draw(Utilities.healthLineTexture, new Rectangle((int)Position.X - 8, (int)Position.Y - 18, (int)XHealthDist, 5), Color.White);

            if (NumCoins > 0)
            {
                spriteBatch.DrawString(Utilities.coinNumberFont, NumCoins.ToString(), new Vector2(Position.X + 14 - (Utilities.coinNumberFont.MeasureString(NumCoins.ToString()).X) / 2, Position.Y - (18 + Utilities.coinNumberFont.MeasureString("O").Y)), Color.Black);
            }


            // draw the weapons on the kart

            Vector2 forward_vector = new Vector2((float)Math.Cos(Angle - Math.PI / 2), (float)Math.Sin((double)Angle - Math.PI / 2));
            forward_vector.Normalize();

            Vector2 backward_vector = -forward_vector;

            Vector2 right_vector = new Vector2((float)Math.Cos((double)Angle), (float)Math.Sin((double)Angle));
            right_vector.Normalize();

            Vector2 left_vector = -right_vector;

            Vector2 front_center = Bounds.UpperLeftCorner() + right_vector * 14.0f;
            Vector2 left_center = Bounds.UpperLeftCorner() + backward_vector * 22.5f;
            Vector2 back_center = Bounds.UpperLeftCorner() + backward_vector * 45.0f + right_vector * 14.0f;
            Vector2 right_center = Bounds.UpperLeftCorner() + backward_vector * 22.5f + right_vector * 28.0f;

            Vector2 center_point = front_center + backward_vector * 22.5f;

            ProjectileSpawnPositions[(int)KartSides.FRONT] = front_center;
            ProjectileSpawnPositions[(int)KartSides.LEFT] = front_center + left_vector * 10.0f - forward_vector * 2.0f;
            ProjectileSpawnPositions[(int)KartSides.REAR] = back_center;
            ProjectileSpawnPositions[(int)KartSides.RIGHT] = front_center + right_vector * 10.0f - forward_vector * 2.0f;

            // front side
            //spriteBatch.Draw(WeaponTextures[(int)KartSides.FRONT], front_center - forward_vector * 2.0f, null, Color.White, Angle, new Vector2(4, 16), 1.0f, SpriteEffects.None, 0);

            // back side
            spriteBatch.Draw(WeaponTextures[(int)KartSides.REAR], back_center + forward_vector * 2.0f, null, Color.White, Angle + (float)Math.PI, new Vector2(4, 16), 1.0f, SpriteEffects.None, 0);

            // left side
            spriteBatch.Draw(WeaponTextures[(int)KartSides.LEFT], front_center + left_vector * 10.0f - forward_vector * 2.0f, null, Color.White, Angle, new Vector2(4, 16), 1.0f, SpriteEffects.None, 0);

            // right side
            spriteBatch.Draw(WeaponTextures[(int)KartSides.RIGHT], front_center + right_vector * 10.0f - forward_vector * 2.0f, null, Color.White, Angle, new Vector2(4, 16), 1.0f, SpriteEffects.None, 0);

            if (shields_active)
            {
                spriteBatch.Draw(Utilities.shieldAnimationTextures[shieldAnimationFrameNum], center_point, null, Color.White, Angle, new Vector2(28.0f,45.0f), 1.0f, SpriteEffects.None, 0);

                timePassedForShieldAnimation += millisElapsed;

                if (timePassedForShieldAnimation > 75)
                {
                    shieldAnimationFrameNum = (shieldAnimationFrameNum + 1) % 6;
                    timePassedForShieldAnimation = 0;
                }
                
            }

            // draw the laser sight
            //if (abilities[(int)KartSides.LEFT].GetType() == typeof(BurstLaser) && leftTrigger ||
            //     abilities[(int)KartSides.RIGHT].GetType() == typeof(BurstLaser) && rightTrigger)
            if ( drawLaserSight )
            {
                spriteBatch.Draw(Utilities.laserSightTexture, front_center, null, Color.White, Angle - (float)Math.PI/2.0f, new Vector2(0.0f,2.0f), 1.0f, SpriteEffects.None, 0);
            }

            // draw some exhaust behind the kart
            timePassedForDustTrail += millisElapsed;

            if (timePassedForDustTrail > 75)
            {
                timePassedForDustTrail = 0;

                if (Math.Abs(Velocity.X) > 0.1f || Math.Abs(Velocity.Y) > 0.1f)
                {
                    //Utilities.booms.Add(new Boom(Utilities.dustTrails, 350, back_center - new Vector2(4.0f, 4.0f), 8, 8, 4, 350 / 4));
                    Utilities.booms.Add(new Boom(Utilities.dustTrails, 350, Bounds.LowerLeftCorner() - new Vector2(4.0f, 4.0f), 8, 8, 4, 350 / 4));
                    Utilities.booms.Add(new Boom(Utilities.dustTrails, 350, Bounds.LowerRightCorner() - new Vector2(4.0f, 4.0f), 8, 8, 4, 350 / 4));
                }
            }
        }

        public void HandleInput(GamePadState gamePadState)
        {
            // this was the original scheme
            /*
            forward = gamePadState.IsButtonDown(Buttons.RightTrigger);
            backward = gamePadState.IsButtonDown(Buttons.LeftTrigger);
            if (gamePadState.ThumbSticks.Left.X != 0.0f || gamePadState.ThumbSticks.Left.Y != 0.0f)
              targetDirection = new Vector2(gamePadState.ThumbSticks.Left.X, -gamePadState.ThumbSticks.Left.Y);
            targetDirection.Normalize(); // we probably don't need to make the vector a unit vector
             */

            previous_y = y;
            previous_x = x;
            previous_a = a;
            previous_b = b;
            previous_leftTrigger = leftTrigger;
            previous_rightTrigger = rightTrigger;

            a = gamePadState.IsButtonDown(Buttons.A);
            b = gamePadState.IsButtonDown(Buttons.B);
            x = gamePadState.IsButtonDown(Buttons.X);
            y = gamePadState.IsButtonDown(Buttons.Y);
            rightTrigger = gamePadState.IsButtonDown(Buttons.RightTrigger);
            leftTrigger = gamePadState.IsButtonDown(Buttons.LeftTrigger);

            if (gamePadState.ThumbSticks.Left.X != 0.0f || gamePadState.ThumbSticks.Left.Y != 0.0f)
            {
                targetDirection = new Vector2(gamePadState.ThumbSticks.Left.X, -gamePadState.ThumbSticks.Left.Y);
                stickPressed = true;
            }
            else
                stickPressed = false;
            if (b || x || y) // let's you rotate without moving
                stickPressed = false;
            // targetDirection.Normalize(); // we probably don't need to make the vector a unit vector
        }

        // attaches an ability to a specified side of the kart
        public void AttachAbility(Ability ability, KartSides side)
        {
            int index = (int)side;

            ability.SideAttached = side;
            abilities[index] = ability;

            // check for health pack or energy pack
            if (side == KartSides.REAR && ability.GetType() == typeof(HealthPack))
            {
                maxHealth *= 1.5f;
                Health = maxHealth;
            }
            else if (side == KartSides.REAR && ability.GetType() == typeof(EnergyPack))
            {
                maxEnergy *= 1.5f;
                Energy = maxEnergy;
            }
        }

        // checks if the button on the controller has been pushed for a given side
        // this is for semi-auto weapons
        public bool ActivateSide(KartSides side, int millis_passed, int firing_period)
        {
            if (dead)
            {
                return false;
            }

            // check if we have enough energy to shoot
            if (abilities[(int)side].EnergyPenalty > Energy)
            {
                return false;
            }

            bool canFire = false;

            timePassedForShooting[(int)side] += millis_passed;

            if (timePassedForShooting[(int)side] >= firing_period)
            {
                canFire = true;
            }

            // REMOVE?
            /*
              if (side == KartSides.FRONT)
              {
                  if (canFire && y && !previous_y)
                  {
                      timePassedForShooting[(int)side] = 0;
                      return true;
                  }
              }
            */
            if (side == KartSides.LEFT)
            {
                if (canFire && leftTrigger && !previous_leftTrigger)
                {
                    timePassedForShooting[(int)side] = 0;
                    return true;
                }
            }
            else if (side == KartSides.REAR)
            {
                if (canFire && a && !previous_a)
                {
                    timePassedForShooting[(int)side] = 0;
                    return true;
                }
            }
            else if (side == KartSides.RIGHT)
            {
                if (canFire && rightTrigger && !previous_rightTrigger)
                {
                    timePassedForShooting[(int)side] = 0;
                    return true;
                }
            }

            return false;

        }

        // will fire projectiles while a button on the controller is held down, not just pushed
        public bool ActivateFullAutoSide(KartSides side, int millis_passed, int firing_period)
        {
            if (dead)
            {
                return false;
            }

            // check if we have enough energy to shoot
            if (abilities[(int)side].EnergyPenalty > Energy)
            {
                return false;
            }

            bool canFire = false;

            timePassedForShooting[(int)side] += millis_passed;

            if (timePassedForShooting[(int)side] >= firing_period)
            {
                canFire = true;
            }

            // REMOVE THIS ONE?
            /*if (side == KartSides.FRONT)
            {
                if (canFire && y)
                {
                    timePassedForShooting[(int)side] = 0;
                    return true;
                }
            }*/
            if (side == KartSides.LEFT)
            {
                if (canFire && leftTrigger)
                {
                    timePassedForShooting[(int)side] = 0;
                    return true;
                }
            }
            else if (side == KartSides.REAR)
            {
                if (canFire && a)
                {
                    timePassedForShooting[(int)side] = 0;
                    return true;
                }
            }
            else if (side == KartSides.RIGHT)
            {
                if (canFire && rightTrigger)
                {
                    timePassedForShooting[(int)side] = 0;
                    return true;
                }
            }

            return false;
        }

        private void HitByProjectile(Projectile p)
        {
            hitByProjectile = true;
            if (p.GetType() == typeof(Rocket))
            {
                // play the explosion sound
                Utilities.RocketExplosion.Play();

                // bump the kart
                Velocity += 50.0f * p.Velocity;
            }
            else if (p.GetType() == typeof(Bullet))
            {
                // bump the kart
                Velocity += 3.0f * p.Velocity;
            }
            else if (p.GetType() == typeof(ShotgunPellet))
            {
                // bump the kart
                Velocity += 10.0f * p.Velocity;
            }
            else if (p.GetType() == typeof(Mine))
            {
                // play the explosion sound
                Utilities.MineHitSound.Play();

                // bump the kart -- push it backwards
                Velocity *= -2.0f;

                // decrement the number of mines in play for the color of this mine
                if (p.Team == Color.Blue)
                {
                    Utilities.blueMinesInPlay--;
                }
                else if (p.Team == Color.Orange)
                {
                    Utilities.orangeMinesInPlay--;
                }
                else if (p.Team == Color.Green)
                {
                    Utilities.greenMinesInPlay--;
                }
                else if (p.Team == Color.Turquoise)
                {
                    Utilities.turqoiseMinesInPlay--;
                }
            }
            else if (p.GetType() == typeof(Shockwave))
            {
                // bump the kart
                Velocity += 50.0f * p.Velocity;
            }
            else if (p.GetType() == typeof(Plasma))
            {
                // bump the kart
                Velocity += 5.0f * p.Velocity;

                // reduce the kart's energy
                Energy -= 20.0f;
                if (Energy < 0.0f)
                    Energy = 0.0f;
            }
            else if (p.GetType() == typeof(StunBullet))
            {
                // bump the kart
                Velocity += 5.0f * p.Velocity;
            }
            else if (p.GetType() == typeof(OilSlick))
            {
                // decrement the number of oil slicks in play
                if (p.Team == Color.Blue)
                {
                    Utilities.blueOilSlicksInPlay--;
                }
                else if (p.Team == Color.Orange)
                {
                    Utilities.orangeOilSlicksInPlay--;
                }
                else if (p.Team == Color.Green)
                {
                    Utilities.greenOilSlicksInPlay--;
                }
                else if (p.Team == Color.Turquoise)
                {
                    Utilities.turqoiseOilSlicksInPlay--;
                }
            }
            else if (p.GetType() == typeof(LaserBeam))
            {
                // send the kart's coins flying
                Velocity += 100.0f * p.Velocity;
            }

            TakeDamage(p.Damage, p.Source);
        }

        private void TakeDamage(float damage, Kart source)
        {
            damage *= Utilities.damageHandicaps[source.ID];
            if (shields_active)
            {
                Health -= (0.5f * damage);

                // play the 'projectile being absorbed into the shield' sound
                Utilities.ShieldAbsorbSound.Play();
            }
            else
                Health -= damage;

            if (Health < 0.0f)
            {
                firstSpawn = false;
                Health = 0.0f;
                source.kills[ID]++;
                deaths[source.ID]++;
                dead = true;
                ScatterCoins(); // must be done before we change the velocity
                Velocity = Utilities.getBase(Team).Bounds.Center - Bounds.Origin - Position;
                Velocity /= 3000.0f;
                timePassedSinceCollision = 0;
                
            }
        }

        private void ScatterCoins()
        {
            NumCoins = (int)((double)NumCoins * 1.5);
            NumCoins += 1;
            int blueCoins = 0;
            if (NumCoins > 200)
                NumCoins = 200;
            while (NumCoins > 25)
            {
                blueCoins += 1;
                NumCoins -= 5;
            }

            double coinAngle = 0.0f;
            double circulatingAngle = (2 * Math.PI) / NumCoins;
            for (int i = 0; i < NumCoins; i++)
            {
                Vector2 v = new Vector2((float)Math.Sin(coinAngle), -(float)Math.Cos(coinAngle));
                v *= 15;
                v += Velocity;
                v /= 2f;
                Utilities.AddNewCoins(new Coin(Position, false, v.X, v.Y));
                coinAngle += circulatingAngle;
            }
            coinAngle = 0.2f;
            circulatingAngle = (2 * Math.PI) / blueCoins;
            for (int i = 0; i < blueCoins; i++)
            {
                Vector2 v = new Vector2((float)Math.Sin(coinAngle), -(float)Math.Cos(coinAngle));
                v *= 15;
                v += Velocity;
                v /= 2f;
                Coin c = new Coin(Position, false, v.X, v.Y);
                c.worth = 5;
                Utilities.AddNewCoins(c);
                coinAngle += circulatingAngle;
            }
            NumCoins = 0;
        }

    }
}
