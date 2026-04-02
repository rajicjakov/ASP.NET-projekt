public class TabMeasure
{
    public int CapitalNumber { get; set; }
    public (int NumberOfNotes, int Note) TimeSignature { get; set; }

    public List<TabColumn> Columns { get; set; }
}