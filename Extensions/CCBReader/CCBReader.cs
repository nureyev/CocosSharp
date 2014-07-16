using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Microsoft.Xna.Framework;

namespace CocosSharp
{
    public enum CCBPropertyType
    {
        Position = 0,
        Size,
        Point,
        PointLock,
        ScaleLock,
        Degrees,
        Integer,
        Float,
        FloatVar,
        Check,
        SpriteFrame,
        Texture,
        Byte,
        Color3,
        Color4FVar,
        Flip,
        Blendmode,
        FntFile,
        Text,
        FontTTF,
        IntegerLabeled,
        Block,
        Animation,
        CCBFile,
        String,
        BlockCCControl,
        FloatScale,
        FloatXY
    }

    internal enum CCBFloatType
    {
        Float0 = 0,
        Float1,
        Minus1,
        Float05,
        Integer,
        Full
    }

    internal enum PlatformType
    {
        All = 0,
        IOS,
        Mac
    }

    public enum CCBTargetType
    {
        None = 0,
        DocumentRoot = 1,
        Owner = 2,
    }

    public enum CCBEasingType
    {
        Instant,

        Linear,

        CubicIn,
        CubicOut,
        CubicInOut,

        ElasticIn,
        ElasticOut,
        ElasticInOut,

        BounceIn,
        BounceOut,
        BounceInOut,

        BackIn,
        BackOut,
        BackInOut,
    }

    public enum CCBPositionType
    {
        RelativeBottomLeft,
        RelativeTopLeft,
        RelativeTopRight,
        RelativeBottomRight,
        Percent,
        MultiplyResolution,
    }

    internal enum SizeType
    {
        Absolute,
        Percent,
        RelativeContainer,
        HorizontalPercent,
        VerticalPercent,
        MultiplyResolution,
    }

    public enum CCBScaleType
    {
        Absolute,
        MultiplyResolution
    }

    /**
     * @addtogroup cocosbuilder
     * @{
     */

    public class CCBFile : CCNode
    {
        private CCNode _CCBFileNode;

        public CCNode FileNode
        {
            get { return _CCBFileNode; }
            set { _CCBFileNode = value; }
        }

        public CCBFile() { }
    }


    public interface ICCNodeLoaderListener
    {
        void OnNodeLoaded(CCNode node, CCNodeLoader nodeLoader);
    }

    public interface ICCBSelectorResolver
    {
		Action<object> OnResolveCCBCCMenuItemSelector(object target, string pSelectorName);
        Action<CCNode> OnResolveCCBCCCallFuncSelector(Object pTarget, string pSelectorName);
		Action<object, CCControlEvent> OnResolveCCBCCControlSelector(object target, string pSelectorName);
    }

    public interface ICCBScriptOwner
    {
        ICCBSelectorResolver CreateNew();
    }

    /**
     * @brief Parse CCBI file which is generated by CocosBuilder
     */

    public class CCBReader 
    {
        private static float __ccbResolutionScale = 1.0f;

        private const int CCBVersion = 5;

        private readonly List<string> _animatedProps = new List<string>();

        private readonly ICCBMemberVariableAssigner _CCBMemberVariableAssigner;
        private readonly ICCBSelectorResolver _CCBSelectorResolver;
        private readonly CCNodeLoaderLibrary _nodeLoaderLibrary;
        private readonly ICCNodeLoaderListener _nodeLoaderListener;
        private readonly List<string> _loadedSpriteSheets;
        private readonly List<string> _stringCache = new List<string>();

		internal bool HasScriptingOwner { get; set; }
        private CCBAnimationManager _actionManager;
        internal byte[] _bytes;
        internal int _currentBit;
        internal int _currentByte;
        internal object _owner;

        private Dictionary<CCNode, CCBAnimationManager> _actionManagers;
        private List<string> _ownerOutletNames;
        private List<CCNode> _ownerOutletNodes;
        private List<CCNode> _nodesWithAnimationManagers;
        private List<CCBAnimationManager> _animationManagersForNodes;

