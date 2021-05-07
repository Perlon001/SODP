using Microsoft.Extensions.Configuration;
using SODP.Domain.Managers;
using SODP.Model;
using SODP.Model.Enums;
using System;

namespace SODP.Application.Managers
{
    public class FolderCommandCreator : IFolderCommandCreator
    {
        protected readonly IConfiguration _configuration;
        protected readonly string _projectFolder;
        protected readonly string _archiveFolder;
        protected readonly string _settingsPrefix;

        public FolderCommandCreator(IConfiguration configuration)
        {
            _configuration = configuration;
            _settingsPrefix = String.Format("{0}Settings:",Environment.OSVersion.Platform.ToString());
            _projectFolder = _configuration.GetSection(String.Format("{0}ActiveFolder",_settingsPrefix)).Value;
            _archiveFolder = _configuration.GetSection(String.Format("{0}ArchiveFolder",_settingsPrefix)).Value;
        }

        public string GetCreateFolderCommand(Project project)
        {
            return String.Format("{0} {1} {2}",GetCommand(FolderCommands.Create), _projectFolder, project.ToString());
        }

        public string GetRenameFolderCommand(string oldFolderName, Project project)
        {
            return String.Format("{0} {1} {2} {3}", GetCommand(FolderCommands.Rename), _projectFolder, oldFolderName, project.ToString());
        }

        public string GetArchiveFolderCommand(Project project)
        {
            return String.Format("{0} {1} {2} {3}", GetCommand(FolderCommands.Archive), _projectFolder, _archiveFolder, project.ToString()); 
        }

        public string GetRestoreFolderCommand(Project project)
        {
            return String.Format("{0} {1} {2} {3}", GetCommand(FolderCommands.Restore), _archiveFolder, _projectFolder, project.ToString());
        }

        public string GetDeleteFolderCommand(Project project)
        {
            return String.Format("{0} {1} {2}", GetCommand(FolderCommands.Delete), _projectFolder, project.ToString());
        }

        private string GetCommand(FolderCommands command)
        {
            return _configuration.GetSection(_settingsPrefix + command.ToString() + "Command" ).Value;
        }
    }
}
