namespace ZayirAlkhayr.Entities.Contracts.DTOs.Setting
{
    public class FactoryResetStepResultDto
    {
        public string Key { get; set; }
        public string DisplayName { get; set; }
        public string StatusMessage { get; set; }
        public int DeletedRecordsCount { get; set; }
        public int RemainingRecordsCount { get; set; }
        public int ExecutionOrder { get; set; }
    }
}
