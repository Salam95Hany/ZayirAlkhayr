using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZayirAlkhayr.Entities.Contracts.DTOs.Setting;

namespace ZayirAlkhayr.Interface.Setting
{
    public interface IBackupService
    {
        Task<BackupArchiveDto> CreateImagesBackupAsync(string backupType);
    }
}
