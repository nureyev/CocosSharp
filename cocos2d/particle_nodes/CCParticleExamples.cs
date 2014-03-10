
using System.IO;
using Microsoft.Xna.Framework.Graphics;

namespace CocosSharp
{

    internal static class CCParticleExample
    {
        private static byte[] _firePngData =
            {
                0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A, 0x00, 0x00, 0x00, 0x0D, 0x49, 0x48, 0x44, 0x52,
                0x00, 0x00, 0x00, 0x20, 0x00, 0x00, 0x00, 0x20, 0x08, 0x06, 0x00, 0x00, 0x00, 0x73, 0x7A, 0x7A,
                0xF4, 0x00, 0x00, 0x00, 0x04, 0x67, 0x41, 0x4D, 0x41, 0x00, 0x00, 0xAF, 0xC8, 0x37, 0x05, 0x8A,
                0xE9, 0x00, 0x00, 0x00, 0x19, 0x74, 0x45, 0x58, 0x74, 0x53, 0x6F, 0x66, 0x74, 0x77, 0x61, 0x72,
                0x65, 0x00, 0x41, 0x64, 0x6F, 0x62, 0x65, 0x20, 0x49, 0x6D, 0x61, 0x67, 0x65, 0x52, 0x65, 0x61,
                0x64, 0x79, 0x71, 0xC9, 0x65, 0x3C, 0x00, 0x00, 0x02, 0x64, 0x49, 0x44, 0x41, 0x54, 0x78, 0xDA,
                0xC4, 0x97, 0x89, 0x6E, 0xEB, 0x20, 0x10, 0x45, 0xBD, 0xE1, 0x2D, 0x4B, 0xFF, 0xFF, 0x37, 0x5F,
                0x5F, 0x0C, 0xD8, 0xC4, 0xAE, 0x2D, 0xDD, 0xA9, 0x6E, 0xA7, 0x38, 0xC1, 0x91, 0xAA, 0x44, 0xBA,
                0xCA, 0x06, 0xCC, 0x99, 0x85, 0x01, 0xE7, 0xCB, 0xB2, 0x64, 0xEF, 0x7C, 0x55, 0x2F, 0xCC, 0x69,
                0x56, 0x15, 0xAB, 0x72, 0x68, 0x81, 0xE6, 0x55, 0xFE, 0xE8, 0x62, 0x79, 0x62, 0x04, 0x36, 0xA3,
                0x06, 0xC0, 0x9B, 0xCA, 0x08, 0xC0, 0x7D, 0x55, 0x80, 0xA6, 0x54, 0x98, 0x67, 0x11, 0xA8, 0xA1,
                0x86, 0x3E, 0x0B, 0x44, 0x41, 0x00, 0x33, 0x19, 0x1F, 0x21, 0x43, 0x9F, 0x5F, 0x02, 0x68, 0x49,
                0x1D, 0x20, 0x1A, 0x82, 0x28, 0x09, 0xE0, 0x4E, 0xC6, 0x3D, 0x64, 0x57, 0x39, 0x80, 0xBA, 0xA3,
                0x00, 0x1D, 0xD4, 0x93, 0x3A, 0xC0, 0x34, 0x0F, 0x00, 0x3C, 0x8C, 0x59, 0x4A, 0x99, 0x44, 0xCA,
                0xA6, 0x02, 0x88, 0xC7, 0xA7, 0x55, 0x67, 0xE8, 0x44, 0x10, 0x12, 0x05, 0x0D, 0x30, 0x92, 0xE7,
                0x52, 0x33, 0x32, 0x26, 0xC3, 0x38, 0xF7, 0x0C, 0xA0, 0x06, 0x40, 0x0F, 0xC3, 0xD7, 0x55, 0x17,
                0x05, 0xD1, 0x92, 0x77, 0x02, 0x20, 0x85, 0xB7, 0x19, 0x18, 0x28, 0x4D, 0x05, 0x19, 0x9F, 0xA1,
                0xF1, 0x08, 0xC0, 0x05, 0x10, 0x57, 0x7C, 0x4F, 0x01, 0x10, 0xEF, 0xC5, 0xF8, 0xAC, 0x76, 0xC8,
                0x2E, 0x80, 0x14, 0x99, 0xE4, 0xFE, 0x44, 0x51, 0xB8, 0x52, 0x14, 0x3A, 0x32, 0x22, 0x00, 0x13,
                0x85, 0xBF, 0x52, 0xC6, 0x05, 0x8E, 0xE5, 0x63, 0x00, 0x86, 0xB6, 0x9C, 0x86, 0x38, 0xAB, 0x54,
                0x74, 0x18, 0x5B, 0x50, 0x58, 0x6D, 0xC4, 0xF3, 0x89, 0x6A, 0xC3, 0x61, 0x8E, 0xD9, 0x03, 0xA8,
                0x08, 0xA0, 0x55, 0xBB, 0x40, 0x40, 0x3E, 0x00, 0xD2, 0x53, 0x47, 0x94, 0x0E, 0x38, 0xD0, 0x7A,
                0x73, 0x64, 0x57, 0xF0, 0x16, 0xFE, 0x95, 0x82, 0x86, 0x1A, 0x4C, 0x4D, 0xE9, 0x68, 0xD5, 0xAE,
                0xB8, 0x00, 0xE2, 0x8C, 0xDF, 0x4B, 0xE4, 0xD7, 0xC1, 0xB3, 0x4C, 0x75, 0xC2, 0x36, 0xD2, 0x3F,
                0x2A, 0x7C, 0xF7, 0x0C, 0x50, 0x60, 0xB1, 0x4A, 0x81, 0x18, 0x88, 0xD3, 0x22, 0x75, 0xD1, 0x63,
                0x5C, 0x80, 0xF7, 0x19, 0x15, 0xA2, 0xA5, 0xB9, 0xB5, 0x5A, 0xB7, 0xA4, 0x34, 0x7D, 0x03, 0x48,
                0x5F, 0x17, 0x90, 0x52, 0x01, 0x19, 0x95, 0x9E, 0x1E, 0xD1, 0x30, 0x30, 0x9A, 0x21, 0xD7, 0x0D,
                0x81, 0xB3, 0xC1, 0x92, 0x0C, 0xE7, 0xD4, 0x1B, 0xBE, 0x49, 0xF2, 0x04, 0x15, 0x2A, 0x52, 0x06,
                0x69, 0x31, 0xCA, 0xB3, 0x22, 0x71, 0xBD, 0x1F, 0x00, 0x4B, 0x82, 0x66, 0xB5, 0xA7, 0x37, 0xCF,
                0x6F, 0x78, 0x0F, 0xF8, 0x5D, 0xC6, 0xA4, 0xAC, 0xF7, 0x23, 0x05, 0x6C, 0xE4, 0x4E, 0xE2, 0xE3,
                0x95, 0xB7, 0xD3, 0x40, 0xF3, 0xA5, 0x06, 0x1C, 0xFE, 0x1F, 0x09, 0x2A, 0xA8, 0xF5, 0xE6, 0x3D,
                0x00, 0xDD, 0xAD, 0x02, 0x2D, 0xC4, 0x4D, 0x66, 0xA0, 0x6A, 0x1F, 0xD5, 0x2E, 0xF8, 0x8F, 0xFF,
                0x2D, 0xC6, 0x4F, 0x04, 0x1E, 0x14, 0xD0, 0xAC, 0x01, 0x3C, 0xAA, 0x5C, 0x1F, 0xA9, 0x2E, 0x72,
                0xBA, 0x49, 0xB5, 0xC7, 0xFA, 0xC0, 0x27, 0xD2, 0x62, 0x69, 0xAE, 0xA7, 0xC8, 0x04, 0xEA, 0x0F,
                0xBF, 0x1A, 0x51, 0x50, 0x61, 0x16, 0x8F, 0x1B, 0xD5, 0x5E, 0x03, 0x75, 0x35, 0xDD, 0x09, 0x6F,
                0x88, 0xC4, 0x0D, 0x73, 0x07, 0x82, 0x61, 0x88, 0xE8, 0x59, 0x30, 0x45, 0x8E, 0xD4, 0x7A, 0xA7,
                0xBD, 0xDA, 0x07, 0x67, 0x81, 0x40, 0x30, 0x88, 0x55, 0xF5, 0x11, 0x05, 0xF0, 0x58, 0x94, 0x9B,
                0x48, 0xEC, 0x60, 0xF1, 0x09, 0xC7, 0xF1, 0x66, 0xFC, 0xDF, 0x0E, 0x84, 0x7F, 0x74, 0x1C, 0x8F,
                0x58, 0x44, 0x77, 0xAC, 0x59, 0xB5, 0xD7, 0x67, 0x00, 0x12, 0x85, 0x4F, 0x2A, 0x4E, 0x17, 0xBB,
                0x1F, 0xC6, 0x00, 0xB8, 0x99, 0xB0, 0xE7, 0x23, 0x9D, 0xF7, 0xCF, 0x6E, 0x44, 0x83, 0x4A, 0x45,
                0x32, 0x40, 0x86, 0x81, 0x7C, 0x8D, 0xBA, 0xAB, 0x1C, 0xA7, 0xDE, 0x09, 0x87, 0x48, 0x21, 0x26,
                0x5F, 0x4A, 0xAD, 0xBA, 0x6E, 0x4F, 0xCA, 0xFB, 0x23, 0xB7, 0x62, 0xF7, 0xCA, 0xAD, 0x58, 0x22,
                0xC1, 0x00, 0x47, 0x9F, 0x0B, 0x7C, 0xCA, 0x73, 0xC1, 0xDB, 0x9F, 0x8C, 0xF2, 0x17, 0x1E, 0x4E,
                0xDF, 0xF2, 0x6C, 0xF8, 0x67, 0xAF, 0x22, 0x7B, 0xF3, 0xEB, 0x4B, 0x80, 0x01, 0x00, 0xB8, 0x21,
                0x72, 0x89, 0x08, 0x10, 0x07, 0x7D, 0x00, 0x00, 0x00, 0x00, 0x49, 0x45, 0x4E, 0x44, 0xAE, 0x42,
                0x60, 0x82
            };

