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
        private readonly string _projectFolder;

        public FolderManager(IConfiguration configuration)
        {
            _configuration = configuration;
            _projectFolder = _configuration.GetSection("AppSettings:ProjectFolder").Value;
        }

        public async Task<(string Command, bool Success)> CreateOrUpdateFolderAsync(Project project)
        {
            var (Command, Success) = GetCreateOrRenameFolderCommand(project);
            if (Success)
            {
                return (Command, await FolderOperationTask(Command, project.ToString()));
            }
            else
            {
                return (Command, false);
            }
        }

        public async Task<(string Command, bool Success)> DeleteFolderAsync(Project project)
        {
            var (Command, Success) = GetDeleteFolderCommand(project);
            if(Success)
            {
                if(Command.Equals("Nothing to do."))
                {
                    return (Command, true);
                }
                else
                {
                    return (Command, !await FolderOperationTask(Command, project.ToString()));
                }
            }
            else
            {
                return (Command, false);
            }
        }

        private Task<bool> FolderOperationTask(string command, string folder)
        {
            return Task.Run(() =>
            {
                command.RunShell();
                if (Directory.Exists(_projectFolder + folder))
                {
                    return true;
                }
                return false;
            });
        }

        private (string Command, bool Success) GetCreateOrRenameFolderCommand(Project project)
        {
            var projectPath = _projectFolder + project.ToString();
            var catalog = GetFolders(project.Number, project.Stage.Sign);
            switch (catalog.Count())
            {
                case 0:
                    {
                        return (" " + _configuration.GetSection("AppSettings:CreateCommand").Value + " " + 
                            (Environment.OSVersion.Platform.Equals(PlatformID.Win32NT) ? _projectFolder + " " + project.ToString() : projectPath), true);
                    }
                case 1:
                    {
                        return (" " + _configuration.GetSection("AppSettings:RenameCommand").Value + " " + 
                            (Environment.OSVersion.Platform.Equals(PlatformID.Win32NT) ? _projectFolder + " " + Path.GetFileName(catalog[0]) + " " + project.ToString() : catalog[0] + " " + projectPath), true);
                    }
                default:
                    {
                        return ("Too many folders.", false);
                    }
            }
        }

        private (string Command, bool Success) GetDeleteFolderCommand(Project project)
        {
            (string, bool) result;
            var projectPath = _projectFolder + project.ToString();
            var catalog = GetFolders(project.Number, project.Stage.Sign);
            switch(catalog.Count())
            {
                case 0:
                    result = ("Nothing to do.", true);
                    break;
                case 1:
                    if (FolderIsEmpty(projectPath))
                    {
                        result = (" " + _configuration.GetSection("AppSettings:RemoveCommand").Value + " " +
                            (Environment.OSVersion.Platform.Equals(PlatformID.Win32NT) ? _projectFolder + " " + project.ToString() : projectPath), true);
                    }
                    else
                    {
                        result = ("Folder is not empty.", false);
                    }
                    break;
                default:
                    result = ("Too many folders.", false);
                    break;
            }

            return result;
        }

        private bool FolderIsEmpty(string folder)
        {
            return Directory.GetFiles(folder, "*.*", SearchOption.AllDirectories).Length == 0;
        }

        private IList<string> GetFolders(string number, string sign)
        {
            var catalog = Directory.EnumerateDirectories(_projectFolder);
            return catalog.Where(x => {
                var symbol = Path.GetFileName(x).GetUntilOrEmpty("_");
                return ((symbol.Substring(0, 4) == number) && (symbol[4..] == sign));
            }).ToList();
        }

        public static (string Number, string Sign) ProjectSymbolResolve(string catalog)
        {
            var symbol = Path.GetFileName(catalog).GetUntilOrEmpty("_");

            return (symbol.Substring(0, 4), symbol[4..]);
        }
    }
}
