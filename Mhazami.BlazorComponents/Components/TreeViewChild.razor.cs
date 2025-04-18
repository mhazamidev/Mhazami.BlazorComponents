﻿using Mhazami.BlazorComponents.Models;
using Microsoft.AspNetCore.Components;

namespace Mhazami.BlazorComponents.Components;

public partial class TreeViewChild
{
    [Parameter] public List<TreeNode> Model { get; set; }
    [Parameter] public List<TreeNode> Nodes { get; set; }
    [Parameter] public bool HasCheckBox { get; set; }
    [Parameter] public string Parent { get; set; }
    [Parameter] public EventCallback<string> OndemandEvent { get; set; }
    [Parameter] public bool IsOndemand { get; set; }
    [Parameter] public EventCallback<TreeNode> CheckedNodeCallback { get; set; }
    [Parameter] public string ExpendedIcon { get; set; }
    [Parameter] public string UnExpendedIcon { get; set; }
    [Parameter] public bool ExpendAll { get; set; } = false;
    [Parameter] public List<TreeNode> CheckedList { get; set; } = new List<TreeNode>();
    [Parameter] public bool Updateble { get; set; } = false;
    [Parameter] public bool Removable { get; set; } = false;
    [Parameter] public RenderFragment ChildContent { get; set; }
    [Parameter] public EventCallback<TreeNode> OnUpdate { get; set; }
    [Parameter] public EventCallback<TreeNode> OnClick { get; set; }
    [Parameter] public EventCallback<TreeNode> OnDelete { get; set; }
    [Parameter] public EventCallback<TreeNode> ChangeSelectedNode { get; set; }
    [Parameter] public bool AllowHtml { get; set; } = false;
    [Parameter] public string OnSelectedCssClass { get; set; } = "";
    [Parameter] public string Tooltip { get; set; }
    private TreeNode? SelectedNodeForDelete = null;
    public TreeNode? SelectedNode = null;
    private bool OpenUpdateModal = false;
    private bool HasOnClick = false;
    string OnSelectClass = "";
    TreeViewChild childTree;
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        HasOnClick = OnClick.HasDelegate;
        PrepareTree();
    }

    void CloseModal() => OpenUpdateModal = false;
    void PrepareTree()
    {
        foreach (var item in Model)
        {
            var children = Nodes.Where(x => x.Parent == item.Id).ToList();
            if (children.Any())
                item.Children = children;
        }
    }

    async Task Expend(string nodeId)
    {
        var node = Model.FirstOrDefault(x => x.Id == nodeId);
        if (node is not null)
            node.IsExpanded = !node.IsExpanded;
        if (IsOndemand)
            await OndemandEvent.InvokeAsync(nodeId);
    }

    async Task CheckedNode(TreeNode node)
    {
        await CheckedNodeCallback.InvokeAsync(node);
    }


    async Task UpdateNode(TreeNode node)
    {
        await OnUpdate.InvokeAsync(node);
    }

    async Task DeleteNode(TreeNode node)
    {
        SelectedNodeForDelete = node;
        await OnDelete.InvokeAsync(node);
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
        await ChangeSelectedNode.InvokeAsync(node);
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
