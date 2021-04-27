using Microsoft.Extensions.Configuration;
using SODP.Domain.Validators;
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

        public void LoadStagesFromJSON(string jsonStages = "")
        {
            if((_context.Stages.Count() == 0) && (File.Exists(jsonStages)))
            {
                var file = File.ReadAllText(jsonStages);
                var stages = JsonSerializer.Deserialize<List<Stage>>(file);
                _context.AddRange(stages);
                _context.SaveChanges();
            }
        }

        public void ImportProjectsFromStore(ProjectStatus status)
        {
            IConfigurationSection section;
            switch (status)
            {
                case ProjectStatus.Active:
                    section = _configuration.GetSection("AppSettings:ActiveFolder");
                    break;
                case ProjectStatus.Archived:
                    section = _configuration.GetSection("AppSettings:ArchiveFolder");
                    break;
                default:
                    return;
            }
            ImportProjectsFromStore(section.Value, status);
            //if (_context.Projects.Count() == 0)
            //{
            //}
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