        private List<string> _ownerCallbackNames;
        private List<CCNode> _ownerCallbackNodes;
        private string _CCBRootPath;
        private bool _jsControlled;

        private static readonly UTF8Encoding utf8Encoder = new UTF8Encoding(false);

        public CCWindow Window { get; set; }
        public CCViewport Viewport { get; set; }
        public CCCamera Camera { get; set; }
        public CCLayer Layer { get; set; }
        public CCDirector Director { get; set; }

        public CCBReader(CCNodeLoaderLibrary nodeLoaderLibrary)
            : this(nodeLoaderLibrary, null, null, null)
        {
        }

        public CCBReader(CCNodeLoaderLibrary nodeLoaderLibrary, ICCBMemberVariableAssigner memberVariableAssigner)
            : this(nodeLoaderLibrary, memberVariableAssigner, null, null)
        {
        }

        public CCBReader(CCNodeLoaderLibrary nodeLoaderLibrary, ICCBMemberVariableAssigner memberVariableAssigner,
                         ICCBSelectorResolver selectorResolver)
            : this(nodeLoaderLibrary, memberVariableAssigner, selectorResolver, null)
        {
        }

        public CCBReader(CCNodeLoaderLibrary nodeLoaderLibrary, ICCBMemberVariableAssigner memberVariableAssigner,
                         ICCBSelectorResolver selectorResolver, ICCNodeLoaderListener nodeLoaderListener)
        {
            _currentByte = -1;
            _currentBit = -1;

            _loadedSpriteSheets = new List<string>();

            _nodeLoaderLibrary = nodeLoaderLibrary;
            _CCBMemberVariableAssigner = memberVariableAssigner;
            _CCBSelectorResolver = selectorResolver;
            _nodeLoaderListener = nodeLoaderListener;
            Init();
        }

        public CCBReader(CCBReader reader)
        {
            _currentByte = -1;
            _currentBit = -1;

            _loadedSpriteSheets = reader._loadedSpriteSheets;
            _nodeLoaderLibrary = reader._nodeLoaderLibrary;

            _CCBMemberVariableAssigner = reader._CCBMemberVariableAssigner;
            _CCBSelectorResolver = reader._CCBSelectorResolver;
            _nodeLoaderListener = reader._nodeLoaderListener;

            _ownerCallbackNames = reader._ownerCallbackNames;
            _ownerCallbackNodes = reader._ownerCallbackNodes;
            _ownerOutletNames = reader._ownerOutletNames;
            _ownerOutletNodes = reader._ownerOutletNodes;

            _CCBRootPath = reader.CCBRootPath;

            Init();
        }

        public CCBReader()
        {
            _currentByte = -1;
            _currentBit = -1;
            Init();
        }

        public ICCBMemberVariableAssigner MemberVariableAssigner
        {
            get { return _CCBMemberVariableAssigner; }
        }

        public ICCBSelectorResolver SelectorResolver
        {
            get { return _CCBSelectorResolver; }
        }

        public CCBAnimationManager AnimationManager
        {
            get { return _actionManager; }
            set { _actionManager = value; }
        }

        // Used in CCNodeLoader.parseProperties()

        public List<string> AnimatedProperties
        {
            get { return _animatedProps; }
        }

        public List<string> LoadedSpriteSheet
        {
            get { return _loadedSpriteSheets; }
        }

        public object Owner
        {
            get { return _owner; }
        }

        public static float ResolutionScale
        {
            get { return __ccbResolutionScale; }
            set { __ccbResolutionScale = value; }
        }

        public string CCBRootPath
        {
            set
            {
                Debug.Assert(value != null, "");
                _CCBRootPath = value;
            }
            get { return _CCBRootPath; }
        }

