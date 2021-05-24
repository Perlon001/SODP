using Microsoft.Extensions.Configuration;
using SODP.Domain.Managers;
using SODP.Model;
using SODP.Model.Enums;
using System;

namespace SODP.Application.Managers
{
    public class FolderConfigurator
    {
        private readonly IConfiguration _configuration;
        public readonly string OSPrefix;
        public readonly string ProjectFolder;
        public readonly string ArchiveFolder;

        public FolderConfigurator(IConfiguration configuration)
        {
            _configuration = configuration;
            OSPrefix = $"{Environment.OSVersion.Platform}Settings:";
            ProjectFolder = _configuration.GetSection($"{OSPrefix}ActiveFolder").Value;
            ArchiveFolder = _configuration.GetSection($"{OSPrefix}ArchiveFolder").Value;
        }
    }
}