        private static CCTexture2D _defaultTexture;

        public static CCTexture2D DefaultTexture
        {
            get
            {
                if (_defaultTexture == null)
                {
                    _defaultTexture = CCTextureCache.SharedTextureCache.AddImage(_firePngData, "__firePngData", SurfaceFormat.Color);
                }

                return _defaultTexture;
            }
        }
    }

    //
    // ParticleFire
    //
    public class CCParticleFire : CCParticleSystemQuad
    {
        public CCParticleFire() : base(250)
        {
            InitCCParticleFire();
        }

        private void InitCCParticleFire()
        {
            // duration
            Duration = ParticleDurationInfinity;

            // Gravity Mode
            EmitterMode = CCEmitterMode.Gravity;

            // Gravity Mode: gravity
            GravityMoveMode gravityMode = new GravityMoveMode();
            gravityMode.Gravity = new CCPoint(0, 0);
            gravityMode.RadialAccel = 0;
            gravityMode.RadialAccelVar = 0;
            gravityMode.Speed = 60;
            gravityMode.SpeedVar = 20;
            GravityMode = gravityMode;

            // starting angle
            Angle = 90;
            AngleVar = 10;

            // emitter position
            CCSize winSize = CCDirector.SharedDirector.WinSize;
            SetPosition(winSize.Width / 2, 60);
            PositionVar = new CCPoint(40, 20);

            // life of particles
            Life = 3;
            LifeVar = 0.25f;


            // size, in pixels
            StartSize = 54.0f;
            StartSizeVar = 10.0f;
            EndSize = ParticleStartSizeEqualToEndSize;

            // emits per frame
            EmissionRate = TotalParticles / Life;

            // color of particles
            CCColor4F startColor;
            startColor.R = 0.76f;
            startColor.G = 0.25f;
            startColor.B = 0.12f;
            startColor.A = 1.0f;
            StartColor = startColor;

            CCColor4F startColorVar;
            startColorVar.R = 0.0f;
            startColorVar.G = 0.0f;
            startColorVar.B = 0.0f;
            startColorVar.A = 0.0f;
            StartColorVar = startColorVar;

            CCColor4F endColor;
            endColor.R = 0.0f;
            endColor.G = 0.0f;
            endColor.B = 0.0f;
            endColor.A = 1.0f;
            EndColor = endColor;

            CCColor4F endColorVar;
            endColorVar.R = 0.0f;
            endColorVar.G = 0.0f;
            endColorVar.B = 0.0f;
            endColorVar.A = 0.0f;
            EndColorVar = endColorVar;

            // additive
            BlendAdditive = true;

            Texture = CCParticleExample.DefaultTexture;
        }
    }

