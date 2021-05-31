using Microsoft.Extensions.Configuration;
using SODP.Domain.Managers;
using SODP.Model;
using SODP.Model.Enums;
using System;

namespace SODP.Application.Managers
{
    public class FolderCommandCreator : IFolderCommandCreator
    {
        private readonly IConfiguration _configuration;
        private readonly FolderConfigurator _folderConfigurator;

        public FolderCommandCreator(IConfiguration configuration, FolderConfigurator folderConfigurator)
        {
            _configuration = configuration;
            _folderConfigurator = folderConfigurator;
        }

        public string GetCommandCreateFolder(Project project)
        {
            return $"{GetCommand(FolderCommands.Create)} {_folderConfigurator.ActiveFolder} {project}";
        }

        public string GetCommandRenameFolder(Project project, string oldName)
        {
            return GetCommandRenameFolder(project, oldName, ProjectsFolder.Active);
        }

        public string GetCommandRenameFolder(Project project, string oldName, ProjectsFolder source)
        {

            return $"{GetCommand(FolderCommands.Rename)} {_folderConfigurator.GetProjectFolder(source)} {oldName} {project}";
        }

        public string GetCommandArchiveFolder(Project project)
        {
            return $"{GetCommand(FolderCommands.Archive)} {_folderConfigurator.ActiveFolder} {_folderConfigurator.ArchiveFolder} {project}"; 
        }

        public string GetCommandRestoreFolder(Project project)
        {
            return $"{GetCommand(FolderCommands.Restore)} {_folderConfigurator.ArchiveFolder} {_folderConfigurator.ActiveFolder} {project}";
        }

        public string GetCommandDeleteFolder(Project project)
        {
            return $"{GetCommand(FolderCommands.Delete)} {_folderConfigurator.ActiveFolder} {project}";
        }

        private string GetCommand(FolderCommands command)
        {
            var OSPrefix = $"{Environment.OSVersion.Platform}Settings:";
            return _configuration.GetSection($"{OSPrefix}{command}Command").Value;
        }


    }
}
