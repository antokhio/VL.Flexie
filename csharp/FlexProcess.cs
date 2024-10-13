namespace VL.Flexie
{

    [ProcessNode(HasStateOutput = true)]
    public class FlexProcess
    {
        public List<FlexNode> Update(FlexNode? root = null, float canvasWidth = 200, float canvasHeight = 200)
        {
            if (root is null) return new List<FlexNode>();

            var pass = FlexUtils.BreadthFirstTopDownTraversal(root, (node) =>
            {
                node.Children?.ForEach(child =>
                {
                    if (child is null) return;
                    child.SetParent(node);
                });

                return node?.Children?.ToList() ?? new List<FlexNode>();
            })
                .ToList();


            foreach (var node in pass)
            {
                if (node.Parent is null)
                {
                    node.Result = new FlexResult
                    {
                        Width = canvasWidth,
                        Height = canvasHeight,
                        X = 0,
                        Y = 0,
                    };
                }

                var flexDirection = node?.Style?.FlexDirection ?? FlexDirection.Row;
                var childrenCount = node?.Children?.Count ?? 1;

                var gap = node?.Style?.Gap ?? 0;
                var totalGaps = gap * (childrenCount - 1);

                var padding = node?.Style?.Padding ?? Padding.Zero();

                var innerWidth = node.Result.Width - padding.Left - padding.Right;
                var innerHeight = node.Result.Height - padding.Top - padding.Bottom;

                if (flexDirection is FlexDirection.Row)
                {
                    var width = (innerWidth - totalGaps) / node?.Children?.Count ?? 1;

                    for (int i = 0; i < node?.Children?.Count; i++)
                    {
                        node.Children[i].Result = new FlexResult
                        {
                            Width = width,
                            Height = innerHeight,
                            X = (width / 2 + width * i + gap * i - innerWidth / 2) + node.Result.X,
                            Y = node.Result.Y
                        };
                    }
                }

                if (flexDirection is FlexDirection.Column)
                {
                    var height = (innerHeight - totalGaps) / node?.Children?.Count ?? 1;

                    for (int i = 0; i < node.Children?.Count; i++)
                    {
                        node.Children[i].Result = new FlexResult
                        {
                            Width = node.Result.Width - padding.Left - padding.Right,
                            Height = height,
                            X = node.Result.X,
                            Y = (height / 2 + height * i + gap * i - innerHeight / 2) + node.Result.Y,
                        };
                    }
                }
            }

            return pass;
        }
    }
}