    //
    // ParticleFireworks
    //
    public class CCParticleFireworks : CCParticleSystemQuad
    {
        public CCParticleFireworks() : base(1500)
        {
            InitCCParticleFireworks();
        }

        private void InitCCParticleFireworks()
        {
            // duration
            Duration = ParticleDurationInfinity;

            // Gravity Mode
            EmitterMode = CCEmitterMode.Gravity;

            // Gravity Mode: gravity
            GravityMoveMode gravityMode = new GravityMoveMode();
            gravityMode.Gravity = new CCPoint(0, -90);
            gravityMode.RadialAccel = 0;
            gravityMode.RadialAccelVar = 0;
            gravityMode.Speed = 180;
            gravityMode.SpeedVar = 50;
            GravityMode = gravityMode;

            // emitter position
            CCSize winSize = CCDirector.SharedDirector.WinSize;
            Position = new CCPoint(winSize.Width / 2, winSize.Height / 2);

            // angle
            Angle = 90;
            AngleVar = 20;

            // life of particles
            Life = 3.5f;
            LifeVar = 1;

            // emits per frame
            EmissionRate = TotalParticles / Life;

            // color of particles
            StartColor = new CCColor4F(0.5f, 0.5f, 0.5f, 1.0f);

            CCColor4F startColorVar;
            startColorVar.R = 0.5f;
            startColorVar.G = 0.5f;
            startColorVar.B = 0.5f;
            startColorVar.A = 0.1f;
            StartColorVar = startColorVar;

            CCColor4F endColor;
            endColor.R = 0.1f;
            endColor.G = 0.1f;
            endColor.B = 0.1f;
            endColor.A = 0.2f;
            EndColor = endColor;

            CCColor4F endColorVar;
            endColorVar.R = 0.1f;
            endColorVar.G = 0.1f;
            endColorVar.B = 0.1f;
            endColorVar.A = 0.2f;
            EndColorVar = endColorVar;

            // size, in pixels
            StartSize = 8.0f;
            StartSizeVar = 2.0f;
            EndSize = ParticleStartSizeEqualToEndSize;

            // additive
            BlendAdditive = false;

            Texture = CCParticleExample.DefaultTexture;
        }
    }

