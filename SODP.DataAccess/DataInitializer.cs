using Microsoft.Extensions.Configuration;
using SODP.Model;
using SODP.Model.Enums;
using SODP.Model.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace SODP.DataAccess
{
    public class DataInitializer : IDisposable
    {
        private readonly IConfiguration _configuration;
        private readonly SODPDBContext _context;

        public DataInitializer(IConfiguration configuration, SODPDBContext context)
        {
            _configuration = configuration;
            _context = context;
        }

        public void LoadData()
        {
            LoadStagesFromJSON();
            ImportProjectsFromStore();
        }

        private void LoadStagesFromJSON()
        {
            var _settingsPrefix = $"{Environment.OSVersion.Platform}Settings:";
            var jsonStages = _configuration.GetSection($"{_settingsPrefix}InitStagesJSON").Value;
            if ((_context.Stages.Count() == 0) && (File.Exists(jsonStages)))
            {
                var file = File.ReadAllText(jsonStages);
                var stages = JsonSerializer.Deserialize<List<Stage>>(file);
                _context.AddRange(stages);
                _context.SaveChanges();
            }
        }

        private void ImportProjectsFromStore()
        {
            var _settingsPrefix = $"{Environment.OSVersion.Platform}Settings:";
            ImportProjectsFromStore(_configuration.GetSection($"{_settingsPrefix}ActiveFolder").Value, ProjectStatus.Active);
            ImportProjectsFromStore(_configuration.GetSection($"{_settingsPrefix}ArchiveFolder").Value, ProjectStatus.Archived);
        }

        private void ImportProjectsFromStore(string projectsFolder, ProjectStatus status)
        {
            var directory = Directory.EnumerateDirectories(projectsFolder);
            var validator = new ProjectNameValidator();
            foreach (var item in directory)
            {
                if (!validator.Validate(item))
                {
                    continue;
                }
                var localization = Path.GetFileName(item);
                var sign = localization.GetUntilOrEmpty("_");
                var currentProject = new Project()
                {
                    Number = sign.Substring(0, 4),
                    Stage = new Stage() { Sign = sign[4..] },
                    Title = localization[(sign.Length + 1)..],
                    Status = status
                };
                var stage = _context.Stages.FirstOrDefault(x => x.Sign == currentProject.Stage.Sign);
                if (stage == null)
                {
                    stage = new Stage() { Sign = currentProject.Stage.Sign, Title = "" };
                }
                currentProject.Stage = stage;

                var project = _context.Projects.FirstOrDefault(x => x.Number == currentProject.Number && x.Stage.Sign == currentProject.Stage.Sign);
                if (project == null)
                {
                    _context.Projects.Add(currentProject);
                    _context.SaveChanges();
                }
            }
        }

        public void Dispose()
        {
        }
    }
}