        private bool Init()
        {
            // Setup action manager
            CCBAnimationManager pActionManager = new CCBAnimationManager();
            AnimationManager = pActionManager;

            // Setup resolution scale and container size
            _actionManager.RootContainerSize = Layer.VisibleBoundsWorldspace.Size;

			HasScriptingOwner = false;

            return true;
        }

        public CCNode ReadNodeGraphFromFile(string fileName)
        {
            return ReadNodeGraphFromFile(fileName, null);
        }

        public CCNode ReadNodeGraphFromFile(string fileName, object owner)
        {
            return ReadNodeGraphFromFile(fileName, owner, Layer.VisibleBoundsWorldspace.Size);
        }

        public CCNode ReadNodeGraphFromFile(string fileName, object owner, CCSize parentSize)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                return null;
            }

            string strCCBFileName = fileName;
            string strSuffix = ".ccbi";
            // Add ccbi suffix
            if (!CCBReader.EndsWith(strCCBFileName, strSuffix))
            {
                strCCBFileName += strSuffix;
            }

            string strPath = CCFileUtils.FullPathFromRelativePath(strCCBFileName);

            var pBytes = CCFileUtils.GetFileBytes(strPath);
            byte[] data = pBytes;

            CCNode ret = ReadNodeGraphFromData(data, owner, parentSize);

