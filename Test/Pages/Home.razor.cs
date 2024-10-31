using Mhazami.BlazorComponents.Models;
using Microsoft.AspNetCore.Components.Web;

namespace Test.Pages;

public partial class Home
{
    private List<Person> people = Person.Model();
    private List<Person> people2 = Person.Model2();
    private List<TreeNode> person = new List<TreeNode>
    {
      new TreeNode{Id = "1", Title = "Iran",HasChildren =false},
      new TreeNode{Id = "2",Title ="Germany",HasChildren = false},
      new TreeNode{Id = "3",Title ="Canada", HasChildren = true,Children = new List<TreeNode>
      {
        new TreeNode{Id = "5", Title = "Tehran",HasChildren =true,Children = new List < TreeNode > {
           new TreeNode{Id = "7", Title = "Darband",HasChildren =false}
        }},
      new TreeNode{Id = "6",Title ="Isfahan",HasChildren = false},
      }
      },
      new TreeNode{Id = "4",Title ="USA"},
    };

    void Onchecked(string value)
    {
        StateHasChanged();
    }

    void test(DateTime dateTime)
    {

    }
    async Task Click()
    {

    }

    async Task TreeClick(TreeNode node)
    {

    }
}


public class Person
{
    public int Id { get; set; }
    public string Name { get; set; }

    public static List<Person> Model()
    {
        return new List<Person>
        {
            new Person { Id = 1,Name="John"},
            new Person { Id = 2,Name="Sarah"},
            new Person { Id = 3,Name="Jack"},
            new Person { Id = 4,Name="Jili"},
            new Person { Id = 5,Name="Tomas"},
        };
    }

    public static List<Person> Model2()
    {
        return new List<Person>
        {
            new Person { Id = 1,Name="John"},

            new Person { Id = 4,Name="Jili"},
            new Person { Id = 5,Name="Tomas"},
        };
    }
}