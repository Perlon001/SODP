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

        // public async Task<(bool Success, string Message)> FolderOperationAsync(FolderOperation operation, Project project)
        // {
        //     switch(operation)
        //     {
        //         case FolderOperation.Create:
        //             return await CreateFolderAsync(project);
        //         case FolderOperation.Rename:
        //             return await RenameFolderAsync(project);
        //         case FolderOperation.Archive:
        //             return await ArchiveFolderAsync(project);
        //         case FolderOperation.Delete:
        //             return await DeleteFolderAsync(project);
        //         default:
        //             return (false, "Nieznana operacja na folderze projektu");
        //     }
        // }


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
                    return (false, String.Format("{0} {1} {2}", command, "Istnieje więcej niż 1 folder projektu.", project.Symbol));
            }
        }

        public async Task<(bool Success, string Message)> RenameFolderAsync(Project project)
        {
            var catalog = GetMatchingFolders(project);
            var command = _folderCommandCreator.GetRenameFolderCommand(catalog[0], project);
            switch(catalog.Count())
            {
                case 0:                    
                    return (false, String.Format("{0} {1}", command, "Folder projektu nie sitnieje."));
                case 1:
                    var result = await FolderOperationTask(command, project.ToString(), true);
                    return (result.Success, String.Format("{0} {1}", command, result.Message));
                default:
                    return (false, String.Format("{0} {1} {2}", command, "Istnieje więcej niż 1 folder projektu.", project.Symbol));
            }
        }

        public async Task<(bool Success, string Message)> DeleteFolderAsync(Project project)
        {
            var catalog = GetMatchingFolders(project);
            var command = _folderCommandCreator.GetDeleteFolderCommand(project);
            return await DeleteOrArchiveActionAsync(command, project);
        }

        public async Task<(bool Success, string Message)> ArchiveFolderAsync(Project project)
        {
            var command = _folderCommandCreator.GetArchiveFolderCommand(project);
            return await DeleteOrArchiveActionAsync(command, project);
        }

        private async Task<(bool Success, string Message)> CreateOrRenameActionAsync(string command, Project project)
        {
            var catalog = GetMatchingFolders(project);
            switch(catalog.Count())
            {
                case 0:
                    return(false, String.Format("{0} {1}", command, "Folder nie istnieje."));
                case 1:
                    var result = await FolderOperationTask(command, project.ToString(), true);
                    return (result.Success, String.Format("{0} {1}",command,result.Message));
                default:
                    return(false, String.Format("{0} {1}", command,"Więcej niż 1 folder projektu.", project.Symbol));
            }
        }

        private async Task<(bool Success, string Message)> DeleteOrArchiveActionAsync(string command, Project project)
        {
            var catalog = GetMatchingFolders(project);
            switch(catalog.Count())
            {
                case 0:
                    return(false, String.Format("{0} {1}", command,"Folder nie istnieje."));
                case 1:
                    var result = await FolderOperationTask(command, project.ToString(), false);
                    return (result.Success, String.Format("{0} {1}",command,result.Message));
                default:
                    return(false, String.Format("{0} {1}", command,"Istnieje więcej niż 1 folder projektu."));
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