    //
    // ParticleSun
    //
    public class CCParticleSun : CCParticleSystemQuad
    {
        public CCParticleSun (int num) : base(num)
        {
            InitCCParticleSun();
        }

        public CCParticleSun() : this(350)
        { 
        }

        private void InitCCParticleSun()
        {
            // additive
            BlendAdditive = true;

            // duration
            Duration = ParticleDurationInfinity;

            // Gravity Mode
            EmitterMode = CCEmitterMode.Gravity;


            // Gravity Mode: gravity
            GravityMoveMode gravityMode = new GravityMoveMode();
            gravityMode.Gravity = new CCPoint(0, 0);
            gravityMode.RadialAccel = 0;
            gravityMode.RadialAccelVar = 0;
            gravityMode.Speed = 20;
            gravityMode.SpeedVar = 5;
            GravityMode = gravityMode;

            // angle
            Angle = 90;
            AngleVar = 360;

            // emitter position
            CCSize winSize = CCDirector.SharedDirector.WinSize;
            Position = new CCPoint(winSize.Width / 2, winSize.Height / 2);
            PositionVar = CCPoint.Zero;

            // life of particles
            Life = 1;
            LifeVar = 0.5f;

            // size, in pixels
            StartSize = 30.0f;
            StartSizeVar = 10.0f;
            EndSize = ParticleStartSizeEqualToEndSize;

            // emits per seconds
            EmissionRate = TotalParticles / Life;

            // color of particles
            StartColor = new CCColor4F(0.76f, 0.25f, 0.12f, 1.0f);
            StartColorVar = new CCColor4F();
            EndColor = new CCColor4F(0.0f, 0.0f, 0.0f, 1.0f);
            EndColorVar = new CCColor4F();

            Texture = CCParticleExample.DefaultTexture;
        }
    }

