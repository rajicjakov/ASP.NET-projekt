public class Tab
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Artist { get; set; }
    public int CreatorId { get; set; }
    public User Creator { get; set; }
    public DateTime DateCreated { get; set; }
    public string StringTuning {  get; set; } //format npr. E-A-D-G-H-E
    public int BPM { get; set; }
    public Difficulty Difficulty { get; set; }

    public List<TabMeasure> Measures { get; set; }
}