using Microsoft.Extensions.Configuration;
using SODP.Domain.Managers;
using SODP.Model;
using System;

namespace SODP.Application.Managers
{
    public class UnixFolderCommandCreator : FolderCommandCreator, IFolderCommandCreator
    {
        public UnixFolderCommandCreator(IConfiguration configuration) : base(configuration) { }
        
        public string GetCreateFolderCommand(Project project)
        {
            return String.Format(@" {0} {1} {2}_{3}",_configuration.GetSection("AppSettings:CreateCommand").Value, _projectFolder, project.Symbol, project.Title.Trim());
        }

        public string GetRenameFolderCommand(string oldFolder, Project project)
        {
            return " " + _configuration.GetSection("AppSettings:RenameCommand").Value + " " + _projectFolder + " " + project.Symbol + "_" + project.Title.Trim(); ;
        }

        public string GetArchiveFolderCommand(Project project)
        {
            return " " + _configuration.GetSection("AppSettings:ArchiveCommand").Value + " " + _projectFolder + " " + project.Symbol + "_" + project.Title.Trim(); ;
        }

        public string GetDeleteFolderCommand(Project project)
        {
            return " " + _configuration.GetSection("AppSettings:DeleteCommand").Value + " " + _projectFolder + " " + project.Symbol + "_" + project.Title.Trim(); ;
        }

    }
}
