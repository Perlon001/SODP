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
        private readonly IConfiguration _configuration;
        private readonly IFolderCommandCreator _folderCommandCreator;
        private readonly string _projectFolder;
        private readonly string _archiveFolder;

        public FolderManager(IConfiguration configuration, FolderCommandCreatorFactory factory)
        {
            _configuration = configuration;
            _folderCommandCreator = factory.GetFolderCommandCreator();
            _projectFolder = _configuration.GetSection("AppSettings:ActiveFolder").Value;
            _archiveFolder = _configuration.GetSection("AppSettings:ArchiveFolder").Value;
        }

        public async Task<(bool Success, string Message)> CreateFolderAsync(Project project)
        {
            (bool Success, string Message) result;
            string command;
            var catalog = GetMatchingFolders(project);
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
                    command = _folderCommandCreator.GetCreateFolderCommand(project);
                    return (false, String.Format("Istnieje więcej niż 1 folder projektu {0}", project.Symbol));
            }
        }

        public async Task<(bool Success, string Message)> RenameFolderAsync(Project project)
        {
            var catalog = GetMatchingFolders(project);
            switch(catalog.Count())
            {
                case 0:                    
                    return await CreateFolderAsync(project);
                case 1:
                    var command = _folderCommandCreator.GetRenameFolderCommand(Path.GetFileName(catalog[0]), project);
                    var result = await FolderOperationTask(command, project.ToString(), true);
                    return (result.Success, String.Format("{0} {1}", command, result.Message));
                default:
                    return (false, String.Format("Istnieje więcej niż 1 folder projektu {0}", project.Symbol));
            }
        }

        public async Task<(bool Success, string Message)> DeleteFolderAsync(Project project)
        {
            (bool Success, string Message) result;
            var catalog = GetMatchingFolders(project);
            var command = _folderCommandCreator.GetDeleteFolderCommand(project);
            switch(catalog.Count())
            {
                case 0:
                    return(false, String.Format("Folder projektu {0} nie istnieje.", project.Symbol));
                case 1:
                    result = await FolderOperationTask(command, Path.GetFileName(catalog[0]), false);
                    return (result.Success, String.Format("{0} {1}", command, result.Message));
                default:
                    command = _folderCommandCreator.GetCreateFolderCommand(project);
                    return (false, String.Format("Istnieje więcej niż 1 folder projektu {0}", project.Symbol));
            }
        }

        public async Task<(bool Success, string Message)> ArchiveFolderAsync(Project project)
        {
            (bool Success, string Message) result;
            var catalog = GetMatchingFolders(project);
            var command = _folderCommandCreator.GetArchiveFolderCommand(project);
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
                    result = await FolderOperationTask(command, project.ToString(), false);
                    return (result.Success, String.Format("{0} {1}", command, result.Message));
                default:
                    command = _folderCommandCreator.GetCreateFolderCommand(project);
                    return (false, String.Format("Istnieje więcej niż 1 folder projektu {0}", project.Symbol));
            }
        }

        public Task<(bool Success, string Message)> RestoreFolderAsync(Project project)
        {
            throw new NotImplementedException("Not implemented RestoreFolderAsync");

            // (bool Success, string Message) result;
            // var catalog = GetMatchingFolders(project);
            // var command = _folderCommandCreator.GetArchiveFolderCommand(project);
            // switch(catalog.Count())
            // {
            //     case 0:
            //         return(false, String.Format("Folder projektu {0} nie istnieje.", project.Symbol));
            //     case 1:
            //         if(FolderIsEmpty(_projectFolder + catalog[0]))
            //         {
            //             return (false, String.Format("Folder projektu {0} jest pusty.", project.Symbol));
            //         }
            //         if(!catalog[0].Equals(project.ToString()))
            //         {
            //             result = await RenameFolderAsync(project);
            //             if(!result.Success)
            //             {
            //                 return result;
            //             }
            //         }
            //         result = await FolderOperationTask(command, project.ToString(), false);
            //         return (result.Success, String.Format("{0} {1}", command, result.Message));
            //     default:
            //         command = _folderCommandCreator.GetCreateFolderCommand(project);
            //         return (false, String.Format("Istnieje więcej niż 1 folder projektu {0}", project.Symbol));
            // }
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

        private IList<string> GetMatchingFolders(Project project)
        {
            var catalog = Directory.EnumerateDirectories(_projectFolder);
            return catalog.Where(x => {
                var symbol = Path.GetFileName(x).GetUntilOrEmpty("_");
                return (!String.IsNullOrEmpty(symbol) && symbol.Equals(project.Symbol));
            }).ToList();
        }

        public static (string Number, string Sign) ProjectSymbolResolve(string catalog)
        {
            var symbol = Path.GetFileName(catalog).GetUntilOrEmpty("_");

            return (symbol.Substring(0, 4), symbol[4..]);
        }
    }
}
