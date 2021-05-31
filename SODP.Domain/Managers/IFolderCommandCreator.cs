using SODP.Model;
using SODP.Model.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SODP.Domain.Managers
{
    public interface IFolderCommandCreator
    {
        string GetCommandRenameFolder(Project project, string oldName);
        string GetCommandRenameFolder(Project project, string oldName, ProjectsFolder source);
        
        string GetCommandCreateFolder(Project project);
        string GetCommandRestoreFolder(Project project);
        string GetCommandDeleteFolder(Project project);
        string GetCommandArchiveFolder(Project project);
    }
}
