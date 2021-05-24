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
        private readonly FolderConfigurator _folderConfigurator;
        protected readonly string _projectFolder;
        protected readonly string _archiveFolder;

        public FolderCommandCreator(IConfiguration configuration, FolderConfigurator folderConfigurator)
        {
            _configuration = configuration;
            _folderConfigurator = folderConfigurator;
            _projectFolder = folderConfigurator.ProjectFolder;
            _archiveFolder = folderConfigurator.ArchiveFolder;
        }

        public string GetCreateFolderCommand(Project project)
        {
            return $"{GetCommand(FolderCommands.Create)} {_projectFolder} {project}";
        }

        public string GetRenameFolderCommand(string oldFolderName, Project project)
        {
            return $"{GetCommand(FolderCommands.Rename)} {_projectFolder} {oldFolderName} {project}";
        }

        public string GetArchiveFolderCommand(Project project)
        {
            return $"{GetCommand(FolderCommands.Archive)} {_projectFolder} {_archiveFolder} {project}"; 
        }

        public string GetRestoreFolderCommand(Project project)
        {
            return $"{GetCommand(FolderCommands.Restore)} {_archiveFolder} {_projectFolder} {project}";
        }

        public string GetDeleteFolderCommand(Project project)
        {
            return $"{GetCommand(FolderCommands.Delete)} {_projectFolder} {project}";
        }

        private string GetCommand(FolderCommands command)
        {
            return _configuration.GetSection($"{_folderConfigurator.OSPrefix}{command}Command").Value;
        }
    }
}
