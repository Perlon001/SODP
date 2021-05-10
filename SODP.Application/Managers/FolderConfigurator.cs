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
            OSPrefix = String.Format("{0}Settings:",Environment.OSVersion.Platform.ToString());
            ProjectFolder = _configuration.GetSection(String.Format("{0}ActiveFolder",OSPrefix)).Value;
            ArchiveFolder = _configuration.GetSection(String.Format("{0}ArchiveFolder",OSPrefix)).Value;
        }
    }
}