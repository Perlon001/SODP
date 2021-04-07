using SODP.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace SODP.Domain.Managers
{
    public interface IFolderCommandCreator
    {
        string GetCreateFolderCommand(Project project);
        string GetRenameFolderCommand(string oldFolder, Project project);
        string GetDeleteFolderCommand(Project project);
        string GetArchiveFolderCommand(Project project);
    }
}
