using System;
using System.IO;

namespace CocosSharp
{
    /// <summary>
    /// brief CCScene is a subclass of CCNode that is used only as an abstract concept.
    /// CCScene and CCNode are almost identical with the difference that CCScene has it's
    /// anchor point (by default) at the center of the screen. Scenes have state management
    /// where they can serialize their state and have it reconstructed upon resurrection.
    ///  It is a good practice to use and CCScene as the parent of all your nodes.
    /// </summary>
    public class CCScene : CCNode
    {
        CCViewport viewport;
        CCWindow window;
        CCApplication application;

        internal event EventHandler SceneViewportChanged = delegate {};


        #region Properties

        public virtual bool IsTransition
        {
            get { return false; }
        }

        public CCRect VisibleBoundsScreenspace
        {
            get { return Viewport.ViewportInPixels; }
        }

        public override CCScene Scene
        {
            get { return this; }
        }

        public override CCWindow Window 
        { 
            get { return window; }
            set 
            {
                if(window != value) 
                {
                    window = value;
                    viewport.LandscapeScreenSizeInPixels = Window.LandscapeWindowSizeInPixels;
                }
            }
        }

        public override CCDirector Director { get; set; }

        public override CCCamera Camera
        {
            get { return null; }
        }

        public override CCViewport Viewport 
        {
            get { return viewport; }
            set 
            {
                if (viewport != value) 
                {
                    // Stop listening to previous viewport's event
                    if(viewport != null)
                        viewport.OnViewportChanged -= OnViewportChanged;

                    viewport = value;

                    viewport.OnViewportChanged += OnViewportChanged;

                    OnViewportChanged(this, null);
                }
            }
        }

        internal override CCEventDispatcher EventDispatcher 
        { 
            get { return Window != null ? Window.EventDispatcher : null; }
        }

        public override CCSize ContentSize
        {
            get { return CCSize.Zero; }
            set {}
        }

        #endregion Properties


        #region Constructors

        public CCScene(CCWindow window, CCViewport viewport, CCDirector director)
        {
            IgnoreAnchorPointForPosition = true;
            AnchorPoint = new CCPoint(0.5f, 0.5f);
            Viewport = viewport;
            Window = window;
            Director = director;
        }

        public CCScene(CCScene scene): this(scene.Window, scene.Viewport, scene.Director)
        {
        }

        #endregion Constructors


        #region Viewport handling

        void OnViewportChanged(object sender, EventArgs e)
        {
            CCViewport viewport = sender as CCViewport;

            if(viewport != null && viewport == Viewport) 
            {
                SceneViewportChanged(this, null);
            }
        }

        #endregion Viewport handling

        protected override void Draw()
        {
            CCDrawManager drawManager = Window.DrawManager;

            base.Draw();

            drawManager.Viewport = Viewport.XnaViewport; 
        }
    }
}