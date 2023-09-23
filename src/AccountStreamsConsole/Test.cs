using System;
namespace AccountStreamsConsole;
public class Test
{
    public string Username { get; set; }
    public string Password { get; set; }
    public string Fullname { get; set; }
    public bool Activated { get; set; }

    public override string ToString()
    {
        return Username + " " + Fullname;
    }
}