    //
    // ParticleGalaxy
    //
    public class CCParticleGalaxy : CCParticleSystemQuad
    {
        public CCParticleGalaxy() : base(200)
        {
            InitCCParticleGalaxy();
        }

        private void InitCCParticleGalaxy()
        {
            // duration
            Duration = ParticleDurationInfinity;

            // Gravity Mode
            EmitterMode = CCEmitterMode.Gravity;

            GravityMoveMode gravityMode = new GravityMoveMode();
            gravityMode.Gravity = new CCPoint(0, 0);
            gravityMode.RadialAccel = -80;
            gravityMode.RadialAccelVar = 0;
            gravityMode.Speed = 60;
            gravityMode.SpeedVar = 10;
            gravityMode.TangentialAccel = 80;
            gravityMode.TangentialAccelVar = 0;
            GravityMode = gravityMode;

            // angle
            Angle = 90;
            AngleVar = 360;

            // emitter position
            CCSize winSize = CCDirector.SharedDirector.WinSize;
            Position = new CCPoint(winSize.Width / 2, winSize.Height / 2);
            PositionVar = CCPoint.Zero;

            // life of particles
            Life = 4;
            LifeVar = 1;

            // size, in pixels
            StartSize = 37.0f;
            StartSizeVar = 10.0f;
            EndSize = ParticleStartSizeEqualToEndSize;

            // emits per second
            EmissionRate = TotalParticles / Life;

            // color of particles
            StartColor = new CCColor4F(0.12f, 0.25f, 0.76f, 1.0f);
            StartColorVar = new CCColor4F();
            EndColor = new CCColor4F(0.0f, 0.0f, 0.0f, 1.0f);
            EndColorVar = new CCColor4F();

            // additive
            BlendAdditive = true;

            Texture = CCParticleExample.DefaultTexture;
        }
    }

    public class CCParticleFlower : CCParticleSystemQuad
    {
        public CCParticleFlower() : base(250)
        {
            InitCCParticleFlower();
        }

