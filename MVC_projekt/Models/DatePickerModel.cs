namespace MVC_projekt.Models
{
    public class DatePickerModel
    {
        public string Name { get; set; } = string.Empty;
        public string Label { get; set; } = string.Empty;
        public string Placeholder { get; set; } = string.Empty;
        public DateTime? Value { get; set; }
    }
}