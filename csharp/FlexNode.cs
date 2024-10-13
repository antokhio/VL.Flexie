namespace VL.Flexie
{
    public enum FlexDirection
    {
        Row, Column
    }

    public record struct Padding
    {
        public float Left { get; set; }
        public float Right { get; set; }
        public float Top { get; set; }
        public float Bottom { get; set; }
        public static Padding Zero() => new Padding { Left = 0.0f, Right = 0.0f, Bottom = 0.0f, Top = 0.0f };
    }

    public static class Paddings
    {
        public static Padding Padding(float padding) => new Padding { Left = padding, Right = padding, Bottom = padding, Top = padding };
        public static Padding PaddingLRTB(float left, float right, float top, float bottom) => new Padding { Left = left, Right = right, Top = top, Bottom = bottom };
        public static Padding PaddingH(float horizontal) => new Padding { Left = horizontal, Right = horizontal, Top = 0, Bottom = 0 };
        public static Padding PaddingV(float vertical) => new Padding { Left = 0, Right = 0, Top = vertical, Bottom = vertical };
        public static Padding PaddingHV(float horizontal, float vertical) => new Padding { Left = horizontal, Right = vertical, Top = vertical, Bottom = vertical };
    }


    public record struct FlexStyle
    {
        public FlexDirection FlexDirection { get; set; }
        public int Flex { get; set; }
        public float Gap { get; set; }
        public Padding Padding { get; set; }
        public int? Width { get; set; }
        public int? Height { get; set; }
    }

    [ProcessNode()]
    public class FlexStyleNode
    {
        public FlexStyle Update(FlexDirection flexDirection = Flexie.FlexDirection.Row, float gap = 0, Padding? padding = null, int? width = null, int? height = null)
        {
            return new FlexStyle
            {
                FlexDirection = flexDirection,
                Gap = gap,
                Padding = padding ?? Padding.Zero(),
                Width = width,
                Height = height
            };
        }
    }

    public record struct FlexResult
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Width { get; set; }
        public float Height { get; set; }

        public void Split(out float x, out float y, out float width, out float height)
        {
            x = X;
            y = Y;
            width = Width;
            height = Height;
        }
    }

    [ProcessNode(HasStateOutput = true)]
    public class FlexNode
    {

        [Pin(PinGroupKind = Model.PinGroupKind.Collection, PinGroupDefaultCount = 1)]
        public Spread<FlexNode> Children { internal get; set; }

        public FlexStyle? Style { internal get; set; }

        [Pin(Visibility = Model.PinVisibility.Hidden)]
        public FlexNode Parent { internal get; set; } = null;

        public FlexResult Result { get; internal set; }
        public void SetParent(FlexNode parent) => Parent = parent;
    }
}
