using Microsoft.Extensions.Configuration;
using SODP.Domain.Managers;
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
            _projectFolder = _configuration.GetSection("AppSettings:ProjectFolder").Value;
            _archiveFolder = _configuration.GetSection("AppSettings:ArchiveFolder").Value;
        }

        public async Task<(string Command, bool Success)> CreateFolderAsync(Project project)
        {
            var catalog = GetFolders(project);
            var command = _folderCommandCreator.GetCreateFolderCommand(project);
            switch(catalog.Count())
            {
                case 0:
                    return (command, await FolderOperationTask(command, project.ToString()));
                case 1:
                    command = _folderCommandCreator.GetRenameFolderCommand(catalog[0], project);
                    return (command, await FolderOperationTask(command, project.ToString()));
                default:
                    return (command, false);
            }
        }

        public async Task<(string Command, bool Success)> RenameFolderAsync(Project project)
        {
            var catalog = GetFolders(project);
            if (catalog.Count() == 1)
            {
                var command = _folderCommandCreator.GetRenameFolderCommand(catalog[0], project);
                return (command, await FolderOperationTask(command, project.ToString()));
            }
            return (String.Format("Project {0} not exist", project.Symbol), false);
        }

        //public async Task<(string Command, bool Success)> CreateOrUpdateFolderAsync(Project project)
        //{
        //    var (Command, Success) = GetCreateOrRenameFolderCommand(project);
        //    if (Success)
        //    {
        //        return (Command, await FolderOperationTask(Command, project.ToString()));
        //    }
            
        //    return (Command, false);
        //}

        public async Task<(string Command, bool Success)> DeleteFolderAsync(Project project)
        {
            var command = _folderCommandCreator.GetDeleteFolderCommand(project);
            var catalog = GetFolders(project);
            if (catalog.Count() == 1)
            {
                return (command, !await FolderOperationTask(command, project.ToString()));
            }
            return (command, false);

            //var (Command, Success) = GetDeleteFolderCommand(project);
            //if(Success)
            //{
            //    if(Command.Equals("Nothing to do."))
            //    {
            //        return (Command, true);
            //    }
            //    else
            //    {
            //        return (Command, !await FolderOperationTask(Command, project.ToString()));
            //    }
            //}

            //return (Command, false);
        }

        public async Task<(string Command, bool Success)> ArchiveFolderAsync(Project project)
        {
            var command = _folderCommandCreator.GetArchiveFolderCommand(project);
            var catalog = GetFolders(project);
            if (catalog.Count() == 1)
            {
                return (command, !await FolderOperationTask(command, project.ToString()));
            }
            return (command, false);

            //var (Command, Success) = GetArchiveFolderCommand(project);
            //if (Success)
            //{
            //    if (Command.Equals("Nothing to do."))
            //    {
            //        return (Command, true);
            //    }
            //    else
            //    {
            //        return (Command, !await FolderOperationTask(Command, project.ToString()));
            //    }
            //}

            //return (Command, false);
        }

        private Task<bool> FolderOperationTask(string command, string folder, bool exist = true)
        {
            return Task.Run(() =>
            {
                command.RunShell();
                return Directory.Exists(_projectFolder + folder) == exist;
            });
        }

        private (string Command, bool Success) GetCreateOrRenameFolderCommand(Project project)
        {
            var unixCreateProjectArg = _projectFolder + project.Symbol + " _" + project.Title.Trim();
            var unixRenameProjectArg = _projectFolder + project.ToString();
            var catalog = GetFolders(project);
            switch (catalog.Count())
            {
                case 0:
                    {
                        return (" " + _configuration.GetSection("AppSettings:CreateCommand").Value + " " +
                            (Environment.OSVersion.Platform.Equals(PlatformID.Win32NT) ? _projectFolder + " " + project.Symbol + " _" + project.Title.Trim() : unixCreateProjectArg), true);
                    }
                case 1:
                    {
                        return (" " + _configuration.GetSection("AppSettings:RenameCommand").Value + " " +
                            (Environment.OSVersion.Platform.Equals(PlatformID.Win32NT) ? _projectFolder + " " + Path.GetFileName(catalog[0]) + " " + project.ToString() : catalog[0] + " " + unixRenameProjectArg), true);
                    }
                default:
                    {
                        return ("Too many folders.", false);
                    }
            }
        }

        //private (string Command, bool Success) GetDeleteFolderCommand(Project project)
        //{
        //    (string, bool) result;
        //    var projectPath = _projectFolder + project.ToString();
        //    var catalog = GetFolders(project.Number, project.Stage.Sign);
        //    switch(catalog.Count())
        //    {
        //        case 0:
        //            result = ("Nothing to do.", true);
        //            break;
        //        case 1:
        //            if (FolderIsEmpty(projectPath))
        //            {
        //                result = (" " + _configuration.GetSection("AppSettings:RemoveCommand").Value + " " +
        //                    (Environment.OSVersion.Platform.Equals(PlatformID.Win32NT) ? _projectFolder + " " + project.ToString() : projectPath), true);
        //            }
        //            else
        //            {
        //                result = ("Folder is not empty.", false);
        //            }
        //            break;
        //        default:
        //            result = ("Too many folders.", false);
        //            break;
        //    }

        //    return result;
        //}
        //private (string Command, bool Success) GetArchiveFolderCommand(Project project)
        //{
        //    (string, bool) result;
        //    var projectPath = _projectFolder + project.ToString();
        //    var catalog = GetFolders(project.Number, project.Stage.Sign);
        //    switch (catalog.Count())
        //    {
        //        case 0:
        //            result = ("Nothing to do.", true);
        //            break;
        //        case 1:
        //            result = (" " + _configuration.GetSection("AppSettings:ArchiveCommand").Value + " " +
        //                (Environment.OSVersion.Platform.Equals(PlatformID.Win32NT) ? _projectFolder + " " + project.ToString() : projectPath), true);
        //            break;
        //        default:
        //            result = ("Too many folders.", false);
        //            break;
        //    }

        //    return result;
        //}

        private bool FolderIsEmpty(string folder)
        {
            return Directory.GetFiles(folder, "*.*", SearchOption.AllDirectories).Length == 0;
        }

        private IList<string> GetFolders(Project project)
        {
            var catalog = Directory.EnumerateDirectories(_projectFolder);
            return catalog.Where(x => {
                var symbol = Path.GetFileName(x).GetUntilOrEmpty("_");
                return ((symbol.Substring(0, 4) == project.Number) && (symbol[4..] == project.Stage.Sign));
            }).ToList();
        }

        public static (string Number, string Sign) ProjectSymbolResolve(string catalog)
        {
            var symbol = Path.GetFileName(catalog).GetUntilOrEmpty("_");

            return (symbol.Substring(0, 4), symbol[4..]);
        }

    }
}