            return ret;
        }

        public CCNode ReadNodeGraphFromData(byte[] bytes, object owner, CCSize parentSize)
        {
            _bytes = bytes;
            _currentByte = 0;
            _currentBit = 0;
            _owner = owner;

            _actionManager.RootContainerSize = parentSize;
            _actionManager.Owner = _owner;
            _ownerOutletNodes = new List<CCNode>();
            _ownerCallbackNodes = new List<CCNode>();

            Dictionary<CCNode, CCBAnimationManager> animationManagers = new Dictionary<CCNode, CCBAnimationManager>();
            CCNode pNodeGraph = ReadFileWithCleanUp(true, animationManagers);

            if (pNodeGraph != null && _actionManager.AutoPlaySequenceId != -1 && !_jsControlled)
            {
                // Auto play animations
                _actionManager.RunAnimationsForSequenceIdTweenDuration(_actionManager.AutoPlaySequenceId, 0);
            }
            // Assign actionManagers to userObject
            if (_jsControlled)
            {
                _nodesWithAnimationManagers = new List<CCNode>();
                _animationManagersForNodes = new List<CCBAnimationManager>();
            }

            foreach (var pElement in animationManagers)
            {
                CCNode pNode = pElement.Key;
                CCBAnimationManager manager = animationManagers[pNode];
                pNode.UserObject = manager;

                if (_jsControlled)
                {
                    _nodesWithAnimationManagers.Add(pNode);
                    _animationManagersForNodes.Add(manager);
                }
            }

            return pNodeGraph;
        }

        public CCScene CreateSceneWithNodeGraphFromFile(string fileName)
        {
            return CreateSceneWithNodeGraphFromFile(fileName, null);
        }

        public CCScene CreateSceneWithNodeGraphFromFile(string fileName, object owner)
        {
            return CreateSceneWithNodeGraphFromFile(fileName, owner, Layer.VisibleBoundsWorldspace.Size);
        }

        public CCScene CreateSceneWithNodeGraphFromFile(string fileName, object owner, CCSize parentSize)
        {
            CCNode pNode = ReadNodeGraphFromFile(fileName, owner, parentSize);
            CCScene pScene = new CCScene(Window, Viewport, Director);
            pScene.AddChild(pNode);

            return pScene;
        }

        /* Utility methods. */

        public static String LastPathComponent(String pPath)
        {
            int slashPos = pPath.IndexOf('/');
            if (slashPos != -1)
            {
                return pPath.Substring(slashPos + 1);
            }
            return pPath;
        }

        public static String DeletePathExtension(String pPath)
        {
            int dotPos = pPath.LastIndexOf('.');
            if (dotPos != -1)
            {
                return pPath.Substring(0, dotPos);
            }
            return pPath;
        }

        public static String ToLowerCase(String pString)
        {
            return pString.ToLower();
        }

        public static bool EndsWith(String pString, String pEnding)
        {
            return pString.EndsWith(pEnding);
        }

        /* Parse methods. */

        public int ReadInt(bool pSigned)
        {
            int numBits = 0;
            while (!GetBit())
            {
                numBits++;
            }

            int current = 0;
            for (int a = numBits - 1; a >= 0; a--)
            {
                if (GetBit())
                {
                    current |= 1 << a;
                }
            }
            current |= 1 << numBits;

            int num;
            if (pSigned)
            {
                int s = current % 2;
                if (s != 0)
                {
                    num = (current / 2);
                }
                else
                {
                    num = (-current / 2);
                }
            }
            else
            {
                num = current - 1;
            }

            AlignBits();

            return num;
        }

        public byte ReadByte()
        {
            byte b = _bytes[_currentByte];
            _currentByte++;
            return b;
        }

        public bool ReadBool()
        {
            return 0 != ReadByte();
        }

        public string ReadUTF8()
        {
            int b0 = ReadByte();
            int b1 = ReadByte();

            int numBytes = b0 << 8 | b1;

            string result = utf8Encoder.GetString(_bytes, _currentByte, numBytes);

            _currentByte += numBytes;

            return result;
        }

        public float ReadFloat()
        {
            var type = (CCBFloatType) ReadByte();

            switch (type)
            {
                case CCBFloatType.Float0:
                    return 0;
                case CCBFloatType.Float1:
                    return 1;
                case CCBFloatType.Minus1:
                    return -1;
                case CCBFloatType.Float05:
                    return 0.5f;
                case CCBFloatType.Integer:
                    return ReadInt(true);
                default:
                    var byteArray = new byte[4];

                    byteArray[0] = _bytes[_currentByte + 0];
                    byteArray[1] = _bytes[_currentByte + 1];
                    byteArray[2] = _bytes[_currentByte + 2];
                    byteArray[3] = _bytes[_currentByte + 3];

                    float f = BitConverter.ToSingle(byteArray, 0);
                    _currentByte += 4;
                    return f;
            }
        }

        public string ReadCachedString()
        {
            int i = ReadInt(false);
            return _stringCache[i];
        }

        public bool IsJSControlled()
        {
            return _jsControlled;
        }

        public bool ReadCallbackKeyframesForSeq(CCBSequence seq)
        {
            int numKeyframes = ReadInt(false);
            if (numKeyframes == 0) return true;

            CCBSequenceProperty channel = new CCBSequenceProperty();
            
            for (int i = 0; i < numKeyframes; ++i)
            {

                float time = ReadFloat();
                string callbackName = ReadCachedString();

                int callbackType = ReadInt(false);

                List<CCBValue> value = new List<CCBValue>();
                value.Add(new CCBValue(callbackName));
                value.Add(new CCBValue(callbackType.ToString()));

                CCBKeyframe keyframe = new CCBKeyframe();

                keyframe.Time = time;
                keyframe.Value = value;

                if (_jsControlled)
                {
                    //string callbackIdentifier;
                    _actionManager.GetKeyframeCallbacks().Add(String.Format("{0}:{1}", callbackType, callbackName));
                }

                channel.Keyframes.Add(keyframe);
            }

            seq.CallBackChannel = channel;

            return true;
        }

        public bool ReadSoundKeyframesForSeq(CCBSequence seq)
        {
            int numKeyframes = ReadInt(false);
            if (numKeyframes == 0) return true;

            CCBSequenceProperty channel = new CCBSequenceProperty();

            for (int i = 0; i < numKeyframes; ++i)
            {

                float time = ReadFloat();
                string soundFile = ReadCachedString();
                float pitch = ReadFloat();
                float pan = ReadFloat();
                float gain = ReadFloat();

                List<CCBValue> value = new List<CCBValue>();

                value.Add(new CCBValue(soundFile));
                value.Add(new CCBValue(pitch.ToString()));
                value.Add(new CCBValue(pan.ToString()));
                value.Add(new CCBValue(gain.ToString()));

                CCBKeyframe keyframe = new CCBKeyframe();
                keyframe.Time = time;
                keyframe.Value = value;
                channel.Keyframes.Add(keyframe);
            }

            seq.SoundChannel = channel;

            return true;
        }

        public List<string> OwnerCallbackNames
        {
            get { return new List<string>(_ownerCallbackNames); }
        }

        public List<CCNode> OwnerCallbackNodes
        {
            get { return _ownerCallbackNodes; }
        }

        public List<string> OwnerOutletNames
        {
            get { return new List<string>(_ownerOutletNames); }
        }

        public List<CCNode> OwnerOutletNodes
        {
            get { return _ownerOutletNodes; }
        }

        public List<CCNode> NodesWithAnimationManagers
        {
            get { return _nodesWithAnimationManagers; }
        }

        public List<CCBAnimationManager> AnimationManagersForNodes
        {
            get { return _animationManagersForNodes; }
        }

        public Dictionary<CCNode, CCBAnimationManager> AnimationManagers
        {
            get { return _actionManagers; }
            set { _actionManagers = value; }
        }

        public void AddOwnerCallbackName(string name)
        {
            _ownerCallbackNames.Add(name);
        }

        public void AddOwnerCallbackNode(CCNode node)
        {
            _ownerCallbackNodes.Add(node);
        }

        public void AddDocumentCallbackName(string name)
        {
            _actionManager.AddDocumentCallbackName(name);
        }

        public void AddDocumentCallbackNode(CCNode node)
        {
            _actionManager.AddDocumentCallbackNode(node);
        }

        public CCNode ReadFileWithCleanUp(bool bCleanUp, Dictionary<CCNode, CCBAnimationManager> am)
        {
            if (!ReadHeader())
            {
                return null;
            }

            if (!ReadStringCache())
            {
                return null;
            }

            if (!ReadSequences())
            {
                return null;
            }

            AnimationManagers = am;

            CCNode node = ReadNodeGraph(null);

            _actionManagers[node] = _actionManager;

            if (bCleanUp)
            {
                CleanUpNodeGraph(node);
            }

            return node;
        }

        public void AddOwnerOutletName(string name)
        {
            _ownerOutletNames.Add(name);
        }

        public void AddOwnerOutletNode(CCNode node)
        {
            if (node == null)
                return;

            _ownerOutletNodes.Add(node);
        }

        private void CleanUpNodeGraph(CCNode node)
        {
            node.UserObject = null;

            if (node.Children != null)
            {
                for (int i = 0; i < node.Children.Count; i++)
                {
                    CleanUpNodeGraph(node.Children[i]);
                }
            }
        }

        private bool ReadSequences()
        {
            List<CCBSequence> sequences = _actionManager.Sequences;

            int numSeqs = ReadInt(false);

            for (int i = 0; i < numSeqs; i++)
            {
                var seq = new CCBSequence();

                seq.Duration = ReadFloat();
                seq.Name = ReadCachedString();
                seq.SequenceId = ReadInt(false);
                seq.ChainedSequenceId = ReadInt(true);

                if (!ReadCallbackKeyframesForSeq(seq)) return false;
                if (!ReadSoundKeyframesForSeq(seq)) return false;

                sequences.Add(seq);
            }

            _actionManager.AutoPlaySequenceId = ReadInt(true);
            return true;
        }


        private CCBKeyframe ReadKeyframe(CCBPropertyType type)
        {
            var keyframe = new CCBKeyframe();

            keyframe.Time = ReadFloat();

            var easingType = (CCBEasingType) ReadInt(false);
            float easingOpt = 0;
            object value = null;

            if (easingType == CCBEasingType.CubicIn
                || easingType == CCBEasingType.CubicOut
                || easingType == CCBEasingType.CubicInOut
                || easingType == CCBEasingType.ElasticIn
                || easingType == CCBEasingType.ElasticOut
                || easingType == CCBEasingType.ElasticInOut)
            {
                easingOpt = ReadFloat();
            }
            keyframe.EasingType = easingType;
            keyframe.EasingOpt = easingOpt;

            if (type == CCBPropertyType.Check)
            {
                value = new CCBValue(ReadBool());
            }
            else if (type == CCBPropertyType.Byte)
            {
                value = new CCBValue(ReadByte());
            }
            else if (type == CCBPropertyType.Color3)
            {
                byte r = ReadByte();
                byte g = ReadByte();
                byte b = ReadByte();

                var c = new CCColor3B(r, g, b);
                value = new CCColor3BWapper(c);
            }
            else if (type == CCBPropertyType.Degrees)
            {
                value = new CCBValue(ReadFloat());
            }
            else if (type == CCBPropertyType.ScaleLock || type == CCBPropertyType.Position || type == CCBPropertyType.FloatXY)
            {
                float a = ReadFloat();
                float b = ReadFloat();

                value = new List<CCBValue>
                    {
                        new CCBValue(a),
                        new CCBValue(b)
                    };
            }
            else if (type == CCBPropertyType.SpriteFrame)
            {
                string spriteSheet = ReadCachedString();
                string spriteFile = ReadCachedString();

                CCSpriteFrame spriteFrame;

                if (String.IsNullOrEmpty(spriteSheet))
                {
                    spriteFile = _CCBRootPath + spriteFile;

                    CCTexture2D texture = CCTextureCache.SharedTextureCache.AddImage(CCFileUtils.RemoveExtension(spriteFile));
                    var bounds = new CCRect(0, 0, texture.ContentSizeInPixels.Width, texture.ContentSizeInPixels.Height);
                    spriteFrame = new CCSpriteFrame(texture, bounds);
                }
                else
                {
                    spriteSheet = _CCBRootPath + spriteSheet;
                    CCSpriteFrameCache frameCache = CCSpriteFrameCache.SharedSpriteFrameCache;

                    // Load the sprite sheet only if it is not loaded            
                    if (!_loadedSpriteSheets.Contains(spriteSheet))
                    {
                        frameCache.AddSpriteFrames(spriteSheet);
                        _loadedSpriteSheets.Add(spriteSheet);
                    }

                    spriteFrame = frameCache[spriteFile];
                }
                value = spriteFrame;
            }

            keyframe.Value = value;

            return keyframe;
        }

        private bool ReadHeader()
        {
            /* If no bytes loaded, don't crash about it. */
            if (_bytes == null)
            {
                return false;
            }

            /* Read magic bytes */
            if (_bytes[_currentByte + 0] != 'i' || _bytes[_currentByte + 1] != 'b' || _bytes[_currentByte + 2] != 'c' ||
                _bytes[_currentByte + 3] != 'c')
            {
                return false;
            }

            _currentByte += 4;

            /* Read version. */
            int version = ReadInt(false);
            if (version != CCBVersion)
            {
                CCLog.Log("WARNING! Incompatible ccbi file version (file: %d reader: %d)", version, CCBVersion);
                return false;
            }

            // Read JS check
            _jsControlled = ReadBool();
            _actionManager.JSControlled = _jsControlled;


            return true;
        }

        private bool ReadStringCache()
        {
            int numStrings = ReadInt(false);

            for (int i = 0; i < numStrings; i++)
            {
                _stringCache.Add(ReadUTF8());
            }

            return true;
        }

        /*private void ReadStringCacheEntry()
        {
            int b0 = ReadByte();
            int b1 = ReadByte();

            int numBytes = b0 << 8 | b1;


            string s = Encoding.UTF8.GetString(_bytes, _currentByte, numBytes);

            _currentByte += numBytes;

            _stringCache.Add(s);
        }*/

        private CCNode ReadNodeGraph()
        {
            return ReadNodeGraph(null);
        }

        private CCNode ReadNodeGraph(CCNode parent)
        {
            /* Read class name. */
            string className = ReadCachedString();

            string _jsControlledName = null;

            if (_jsControlled)
            {
                _jsControlledName = ReadCachedString();
            }

            // Read assignment type and name
            var memberVarAssignmentType = (CCBTargetType) ReadInt(false);

            string memberVarAssignmentName = String.Empty;
            if (memberVarAssignmentType != CCBTargetType.None)
            {
                memberVarAssignmentName = ReadCachedString();
            }

            CCNodeLoader ccNodeLoader = _nodeLoaderLibrary.GetCCNodeLoader(className);
            if (ccNodeLoader == null)
            {
                CCLog.Log("no corresponding node loader for {0}", className);
                return null;
            }

            CCNode node = ccNodeLoader.LoadCCNode(parent, this);

            // Set root node
            if (_actionManager.RootNode == null)
            {
                _actionManager.RootNode = node;
            }

            // Assign controller
            if (_jsControlled && node == _actionManager.RootNode)
            {
                _actionManager.DocumentControllerName = _jsControlledName;
            }

            // Read animated properties
            var seqs = new Dictionary<int, Dictionary<string, CCBSequenceProperty>>();
            _animatedProps.Clear();

            int numSequence = ReadInt(false);
            for (int i = 0; i < numSequence; ++i)
            {
                int seqId = ReadInt(false);
                var seqNodeProps = new Dictionary<string, CCBSequenceProperty>();

                int numProps = ReadInt(false);

                for (int j = 0; j < numProps; ++j)
                {
                    var seqProp = new CCBSequenceProperty();

                    seqProp.Name = ReadCachedString();
                    seqProp.Type = (CCBPropertyType) ReadInt(false);
                    _animatedProps.Add(seqProp.Name);

                    int numKeyframes = ReadInt(false);

                    for (int k = 0; k < numKeyframes; ++k)
                    {
                        CCBKeyframe keyframe = ReadKeyframe(seqProp.Type);

                        seqProp.Keyframes.Add(keyframe);
                    }

                    seqNodeProps.Add(seqProp.Name, seqProp);
                }

                seqs.Add(seqId, seqNodeProps);
            }

            if (seqs.Count > 0)
            {
                _actionManager.AddNode(node, seqs);
            }

            // Read properties
            ccNodeLoader.ParseProperties(node, parent, this);

            bool isCCBFileNode = node is CCBFile;

            // Handle sub ccb files (remove middle node)
            if (isCCBFileNode)
            {
                var ccbFileNode = (CCBFile) node;

                CCNode embeddedNode = ccbFileNode.FileNode;
                embeddedNode.Position = ccbFileNode.Position;
                embeddedNode.RotationX = ccbFileNode.RotationX;
                embeddedNode.RotationY = ccbFileNode.RotationY;
                embeddedNode.ScaleX = ccbFileNode.ScaleX;
                embeddedNode.ScaleY = ccbFileNode.ScaleY;
                embeddedNode.Tag = ccbFileNode.Tag;
                embeddedNode.Visible = true;
                //embeddedNode.IgnoreAnchorPointForPosition = ccbFileNode.IgnoreAnchorPointForPosition;

                _actionManager.MoveAnimationsFromNode(ccbFileNode, embeddedNode);

                ccbFileNode.FileNode = null;

                node = embeddedNode;
            }

#if CCB_ENABLE_JAVASCRIPT
    /*
     if (memberVarAssignmentType && memberVarAssignmentName && ![memberVarAssignmentName isEqualToString:@""])
     {
     [[JSCocoa sharedController] setObject:node withName:memberVarAssignmentName];
     }*/
#else
            if (memberVarAssignmentType != CCBTargetType.None)
            {
                if (!_jsControlled)
                {
                    object target = null;
                    if (memberVarAssignmentType == CCBTargetType.DocumentRoot)
                    {
                        target = _actionManager.RootNode;
                    }
                    else if (memberVarAssignmentType == CCBTargetType.Owner)
                    {
                        target = _owner;
                    }

                    if (target != null)
                    {
                        var targetAsCCBMemberVariableAssigner = target as ICCBMemberVariableAssigner;

                        bool assigned = false;
                        if (memberVarAssignmentType != CCBTargetType.None)
                        {
                            if (targetAsCCBMemberVariableAssigner != null)
                            {
                                assigned = targetAsCCBMemberVariableAssigner.OnAssignCCBMemberVariable(target,
                                                                                                       memberVarAssignmentName,
                                                                                                       node);
                            }

                            if (!assigned && _CCBMemberVariableAssigner != null)
                            {
                                _CCBMemberVariableAssigner.OnAssignCCBMemberVariable(target, memberVarAssignmentName,
                                                                                     node);
                            }
                        }
                    }
                }
                else
                {
                    if (memberVarAssignmentType == CCBTargetType.DocumentRoot)
                    {
                        _actionManager.AddDocumentOutletName(memberVarAssignmentName);
                        _actionManager.AddDocumentOutletNode(node);
                    }
                    else
                    {
                        _ownerOutletNames.Add(memberVarAssignmentName);
                        _ownerOutletNodes.Add(node);
                    }
                }
            }

            // Assign custom properties.
    if (ccNodeLoader.CustomProperties.Count > 0)
    {
        bool customAssigned = false;
        
        if(!_jsControlled)
        {
            Object target = node;
            if(target != null)
            {
                ICCBMemberVariableAssigner targetAsCCBMemberVariableAssigner = target as ICCBMemberVariableAssigner;
                if(targetAsCCBMemberVariableAssigner != null) {
                    
                    var pCustomPropeties = ccNodeLoader.CustomProperties;
                    foreach (var pElement in pCustomPropeties)
                    {
                        customAssigned = targetAsCCBMemberVariableAssigner.OnAssignCCBCustomProperty(target, pElement.Key, pElement.Value);

                        if(!customAssigned && _CCBMemberVariableAssigner != null)
                        {
                            customAssigned = _CCBMemberVariableAssigner.OnAssignCCBCustomProperty(target, pElement.Key, pElement.Value);
                        }
                    }
                }
            }
        }
    }
#endif
            // CCB_ENABLE_JAVASCRIPT

            _animatedProps.Clear();

            /* Read and add children. */
            int numChildren = ReadInt(false);
            for (int i = 0; i < numChildren; i++)
            {
                CCNode child = ReadNodeGraph(node);
                node.AddChild(child);
            }

            if (!isCCBFileNode)
            {
                // Call onNodeLoaded
                var nodeAsCCNodeLoaderListener = node as ICCNodeLoaderListener;
                if (nodeAsCCNodeLoaderListener != null)
                {
                    nodeAsCCNodeLoaderListener.OnNodeLoaded(node, ccNodeLoader);
                }
                else if (_nodeLoaderListener != null)
                {
                    _nodeLoaderListener.OnNodeLoaded(node, ccNodeLoader);
                }
            }

            return node;
        }

        private bool GetBit()
        {
            bool bit;
            byte b = _bytes[_currentByte];
            if ((b & (1 << _currentBit)) != 0)
            {
                bit = true;
            }
            else
            {
                bit = false;
            }

            _currentBit++;

            if (_currentBit >= 8)
            {
                _currentBit = 0;
                _currentByte++;
            }

            return bit;
        }


        private void AlignBits()
        {
            if (_currentBit != 0)
            {
                _currentBit = 0;
                _currentByte++;
            }
        }


    }
}