        private void InitCCParticleFlower()
        {
            // duration
            Duration = ParticleDurationInfinity;

            // Gravity Mode
            EmitterMode = CCEmitterMode.Gravity;

            GravityMoveMode gravityMode = new GravityMoveMode();
            gravityMode.Gravity = new CCPoint(0, 0);
            gravityMode.RadialAccel = -60;
            gravityMode.RadialAccelVar = 0;
            gravityMode.Speed = 80;
            gravityMode.SpeedVar = 10;
            gravityMode.TangentialAccel = 15;
            gravityMode.TangentialAccelVar = 0;
            GravityMode = gravityMode;

            // angle
            Angle = 90;
            AngleVar = 360;

            // emitter position
            CCSize winSize = CCDirector.SharedDirector.WinSize;
            Position = new CCPoint(winSize.Width / 2, winSize.Height / 2);
            PositionVar = CCPoint.Zero;

            // life of particles
            Life = 4;
            LifeVar = 1;

            // size, in pixels
            StartSize = 30.0f;
            StartSizeVar = 10.0f;
            EndSize = ParticleStartSizeEqualToEndSize;

            // emits per second
            EmissionRate = TotalParticles / Life;

            // color of particles
            StartColor = new CCColor4F(0.5f, 0.5f, 0.5f, 1.0f);
            StartColorVar = new CCColor4F(0.5f, 0.5f, 0.5f, 0.5f);
            EndColor = new CCColor4F(0.0f, 0.0f, 0.0f, 1.0f);
            EndColorVar = new CCColor4F();

            // additive
            BlendAdditive = true;

            Texture = CCParticleExample.DefaultTexture;
        }
    }

    public class CCParticleMeteor : CCParticleSystemQuad
    {
        public CCParticleMeteor() : base(150)
        {
            InitCCParticleMeteor();
        }

        private void InitCCParticleMeteor()
        {
            // duration
            Duration = ParticleDurationInfinity;

            // Gravity Mode
            EmitterMode = CCEmitterMode.Gravity;

            GravityMoveMode gravityMode = new GravityMoveMode();
            gravityMode.Gravity = new CCPoint(-200, 200);
            gravityMode.RadialAccel = 0;
            gravityMode.RadialAccelVar = 0;
            gravityMode.Speed = 15;
            gravityMode.SpeedVar = 5;
            gravityMode.TangentialAccel = 0;
            gravityMode.TangentialAccelVar = 0;
            GravityMode = gravityMode;

            // angle
            Angle = 90;
            AngleVar = 360;

            // emitter position
            CCSize winSize = CCDirector.SharedDirector.WinSize;
            Position = new CCPoint(winSize.Width / 2, winSize.Height / 2);
            PositionVar = CCPoint.Zero;

            // life of particles
            Life = 2;
            LifeVar = 1;

            // size, in pixels
            StartSize = 60.0f;
            StartSizeVar = 10.0f;
            EndSize = ParticleStartSizeEqualToEndSize;

            // emits per second
            EmissionRate = TotalParticles / Life;

            // color of particles
            StartColor = new CCColor4F(0.2f, 0.4f, 0.7f, 1.0f);
            StartColorVar = new CCColor4F(0.0f, 0.0f, 0.2f, 0.1f);
            EndColor = new CCColor4F(0.0f, 0.0f, 0.0f, 1.0f);
            EndColorVar = new CCColor4F();

            // additive
            BlendAdditive = true;

            Texture = CCParticleExample.DefaultTexture;
        }
    }

    public class CCParticleSpiral : CCParticleSystemQuad
    {
        public CCParticleSpiral() : base(500)
        {
            InitCCParticleSpiral();
        }

        private void InitCCParticleSpiral()
        {
            // duration
            Duration = ParticleDurationInfinity;

            // Gravity Mode
            EmitterMode = CCEmitterMode.Gravity;

            GravityMoveMode gravityMode = new GravityMoveMode();
            gravityMode.Gravity = new CCPoint(0, 0);
            gravityMode.RadialAccel = -380;
            gravityMode.RadialAccelVar = 0;
            gravityMode.Speed = 150;
            gravityMode.SpeedVar = 0;
            gravityMode.TangentialAccel = 45;
            gravityMode.TangentialAccelVar = 0;
            GravityMode = gravityMode;

            // angle
            Angle = 90;
            AngleVar = 0;

            // emitter position
            CCSize winSize = CCDirector.SharedDirector.WinSize;
            Position = new CCPoint(winSize.Width / 2, winSize.Height / 2);
            PositionVar = CCPoint.Zero;

            // life of particles
            Life = 12;
            LifeVar = 0;

            // size, in pixels
            StartSize = 20.0f;
            StartSizeVar = 0.0f;
            EndSize = ParticleStartSizeEqualToEndSize;

            // emits per second
            EmissionRate = TotalParticles / Life;

            // color of particles
            StartColor = new CCColor4F(0.5f, 0.5f, 0.5f, 1.0f);
            StartColorVar = new CCColor4F(0.5f, 0.5f, 0.5f, 0.0f);
            EndColor = new CCColor4F(0.5f, 0.5f, 0.5f, 1.0f);
            EndColorVar = new CCColor4F(0.5f, 0.5f, 0.5f, 0.0f);

            // additive
            BlendAdditive = false;

            Texture = CCParticleExample.DefaultTexture;
        }
    }

