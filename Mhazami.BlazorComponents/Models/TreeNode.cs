namespace Mhazami.BlazorComponents.Models;

public record class TreeNode
{
    public string Id { get; set; }
    public string Title { get; set; }
    public string? Parent { get; set; }
    public string? Icon { get; set; }
    public List<TreeNode> Children { get; set; } = new List<TreeNode>();
    private bool _hasChildren;
    public bool HasChildren
    {
        get
        {
            if (Children is not null && Children.Any())
                return true;
            return _hasChildren;
        }
        set
        {
            _hasChildren = value;
        }
    }
    public bool IsExpanded { get; set; }

    internal bool ShowIcons { get; set; } = false;
}
