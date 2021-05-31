using SODP.Domain.Managers;
using SODP.Model;
using SODP.Model.Enums;
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
        private readonly FolderConfigurator _folderConfigurator;

        public FolderManager(FolderConfigurator folderConfigurator, IFolderCommandCreator folderCommandCreator)
        {
            _folderCommandCreator = folderCommandCreator;
            _folderConfigurator = folderConfigurator;
        }

        public async Task<(bool Success, string Message)> CreateFolderAsync(Project project)
        {
            (bool Success, string Message) result;
            string command;
            var catalog = GetMatchingFolders(_folderConfigurator.ActiveFolder, project);
            switch (catalog.Count())
            {
                case 0:
                    command = _folderCommandCreator.GetCommandCreateFolder(project);
                    result = await FolderOperationTask(command, project.ToString(), true);
                    return (result.Success, $"{command} {result.Message}");
                case 1:
                    command = _folderCommandCreator.GetCommandRenameFolder(project, catalog[0]);
                    result = await FolderOperationTask(command, project.ToString(), true);
                    return (result.Success, $"{command} {result.Message}");
                default:
                    return (false, $"Istnieje więcej niż 1 folder projektu {project.Symbol}");
            }
        }

        public async Task<(bool Success, string Message)> RenameFolderAsync(Project project)
        {
            return await RenameFolderAsync(project, ProjectsFolder.Active);
        }

        public async Task<(bool Success, string Message)> RenameFolderAsync(Project project, ProjectsFolder source)
        {
            var catalog = GetMatchingFolders(_folderConfigurator.GetProjectFolder(source), project);
            switch (catalog.Count())
            {
                case 0:
                    return await CreateFolderAsync(project);
                case 1:
                    var command = _folderCommandCreator.GetCommandRenameFolder(project, catalog[0]);
                    var (Success, Message) = await FolderOperationTask(command, project.ToString(), true);
                    return (Success, $"{command} {Message}");
                default:
                    return (false, $"Istnieje więcej niż 1 folder projektu {project.Symbol}");
            }
        }

        public async Task<(bool Success, string Message)> DeleteFolderAsync(Project project)
        {
            var catalog = GetMatchingFolders(_folderConfigurator.ActiveFolder, project);
            switch(catalog.Count())
            {
                case 0:
                    return(true, $"Folder projektu {project.Symbol} nie istnieje.");
                case 1:
                    if(!FolderIsEmpty($"{_folderConfigurator.ActiveFolder}{catalog[0]}"))
                    {
                        return (false,$"Folder projektu {project.Symbol} nie jest pusty.");
                    }
                    var command = _folderCommandCreator.GetCommandDeleteFolder(project);
                    var (Success, Message) = await FolderOperationTask(command, catalog[0], false);
                    return (Success, $"{command} {Message}");
                default:
                    return (false, $"Istnieje więcej niż 1 folder projektu {project.Symbol}");
            }
        }

        public async Task<(bool Success, string Message)> ArchiveFolderAsync(Project project)
        {
            (bool Success, string Message) result;
            var catalog = GetMatchingFolders(_folderConfigurator.ActiveFolder, project);
            switch(catalog.Count())
            {
                case 0:
                    return(false, $"Folder projektu {project.Symbol} nie istnieje.");
                case 1:
                    if(FolderIsEmpty(_folderConfigurator.ActiveFolder + catalog[0]))
                    {
                        return (false, $"Folder projektu {project.Symbol} jest pusty.");
                    }
                    if(!catalog[0].Equals(project.ToString()))
                    {
                        result = await RenameFolderAsync(project);
                        if(!result.Success)
                        {
                            return result;
                        }
                    }
                    var command = _folderCommandCreator.GetCommandArchiveFolder(project);
                    result = await FolderOperationTask(command, project.ToString(), false);
                    return (result.Success, $"{command} {result.Message}");
                default:
                    return (false, $"Istnieje więcej niż 1 folder projektu {project.Symbol}");
            }
        }

        public async Task<(bool Success, string Message)> RestoreFolderAsync(Project project)
        {
            (bool Success, string Message) result;
            var catalog = GetMatchingFolders(_folderConfigurator.ArchiveFolder, project);
            switch(catalog.Count())
            {
                case 0:
                    return(false, $"Folder projektu {project.Symbol} nie istnieje.");
                case 1:
                    if (!catalog[0].Equals(project.ToString()))
                    {
                        result = await RenameFolderAsync(project);
                        if (!result.Success)
                        {
                            return result;
                        }
                    }
                    var command = _folderCommandCreator.GetCommandRestoreFolder(project);
                    result = await FolderOperationTask(command, catalog[0], true);
                    return (result.Success, $"{command} {result.Message}");
                default:
                    return (false, $"Istnieje więcej niż 1 folder projektu {project.Symbol}");
            }
        }

        private Task<(bool Success, string Message)> FolderOperationTask(string command, string folder, bool exist = true)
        {
            return Task.Run(() =>
            {
                var message = command.RunShell();
                return (Directory.Exists(_folderConfigurator.ActiveFolder + folder) == exist, message);
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