    public class CCParticleExplosion : CCParticleSystemQuad
    {
        public CCParticleExplosion() : base(700)
        {
            InitCCParticleExplosion();
        }

        private void InitCCParticleExplosion()
        {
            // duration
            Duration = 0.1f;

            EmitterMode = CCEmitterMode.Gravity;

            GravityMoveMode gravityMode = new GravityMoveMode();
            gravityMode.Gravity = new CCPoint(0, 0);
            gravityMode.RadialAccel = 0;
            gravityMode.RadialAccelVar = 0;
            gravityMode.Speed = 70;
            gravityMode.SpeedVar = 40;
            gravityMode.TangentialAccel = 0;
            gravityMode.TangentialAccelVar = 0;
            GravityMode = gravityMode;

            // angle
            Angle = 90;
            AngleVar = 360;

            // emitter position
            CCSize winSize = CCDirector.SharedDirector.WinSize;
            Position = new CCPoint(winSize.Width / 2, winSize.Height / 2);
            PositionVar = CCPoint.Zero;

            // life of particles
            Life = 5.0f;
            LifeVar = 2;

            // size, in pixels
            StartSize = 15.0f;
            StartSizeVar = 10.0f;
            EndSize = ParticleStartSizeEqualToEndSize;

            // emits per second
            EmissionRate = TotalParticles / Duration;

            // color of particles
            StartColor = new CCColor4F(0.7f, 0.1f, 0.2f, 1.0f);
            StartColorVar = new CCColor4F(0.5f, 0.5f, 0.5f, 0.0f);
            EndColor = new CCColor4F(0.5f, 0.5f, 0.5f, 0.0f);
            EndColorVar = new CCColor4F(0.5f, 0.5f, 0.5f, 0.0f);


            // additive
            BlendAdditive = false;

            Texture = CCParticleExample.DefaultTexture;
        }
    }

    public class CCParticleSmoke : CCParticleSystemQuad
    {
        public CCParticleSmoke() : base(200)
        {
            InitCCParticleSmoke(); 
        }

        private void InitCCParticleSmoke()
        {
            // duration
            Duration = ParticleDurationInfinity;

            // Emitter mode: Gravity Mode
            EmitterMode = CCEmitterMode.Gravity;

            GravityMoveMode gravityMode = new GravityMoveMode();
            gravityMode.Gravity = new CCPoint(0, 0);
            gravityMode.RadialAccel = 0;
            gravityMode.RadialAccelVar = 0;
            gravityMode.Speed = 25;
            gravityMode.SpeedVar = 10;
            GravityMode = gravityMode;

            // angle
            Angle = 90;
            AngleVar = 5;

            // emitter position
            CCSize winSize = CCDirector.SharedDirector.WinSize;
            Position = new CCPoint(winSize.Width / 2, 0);
            PositionVar = new CCPoint(20, 0);

            // life of particles
            Life = 4;
            LifeVar = 1;

            // size, in pixels
            StartSize = 60.0f;
            StartSizeVar = 10.0f;
            EndSize = ParticleStartSizeEqualToEndSize;

            // emits per frame
            EmissionRate = TotalParticles / Life;

            // color of particles
            StartColor = new CCColor4F(0.8f, 0.8f, 0.8f, 1.0f);
            StartColorVar = new CCColor4F(0.02f, 0.02f, 0.02f, 0.0f);
            EndColor = new CCColor4F(0.0f, 0.0f, 0.0f, 1.0f);
            EndColorVar = new CCColor4F();

            // additive
            BlendAdditive = false;

            Texture = CCParticleExample.DefaultTexture;
        }
    }

