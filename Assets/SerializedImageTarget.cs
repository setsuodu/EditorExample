using UnityEditor;

public class SerializedImageTarget : SerializedTrackables
{
    private readonly SerializedProperty mAspectRatio;
    private readonly SerializedProperty mHeight;
    private readonly SerializedProperty mImageTargetType;
    private readonly SerializedProperty mWidth;

    public SerializedImageTarget(SerializedObject target) : base (target)
    {
        this.mAspectRatio = target.FindProperty("mAspectRatio");
        this.mImageTargetType = target.FindProperty("mImageTargetType");
    }

    public float AspectRatio
    {
        get
        {
            return this.mAspectRatio.floatValue;
        }
        set
        {
            this.mAspectRatio.floatValue = value;
        }
    }

    public SerializedProperty AspectRatioProperty()
    {
        return this.mAspectRatio;
    }

    public float Height
    {
        get
        {
            return this.mWidth.floatValue;
        }
        set
        {
            this.mWidth.floatValue = value;
        }
    }

    public SerializedProperty HeightProperty()
    {
        return this.mHeight;
    }

    public ImageTargetType ImageTargetType
    {
        get
        {
            return (ImageTargetType)this.mImageTargetType.enumValueIndex;
        }
        set
        {
            this.mImageTargetType.enumValueIndex = (int)value;
        }
    }

    public SerializedProperty ImageTargetTypeProperty()
    {
        return this.mImageTargetType;
    }

    public float Width
    {
        get
        {
            return this.mWidth.floatValue;
        }
        set
        {
            this.mWidth.floatValue = value;
        }
    }

    public SerializedProperty WidthProperty()
    {
        return this.mWidth;
    }
}
