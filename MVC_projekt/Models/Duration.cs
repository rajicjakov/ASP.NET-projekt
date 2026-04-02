public class Duration
{
    public int Base {  get; set; }//1=cijela nota, 2=polovinka, 4=èetvrtinka etc.
    public bool IsDotted { get; set; }

    public float GetTotal()
    {
        if (IsDotted)
            return 1 / Base + 1 / (Base / 2);
        return 1 / Base;
    }
}