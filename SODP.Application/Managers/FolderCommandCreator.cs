using Microsoft.Extensions.Configuration;
using SODP.Model;
using SODP.Model.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SODP.Application.Managers
{
    public abstract class FolderCommandCreator
    {
        protected readonly IConfiguration _configuration;
        protected readonly string _projectFolder;
        protected readonly string _archiveFolder;

        public FolderCommandCreator(IConfiguration configuration)
        {
            _configuration = configuration;
            _projectFolder = _configuration.GetSection("AppSettings:ActiveFolder").Value;
            _archiveFolder = _configuration.GetSection("AppSettings:ArchiveFolder").Value;
        }

        protected IList<string> GetFolders(Project project)
        {
            var catalog = Directory.EnumerateDirectories(_projectFolder);
            return catalog.Where(x => {
                var symbol = Path.GetFileName(x).GetUntilOrEmpty("_");
                return ((symbol.Substring(0, 4) == project.Number) && (symbol[4..] == project.Stage.Sign));
            }).ToList();
        }
    }
}
