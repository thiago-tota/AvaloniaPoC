namespace AvaloniaWebBrowserControl.DTO
{
    public class EmbeddedBrowserSettings
    {
        public string? Key { get; set; }
        public string? Value { get; set; }
        public EmbeddedBrowserSettingsAction Action { get; set; }
    }

    public enum EmbeddedBrowserSettingsAction : byte
    {
        Add,
        Remove
    }
}
