using Microsoft.Extensions.Configuration;
using SODP.Domain.Managers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using SODP.Model;
using SODP.Model.Extensions;

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

        public async Task<(string,bool)> CreateOrUpdateFolder(Project project)
        {
            var command = GetCreateOrRenameFolderCommand(project);
            if (command.Item2)
            {
                return (command.Item1, await FolderOperationTask(command.Item1, project.ToString()));
            }
            else
            {
                return (command.Item1, false);
            }
        }

        public async Task<(string,bool)> DeleteFolder(Project project)
        {
            var command = GetDeleteFolderCommand(project);
            if(command.Item2)
            {
                if(command.Item1.Equals("Nothing to do."))
                {
                    return (command.Item1, true);
                }
                else
                {
                    return (command.Item1, !await FolderOperationTask(command.Item1, project.ToString()));
                }
            }
            else
            {
                return (command.Item1, false);
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

        private (string, bool) GetCreateOrRenameFolderCommand(Project project)
        {
            var projectPath = _projectFolder + project.ToString();
            var catalog = GetFolders(project.Number, project.StageSign);
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

        private (string,bool) GetDeleteFolderCommand(Project project)
        {
            var projectPath = _projectFolder + project.ToString();
            var catalog = GetFolders(project.Number, project.StageSign);
            switch(catalog.Count())
            {
                case 0:
                    return ("Nothing to do.", true);
                case 1:
                    if (FolderIsEmpty(projectPath))
                    {
                        return (" " + _configuration.GetSection("AppSettings:RemoveCommand").Value + " " +
                            (Environment.OSVersion.Platform.Equals(PlatformID.Win32NT) ? _projectFolder + " " + project.ToString() : projectPath), true);
                    }
                    else
                    {
                        return("Folder is not empty.", false);
                    }
                default:
                    return ("Too many folders.", false);
            }
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
    }
}
