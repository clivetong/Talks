// https://refactoring.guru/design-patterns/memento

// Serialize state but from the inside
//  - more than just JSON serializing the public fields of an object

using System.Text.Json;

// You get issues if you do this from the outside

Me me = new 
(
    Id : 1,
    ssn : 2,
    Message : "A Message"
);

Console.WriteLine(me);

string json = JsonSerializer.Serialize(me);
Console.WriteLine(json);

Me me2 = JsonSerializer.Deserialize<Me>(json);

Console.WriteLine(me2);

// Move the logic into the class itself - that's the purpose of this pattern
//  some fields might only make sense for certain other field values say

class Me
{
    public int Id { get; }
    public int Ssn { get; }
    public string Message { get; }

    public Me(int Id, int ssn, string Message)
    {
        this.Id = Id;
        Ssn = ssn;
        this.Message = Message;
    }
    void AndBehaviour()
    {
        Console.WriteLine(Ssn);
    }
}