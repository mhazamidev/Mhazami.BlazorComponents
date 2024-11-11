using Mhazami.BlazorComponents.Models;
using Microsoft.AspNetCore.Components;

namespace Mhazami.BlazorComponents.Components;

public partial class TreeView
{
    [Parameter] public List<TreeNode> Model { get; set; }
    [Parameter] public bool IsOndemand { get; set; }
    [Parameter] public bool HasCheckBox { get; set; }
    [Parameter] public EventCallback<string> OndemandEvent { get; set; }
    [Parameter] public EventCallback<List<TreeNode>> CheckedListCallback { get; set; }
    [Parameter] public EventCallback<TreeNode> CheckedNodeCallback { get; set; }
    [Parameter] public string ExpendedIcon { get; set; } = "<i class=\"fal fa-plus\"></i>";
    [Parameter] public string UnExpendedIcon { get; set; } = "<i class=\"fa-solid fa-minus\"></i>";
    [Parameter] public bool ExpendAll { get; set; } = false;
    [Parameter] public List<TreeNode> CheckedList { get; set; } = new List<TreeNode>();
    [Parameter] public bool Updateble { get; set; } = false;
    [Parameter] public bool Removable { get; set; } = false;
    [Parameter] public RenderFragment ChildContent { get; set; }
    [Parameter] public EventCallback<TreeNode> OnUpdate { get; set; }
    [Parameter] public EventCallback<TreeNode> OnClick { get; set; }
    [Parameter] public EventCallback<TreeNode> OnDelete { get; set; }
    [Parameter] public string Tooltip { get; set; }
    [Parameter] public string OnSelectedCssClass { get; set; } = "";
    private List<TreeNode> MainNodes = new List<TreeNode>();
    private bool OpenUpdateModal = false;
    private TreeNode? SelectedNodeForDelete = null;
    private bool HasOnClick = false;
    private TreeNode? SelectedNode = null;
    string OnSelectClass = "";
    TreeViewChild childTree;
    [Parameter] public EventCallback<TreeNode> ChangeSelectedNode { get; set; }
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        HasOnClick = OnClick.HasDelegate;
        PrepareTree();
        StateHasChanged();
    }

    public void Refresh()
    {
        PrepareTree();
        StateHasChanged();
    }

    public void Refresh(IEnumerable<TreeNode> nodes)
    {
        Model = nodes.ToList();
        Refresh();
    }



    protected override void OnParametersSet()
        => PrepareTree();
    void CloseModal() => OpenUpdateModal = false;

    void PrepareTree()
    {
        var parents = Model.Where(x => !string.IsNullOrEmpty(x.Parent)).Select(x => x.Parent);
        MainNodes = Model.Where(x => string.IsNullOrEmpty(x.Parent) || !parents.Contains(x.Parent)).ToList();
        StateHasChanged();
    }

    async void Expend(string nodeId)
    {
        var node = Model.FirstOrDefault(x => x.Id == nodeId);
        if (node is not null)
            node.IsExpanded = !node.IsExpanded;
        if (IsOndemand)
            await LoadOndemand(nodeId);
    }

    async Task CheckedNode(TreeNode node)
    {
        if (node is not null)
        {
            var item = CheckedList.FirstOrDefault(x => x.Id == node.Id);
            if (item is null)
            {
                CheckedList.Add(node);
                await CheckedNodeCallback.InvokeAsync(node);
            }
            else
                CheckedList.Remove(item);

            await CheckedListCallback.InvokeAsync(CheckedList);

        }
    }

    async Task LoadOndemand(string parentId)
    {
        await OndemandEvent.InvokeAsync(parentId);
    }

    async Task UpdateNode(TreeNode node)
    {
        OpenUpdateModal = true;
        await OnUpdate.InvokeAsync(node);
    }

    void DeleteNode(TreeNode node)
    {
        SelectedNodeForDelete = node;
    }

    async Task ConfirmDelete(bool confirm)
    {
        if (!confirm)
        {
            SelectedNodeForDelete = null;
            return;
        }

        await OnDelete.InvokeAsync(SelectedNodeForDelete);
    }

    async Task CallOnClick(TreeNode node)
    {
        SelectedNode = node;
        SetOnClickSelectedClass(node);
        if (childTree is not null)
            childTree.SelectedNode = node;
        await OnClick.InvokeAsync(node);
    }

    string SetOnClickSelectedClass(TreeNode node)
    {
        if (!string.IsNullOrEmpty(OnSelectedCssClass))
            OnSelectClass = OnSelectedCssClass;
        else
            OnSelectClass = "isselected";

        return OnSelectClass;
    }
}
