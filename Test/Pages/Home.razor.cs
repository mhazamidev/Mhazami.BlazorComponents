namespace Test.Pages;

public partial class Home
{
    private List<Person> people = Person.Model();
    private List<Person> person = new List<Person>
    {
       new Person { Id = 2,Name="Sarah"},
            new Person { Id = 3,Name="Jack"},
    };

    private List<Person> personable = new List<Person>
    {
       new Person { Id = 5,Name="Tomas"}, 
    };
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
}