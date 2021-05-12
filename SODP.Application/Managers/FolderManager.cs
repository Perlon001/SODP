using Microsoft.Extensions.Configuration;
using SODP.Domain.Managers;
using SODP.Model.Enums;
using SODP.Model;
using SODP.Model.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SODP.Application.Managers
{
    public class FolderManager : IFolderManager
    {
        private readonly IFolderCommandCreator _folderCommandCreator;
        private readonly string _projectFolder;
        private readonly string _archiveFolder;

        public FolderManager(FolderConfigurator folderConfigurator, IFolderCommandCreator folderCommandCreator)
        {
            _projectFolder = folderConfigurator.ProjectFolder;
            _archiveFolder = folderConfigurator.ArchiveFolder;
            _folderCommandCreator = folderCommandCreator;
        }

        public async Task<(bool Success, string Message)> CreateFolderAsync(Project project)
        {
            (bool Success, string Message) result;
            string command;
            var catalog = GetMatchingFolders(_projectFolder, project);
            switch(catalog.Count())
            {
                case 0:
                    command = _folderCommandCreator.GetCreateFolderCommand(project);
                    result = await FolderOperationTask(command, project.ToString(), true);
                    return (result.Success, String.Format("{0} {1}", command, result.Message));
                case 1:
                    command = _folderCommandCreator.GetRenameFolderCommand(catalog[0], project);
                    result = await FolderOperationTask(command, project.ToString(), true);
                    return (result.Success, String.Format("{0} {1}", command, result.Message));
                default:
                    return (false, String.Format("Istnieje więcej niż 1 folder projektu {0}", project.Symbol));
            }
        }

        public async Task<(bool Success, string Message)> RenameFolderAsync(Project project)
        {
            var catalog = GetMatchingFolders(_projectFolder, project);
            switch(catalog.Count())
            {
                case 0:                    
                    return await CreateFolderAsync(project);
                case 1:
                    var command = _folderCommandCreator.GetRenameFolderCommand(catalog[0], project);
                    var result = await FolderOperationTask(command, project.ToString(), true);
                    return (result.Success, String.Format("{0} {1}", command, result.Message));
                default:
                    return (false, String.Format("Istnieje więcej niż 1 folder projektu {0}", project.Symbol));
            }
        }

        public async Task<(bool Success, string Message)> DeleteFolderAsync(Project project)
        {
            (bool Success, string Message) result;
            var catalog = GetMatchingFolders(_projectFolder, project);
            switch(catalog.Count())
            {
                case 0:
                    return(true, String.Format("Folder projektu {0} nie istnieje.", project.Symbol));
                case 1:
                    if(!FolderIsEmpty(String.Format("{0}{1}",_projectFolder, catalog[0])))
                    {
                        return (false,String.Format("Folder projektu {0} nie jest pusty.", project.Symbol));
                    }
                    var command = _folderCommandCreator.GetDeleteFolderCommand(project);
                    result = await FolderOperationTask(command, catalog[0], false);
                    return (result.Success, String.Format("{0} {1}", command, result.Message));
                default:
                    return (false, String.Format("Istnieje więcej niż 1 folder projektu {0}", project.Symbol));
            }
        }

        public async Task<(bool Success, string Message)> ArchiveFolderAsync(Project project)
        {
            (bool Success, string Message) result;
            var catalog = GetMatchingFolders(_projectFolder, project);
            switch(catalog.Count())
            {
                case 0:
                    return(false, String.Format("Folder projektu {0} nie istnieje.", project.Symbol));
                case 1:
                    if(FolderIsEmpty(_projectFolder + catalog[0]))
                    {
                        return (false, String.Format("Folder projektu {0} jest pusty.", project.Symbol));
                    }
                    if(!catalog[0].Equals(project.ToString()))
                    {
                        result = await RenameFolderAsync(project);
                        if(!result.Success)
                        {
                            return result;
                        }
                    }
                    var command = _folderCommandCreator.GetArchiveFolderCommand(project);
                    result = await FolderOperationTask(command, project.ToString(), false);
                    return (result.Success, String.Format("{0} {1}", command, result.Message));
                default:
                    return (false, String.Format("Istnieje więcej niż 1 folder projektu {0}", project.Symbol));
            }
        }

        public async Task<(bool Success, string Message)> RestoreFolderAsync(Project project)
        {
            (bool Success, string Message) result;
            var catalog = GetMatchingFolders(_archiveFolder, project);
            switch(catalog.Count())
            {
                case 0:
                    return(false, String.Format("Folder projektu {0} nie istnieje.", project.Symbol));
                case 1:
                    var command = _folderCommandCreator.GetRestoreFolderCommand(project);
                    result = await FolderOperationTask(command, project.ToString(), true);
                    return (result.Success, String.Format("{0} {1}", command, result.Message));
                default:
                    return (false, String.Format("Istnieje więcej niż 1 folder projektu {0}", project.Symbol));
            }
        }

        private Task<(bool Success, string Message)> FolderOperationTask(string command, string folder, bool exist = true)
        {
            return Task.Run(() =>
            {
                var message = command.RunShell();
                return (Directory.Exists(_projectFolder + folder) == exist, message);
            });
        }

        private bool FolderIsEmpty(string folder)
        {
            return Directory.GetFiles(folder, "*.*", SearchOption.AllDirectories).Length == 0;
        }

        private IList<string> GetMatchingFolders(string projectFolder, Project project)
        {
            return Directory.EnumerateDirectories(projectFolder)
                .Select(x => Path.GetFileName(x))
                .Where(n => {
                    var symbol = n.GetUntilOrEmpty("_");
                    return (!String.IsNullOrEmpty(symbol) && symbol.Equals(project.Symbol));
                })
                .ToList();
        }
    }
}
