using Microsoft.Extensions.Configuration;
using SODP.Domain.Managers;
using SODP.Model;
using System.IO;

namespace SODP.Application.Managers
{
    public class Win32FolderCommandCreator : FolderCommandCreator, IFolderCommandCreator
    {
        public Win32FolderCommandCreator(IConfiguration configuration) : base(configuration) { }

        public string GetCreateFolderCommand(Project project)
        {
            return " " + _configuration.GetSection("AppSettings:CreateCommand").Value + " " + _projectFolder + " " + project.ToString();
        }

        public string GetRenameFolderCommand(string oldFolder, Project project)
        {
            return " " + _configuration.GetSection("AppSettings:RenameCommand").Value + " " +_projectFolder + " " + Path.GetFileName(oldFolder) + " " + project.ToString();
        }

        public string GetArchiveFolderCommand(Project project)
        {
            return " " + _configuration.GetSection("AppSettings:ArchiveCommand").Value + " " + _projectFolder + " " + project.ToString() + " " + _archiveFolder;
        }

        public string GetDeleteFolderCommand(Project project)
        {
            return " " + _configuration.GetSection("AppSettings:DeleteCommand").Value + " " + _projectFolder + " " + project.ToString();
        }

    }
}
