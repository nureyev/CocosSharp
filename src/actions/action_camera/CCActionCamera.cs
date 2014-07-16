namespace CocosSharp
{
    public class CCActionCamera : CCActionInterval
    {

        #region Constructors

        protected CCActionCamera (float duration) : base (duration)
        {
        }

        #endregion Constructors

        /// <summary>
        /// Start the Camera operation on the given target.
        /// </summary>
        /// <param name="target"></param>
        protected internal override CCActionState StartAction (CCNode target)
        {
            return new CCActionCameraState (this, target);

        }

        public override CCFiniteTimeAction Reverse ()
        {
            return new CCReverseTime (this);
        }
    }

    public class CCActionCameraState : CCActionIntervalState
    {
        protected CCPoint3 CameraCenter;
        protected CCPoint3 CameraTarget;
        protected CCPoint3 CameraUpDirection;

        public CCActionCameraState (CCActionCamera action, CCNode target)
            : base (action, target)
        {       
            CCCamera camera = target.Camera;

            CameraCenter = camera.CenterInWorldspace;
            CameraTarget = camera.TargetInWorldspace;
            CameraUpDirection = camera.UpDirection;
        }

    }
}