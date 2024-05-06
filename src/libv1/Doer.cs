using dllHell;

namespace mylib;

public class Doer : ITHING_DOER
{
    public IList<string> DoThing()
    {
         return new List<string>(){"a1","b1"};
    }
}