    public class CCParticleSnow : CCParticleSystemQuad
    {
        public CCParticleSnow() : base(700)
        {
            InitCCParticleSnow(); 
        }

        private void InitCCParticleSnow()
        {
            // duration
            Duration = ParticleDurationInfinity;

            // set gravity mode.
            EmitterMode = CCEmitterMode.Gravity;

            GravityMoveMode gravityMode = new GravityMoveMode();
            gravityMode.Gravity = new CCPoint(0, -1);
            gravityMode.RadialAccel = 0;
            gravityMode.RadialAccelVar = 1;
            gravityMode.TangentialAccel = 0;
            gravityMode.TangentialAccelVar = 1;
            gravityMode.Speed = 5;
            gravityMode.SpeedVar = 1;
            GravityMode = gravityMode;

            // emitter position
            CCSize winSize = CCDirector.SharedDirector.WinSize;
            Position = new CCPoint(winSize.Width / 2, winSize.Height + 10);
            PositionVar = new CCPoint(winSize.Width / 2, 0);

            // angle
            Angle = -90;
            AngleVar = 5;

            // life of particles
            Life = 45;
            LifeVar = 15;

            // size, in pixels
            StartSize = 10.0f;
            StartSizeVar = 5.0f;
            EndSize = ParticleStartSizeEqualToEndSize;

            // emits per second
            EmissionRate = 10;

            // color of particles
            StartColor = new CCColor4F(1.0f, 1.0f, 1.0f, 1.0f);
            StartColorVar = new CCColor4F();
            EndColor = new CCColor4F(1.0f, 1.0f, 1.0f, 0.0f);
            EndColorVar = new CCColor4F();

            // additive
            BlendAdditive = false;

            Texture = CCParticleExample.DefaultTexture;
        }
    }

    public class CCParticleRain : CCParticleSystemQuad
    {
        public CCParticleRain() : base(1000)
        {
            InitCCParticleRain();
        }

        private void InitCCParticleRain()
        {
            // duration
            Duration = ParticleDurationInfinity;

            EmitterMode = CCEmitterMode.Gravity;

            GravityMoveMode gravityMode = new GravityMoveMode();
            gravityMode.Gravity = new CCPoint(10, -10);
            gravityMode.RadialAccel = 0;
            gravityMode.RadialAccelVar = 1;
            gravityMode.TangentialAccel = 0;
            gravityMode.TangentialAccelVar = 1;
            gravityMode.Speed = 130;
            gravityMode.SpeedVar = 30;
            GravityMode = gravityMode;

            // angle
            Angle = -90;
            AngleVar = 5;


            // emitter position
            CCSize winSize = CCDirector.SharedDirector.WinSize;
            Position = new CCPoint(winSize.Width / 2, winSize.Height);
            PositionVar = new CCPoint(winSize.Width / 2, 0);

            // life of particles
            Life = 4.5f;
            LifeVar = 0;

            // size, in pixels
            StartSize = 4.0f;
            StartSizeVar = 2.0f;
            EndSize = ParticleStartSizeEqualToEndSize;

            // emits per second
            EmissionRate = 20;

            // color of particles
            StartColor = new CCColor4F(0.7f, 0.8f, 1.0f, 1.0f);
            StartColorVar = new CCColor4F();
            EndColor = new CCColor4F(0.7f, 0.8f, 1.0f, 0.5f);
            EndColorVar = new CCColor4F();

            // additive
            BlendAdditive = false;

            Texture = CCParticleExample.DefaultTexture;
        }
    }
}