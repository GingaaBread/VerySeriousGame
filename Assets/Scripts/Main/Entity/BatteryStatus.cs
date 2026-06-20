namespace Main.Entity
{
    public class BatteryStatus
    {
        public int CurrentBatteryLevel { get; set; } = 10;
        public int CurrentMaxBatteryLevel { get; set; } = 10;
        public int CurrentDepletionInterval { get; set; } = 3;
    }
}